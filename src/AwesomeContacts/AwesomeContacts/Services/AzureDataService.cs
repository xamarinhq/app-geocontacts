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
        const string accountURL = @"{account url}";
        const string databaseId = @"{database name}";
        const string collectionId = @"UserItems";
        const string resourceTokenBrokerURL = @"{resource token broker base url, e.g. https://xamarin.azurewebsites.net}";

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
            httpClient = new HttpClient();
            DocClient = new DocumentClient(new Uri(CommonConstants.CosmosDbUrl), CommonConstants.CosmosAuthKey);

            DocClient.OpenAsync();
        }

        public IEnumerable<Contact> GetAll()
        {
            var allCDAs = DocClient.CreateDocumentQuery<Contact>(allCDACollectionLink)
                                   .OrderBy(cda => cda.Name).ToList();

            string imgSrc = "";
            foreach (var cda in allCDAs)
            {
                if (cda.Image.TryGetValue("Src", out imgSrc))
                    cda.PhotoUrl = $"https://developer.microsoft.com/en-us/advocates/{imgSrc}";

                var twitterUserName = cda.Twitter.Substring(
                    cda.Twitter.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1);

                cda.TwitterHandle = $"@{twitterUserName}";
            }

            return allCDAs;
        }

        public Task<Contact> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Contact> GetNearbyAsync(double userLongitude, double userLatitude)
        {
            var allCDAs = GetAll();

            var userPoint = new Point(userLongitude, userLatitude);
            var feedOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };

            // Find the CDAs with hometowns by the user
            var hometownCDAs = DocClient.CreateDocumentQuery<Contact>(allCDACollectionLink, feedOptions)
                .Where(cda => userPoint.Distance(cda.Hometown.Position) < maximumCDADistance)
                .ToList();

            // Find the CDAs who checked in within the last 24 hours
            var midnightYesterday = DateTimeOffset.UtcNow.AddDays(-1).Date;

            var latestClosestPositions = DocClient.CreateDocumentQuery<LocationUpdate>(locationCollectionLink, feedOptions)
                                           .Where(ll => ll.InsertTime > midnightYesterday)
                                           .Where(ll => userPoint.Distance(ll.Position) < maximumCDADistance)
                                           .ToList();

            // For now assuming only one check in per day
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

        public async Task<T> GetAsync<T>(string url, int hours = 2, bool forceRefresh = false)
        {
            var json = string.Empty;

            //check if we are connected, else check to see if we have valid data
            if (!CrossConnectivity.Current.IsConnected)
                json = Barrel.Current.Get(url);
            else if (!forceRefresh && !Barrel.Current.IsExpired(url))
                json = Barrel.Current.Get(url);

            try
            {
                //skip web request because we are using cached data
                if (string.IsNullOrWhiteSpace(json))
                {
                    json = await httpClient.GetStringAsync(url);
                    Barrel.Current.Add(url, json, TimeSpan.FromHours(hours));
                }
                return await Task.Run(() => JsonConvert.DeserializeObject<T>(json));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable to get information from server {ex}");
                //probably re-throw here :)
            }

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
