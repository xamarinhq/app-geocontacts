using GeoContacts.Model;
using Microsoft.Azure.Documents.Client;
using MonkeyCache.FileStore;
using Newtonsoft.Json;
using Plugin.Connectivity;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GeoContacts.Helpers;
using System.Linq;
using GeoContacts.SharedModels;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents.Spatial;
using MvvmHelpers;
using GeoContacts.Resources;

namespace GeoContacts.Services
{
    public class AzureDataService : IDataService
    {
        const string cdaCacheKey = "allcdas2";
        const int maximumCDADistance = 50000; //meters

        readonly Uri locationCollectionLink = UriFactory.CreateDocumentCollectionUri(
                CommonConstants.CDADatabaseId, CommonConstants.CDALocationCollectionId
            );

        readonly Uri allCDACollectionLink = UriFactory.CreateDocumentCollectionUri(
                CommonConstants.CDADatabaseId, CommonConstants.AllCDACollectionId
            );

        public DocumentClient DocClient { get; private set; }
        public string UserId { get; private set; }
        HttpClient httpClient;

        public AzureDataService()
        {
            httpClient = new HttpClient();
            DocClient = new DocumentClient(new Uri(CommonConstants.CosmosDbUrl), CommonConstants.CosmosAuthKey);
        }

        public async Task Initialize()
        {
            await DocClient.OpenAsync();
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            var cache = GetCache(cdaCacheKey);

            if (cache != null)
                return cache;


            var allCDAQuery = DocClient.CreateDocumentQuery<Contact>(allCDACollectionLink)
                                   .OrderBy(cda => cda.Name)
                                   .AsDocumentQuery();

            List<Contact> allCDAs = new List<Contact>();
            while (allCDAQuery.HasMoreResults)
            {
                allCDAs.AddRange(await allCDAQuery.ExecuteNextAsync<Contact>());
            }

            foreach (var cda in allCDAs)
            {
                if (cda.Image.TryGetValue("Src", out string imgSrc))
                {
                    // The image source may be a full URL or a partial one
                    if (imgSrc.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                        cda.PhotoUrl = imgSrc;
                    else
                        cda.PhotoUrl = $"https://developer.microsoft.com/en-us/advocates/{imgSrc}";
                }

                var twitterUserName = cda.Twitter.Substring(
                    cda.Twitter.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1);

                cda.TwitterHandle = $"@{twitterUserName}";
            }

            var json = JsonConvert.SerializeObject(allCDAs.ToArray());
            MonkeyCache.FileStore.Barrel.Current.Add(cdaCacheKey, json, TimeSpan.FromHours(2));

            return allCDAs;
        }

        public Task<Contact> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Grouping<string, Contact>>> GetNearbyAsync(double userLongitude, double userLatitude)
        {
            var groupedNearby = new List<Grouping<string, Contact>>();
            var allCDAs = await GetAllAsync();

            var userPoint = new Point(userLongitude, userLatitude);
            var feedOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };

            // Find the CDAs with hometowns by the user
            var hometownCDAQuery = DocClient.CreateDocumentQuery<Contact>(allCDACollectionLink, feedOptions)
                .Where(cda => userPoint.Distance(cda.Hometown.Position) < maximumCDADistance)
                .AsDocumentQuery();

            var hometownCDAs = new List<Contact>();
            while (hometownCDAQuery.HasMoreResults)
            {
                hometownCDAs.AddRange(await hometownCDAQuery.ExecuteNextAsync<Contact>());
            }

            // Find the CDAs who checked in within the last 7Days
            var daysAgo = DateTimeOffset.UtcNow.AddDays(-7).Date;

            var latestClosestPositionsQuery = DocClient.CreateDocumentQuery<LocationUpdate>(locationCollectionLink, feedOptions)
                                                       .Where(ll => ll.InsertTime > daysAgo)
                                                       .Where(ll => userPoint.Distance(ll.Position) < maximumCDADistance)
                                                       .AsDocumentQuery();

            var latestClosestPositions = new List<LocationUpdate>();
            while (latestClosestPositionsQuery.HasMoreResults)
            {
                latestClosestPositions.AddRange(await latestClosestPositionsQuery.ExecuteNextAsync<LocationUpdate>());
            }

            // Make sure only the most recent update per CDA is grabbed
            var mostRecentCDACheckins = from lcp in latestClosestPositions
                                        group lcp by lcp.UserPrincipalName into g
                                        select g.OrderByDescending(t => t.InsertTime).FirstOrDefault();

            // Remove any hometownCDAs that are in the latest closest position
            foreach (var cdaCheckin in mostRecentCDACheckins)
            {
                hometownCDAs.RemoveAll(cda => cdaCheckin.UserPrincipalName == cda.UserPrincipalName);
            }

            // Create a list that will hold all the CDAs that are nearby
            var allCDAsNearby = new List<Contact>();

            // Add CDAs in the latest closest position
            foreach (var cdaCheckin in mostRecentCDACheckins)
            {
                // Use the Contact class - so match up a cda check-in location class to their corresponding contact
                var foundCDA = allCDAs.First(cda => cda.UserPrincipalName == cdaCheckin.UserPrincipalName);

                // Then mark their curent location
                foundCDA.CurrentLocation = cdaCheckin.Position;

                allCDAsNearby.Add(foundCDA);
            }



            // Make sure the current location of the CDAs whose hometowns are near also have the current location set properly
            hometownCDAs.ForEach(cda => cda.CurrentLocation = cda.Hometown.Position);

            if(allCDAsNearby.Count > 0)
                groupedNearby.Add(new Grouping<string, Contact>(AppResources.RecentCheckin, allCDAsNearby));
            if(hometownCDAs.Count > 0)
                groupedNearby.Add(new Grouping<string, Contact>(AppResources.Hometown, hometownCDAs));

            foreach (var grouped in groupedNearby)
            {
                foreach (var cda in grouped.Items)
                {
                    if (cda.Image.TryGetValue("Src", out string imgSrc))
                    {
                        // Image source could be a full url or a partial
                        if (imgSrc.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                            cda.PhotoUrl = imgSrc;
                        else
                            cda.PhotoUrl = $"https://developer.microsoft.com/en-us/advocates/{imgSrc}";
                    }

                    var twitterUserName = cda.Twitter.Substring(
                        cda.Twitter.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1);

                    cda.TwitterHandle = $"@{twitterUserName}";
                }
            }

            return groupedNearby;
        }

        public List<Contact> GetCache(string key, bool forceRefresh = false)
        {
            var json = string.Empty;
            //check if we are connected, else check to see if we have valid data
            if (!CrossConnectivity.Current.IsConnected)
                json = Barrel.Current.Get(key);
            else if (!forceRefresh && !Barrel.Current.IsExpired(key))
                json = Barrel.Current.Get(key);

            if (!string.IsNullOrWhiteSpace(json))
                return JsonConvert.DeserializeObject<Contact[]>(json).ToList();

            return null;
        }

        public async Task UpdateLocationAsync(Plugin.Geolocator.Abstractions.Position position, Address address, string accessToken)
        {
            //This should call an azure service
            try
            {
                var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                var location = new GeoContacts.SharedModels.LocationUpdate
                {
                    Country = address.CountryCode,
                    Position = new Point(position.Longitude, position.Latitude),
                    State = address.AdminArea,
                    Town = address.Locality
                };

                var json = JsonConvert.SerializeObject(location);
                var content = new StringContent(json);
                var resp = await client.PostAsync(CommonConstants.FunctionUrl, content);

                var respBody = await resp.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR: {ex.Message}");
            }
        }
    }
}
