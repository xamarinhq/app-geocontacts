using AwesomeContacts.Model;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
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
    }
}
