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

        //private Uri collectionLink = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);

        public DocumentClient DocClient { get; private set; }
        public string UserId { get; private set; }
        HttpClient httpClient;

        public AzureDataService()
        {
            httpClient = new HttpClient();
            DocClient = new DocumentClient(new Uri(CommonConstants.CosmosDbUrl), CommonConstants.CosmosAuthKey);
        }

        public IEnumerable<Contact> GetAll()
        {
            var allCDACollectionLink = UriFactory.CreateDocumentCollectionUri(
                CommonConstants.CDADatabaseId, CommonConstants.AllCDACollectionId
            );

            var allCDAs = DocClient.CreateDocumentQuery<Contact>(allCDACollectionLink)
                                   .OrderBy(cda => cda.Name).ToList();

            foreach (var cda in allCDAs)
            {
                string imgSrc = "";
                if (cda.Image.TryGetValue("Src", out imgSrc))
                    cda.PhotoUrl = $"https://developer.microsoft.com/en-us/advocates/{imgSrc}";

                cda.TwitterHandle = cda.Twitter.Substring(
                    cda.Twitter.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1);
            }

            return allCDAs;
        }

        public Task<Contact> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Contact>> GetNearbyAsync()
        {
            var locationLink = UriFactory.CreateDocumentCollectionUri(
                CommonConstants.CDADatabaseId, CommonConstants.CDALocationCollectionId
            );
            var allCDALink = UriFactory.CreateDocumentCollectionUri(
                CommonConstants.CDADatabaseId, CommonConstants.AllCDACollectionId
            );

            var lu = new LocationUpdate();

            // Get a distinct list of the latest locations


            var feedOptions = new FeedOptions { MaxItemCount = -1, EnableCrossPartitionQuery = true };

            var point = new Point(-122.130603, 47.6451);
            var latestLocations = DocClient.CreateDocumentQuery<LocationUpdate>(locationLink, feedOptions)
                                           .Where(ll => point.Distance(ll.Position) < 50000)
                                           .OrderByDescending(l => l.InsertTime).ToList();


            throw new NotImplementedException();
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
