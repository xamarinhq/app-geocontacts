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

namespace AwesomeContacts.Services
{
    public class AzureDataService : IDataService
    {
        const string accountURL = @"{account url}";
        const string databaseId = @"{database name}";
        const string collectionId = @"UserItems";
        const string resourceTokenBrokerURL = @"{resource token broker base url, e.g. https://xamarin.azurewebsites.net}";

        private Uri collectionLink = UriFactory.CreateDocumentCollectionUri(databaseId, collectionId);

        public DocumentClient Client { get; private set; }
        public string UserId { get; private set; }
        HttpClient httpClient;

        public AzureDataService()
        {
            httpClient = new HttpClient();
        }

        public Task<IEnumerable<Contact>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Contact> GetAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Contact>> GetNearbyAsync()
        {
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

        public Task UpdateLocationAsync(Position position, Address address)
        {
            //This should call an azure service
            return Task.CompletedTask;
        }
    }
}
