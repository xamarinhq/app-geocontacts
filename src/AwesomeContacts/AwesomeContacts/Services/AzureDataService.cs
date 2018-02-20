using AwesomeContacts.Model;
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
using AwesomeContacts.Helpers;
using System.Linq;
using AwesomeContacts.SharedModels;
using Microsoft.Azure.Documents.Linq;
using Microsoft.Azure.Documents.Spatial;

namespace AwesomeContacts.Services
{
    public class AzureDataService : IDataService
    {
        const string cdaCacheKey = "allcdas";
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
            if (CommonConstants.CosmosDbUrl == "CDC_URL" || CommonConstants.CosmosAuthKey == "CDC_AUTH")
                throw new Exception("CosmosDB not configured, please update Helpers/CommonConstants.cs");

            httpClient = new HttpClient();
            DocClient = new DocumentClient(new Uri(CommonConstants.CosmosDbUrl), CommonConstants.CosmosAuthKey);
        }

        public async Task Initialize()
        {
            await DocClient.OpenAsync();
        }

        public async Task<IEnumerable<Contact>> GetAllAsync()
        {
            var cache = GetCache<List<Contact>>(cdaCacheKey);


            var allCDAQuery = DocClient.CreateDocumentQuery<Contact>(allCDACollectionLink)
                                   .OrderBy(cda => cda.Name)
                                   .AsDocumentQuery();

            List<Contact> allCDAs = new List<Contact>();
            while (allCDAQuery.HasMoreResults)
            {
                allCDAs.AddRange(await allCDAQuery.ExecuteNextAsync<Contact>());
            }

            string imgSrc = "";
            foreach (var cda in allCDAs)
            {
                if (cda.Image.TryGetValue("Src", out imgSrc))
                    cda.PhotoUrl = $"https://developer.microsoft.com/en-us/advocates/{imgSrc}";

                var twitterUserName = cda.Twitter.Substring(
                    cda.Twitter.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1);

                cda.TwitterHandle = $"@{twitterUserName}";
            }

            MonkeyCache.FileStore.Barrel.Current.Add<List<Contact>>(cdaCacheKey, allCDAs, TimeSpan.FromHours(2));

            return allCDAs;
        }

        public Task<Contact> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Contact>> GetNearbyAsync(double userLongitude, double userLatitude)
        {
            var allCDAs = await GetAllAsync();

            var userPoint = new Point(userLongitude, userLatitude);
            var feedOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };

            // Find the CDAs with hometowns by the user
            var hometownCDAQuery = DocClient.CreateDocumentQuery<Contact>(allCDACollectionLink, feedOptions)
                .Where(cda => userPoint.Distance(cda.Hometown.Position) < maximumCDADistance)
                .AsDocumentQuery();

            List<Contact> hometownCDAs = new List<Contact>();
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

            List<LocationUpdate> latestClosestPositions = new List<LocationUpdate>();
            while (latestClosestPositionsQuery.HasMoreResults)
            {
                latestClosestPositions.AddRange(await latestClosestPositionsQuery.ExecuteNextAsync<LocationUpdate>());
            }

            // Make sure only one upate per person included
            // todo: make sure it's the most recent update
            latestClosestPositions = latestClosestPositions.Distinct(new LocationUpdateCompare()).ToList();

            // Remove any hometownCDAs that are in the latest closest position
            foreach (var closeCDA in latestClosestPositions)
            {
                hometownCDAs.RemoveAll(cda => closeCDA.UserPrincipalName == cda.UserPrincipalName);
            }

            List<Contact> allCDAsNearby = new List<Contact>();
            // Add CDAs in the latest closest position
            foreach (var closeCDA in latestClosestPositions)
            {
                var foundCDA = allCDAs.First(cda => cda.UserPrincipalName == closeCDA.UserPrincipalName);
                foundCDA.CurrentLocation = closeCDA.Position;

                allCDAsNearby.Add(foundCDA);
            }
            // Finally create a list of CDAs that are close by
            hometownCDAs.ForEach(cda => cda.CurrentLocation = cda.Hometown.Position);
            allCDAsNearby.AddRange(hometownCDAs);

            return allCDAsNearby;
        }

        public T GetCache<T>(string key, bool forceRefresh = false)
        {
            //check if we are connected, else check to see if we have valid data
            if (!CrossConnectivity.Current.IsConnected)
                return Barrel.Current.Get<T>(key);
            else if (!forceRefresh && !Barrel.Current.IsExpired(key))
               return Barrel.Current.Get<T>(key);

            return default(T);
        }

        public async Task UpdateLocationAsync(Plugin.Geolocator.Abstractions.Position position, Address address, string accessToken)
        {
            //This should call an azure service
            try
            {
                var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                var location = new AwesomeContacts.SharedModels.LocationUpdate
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
