using AwesomeContacts.Model;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeContacts.Services
{
    public interface IDataService
    {
        IEnumerable<Contact> GetAll();
        Task<Contact> GetAsync(string id);
        Task<IEnumerable<Contact>> GetNearbyAsync();

        Task UpdateLocationAsync(Plugin.Geolocator.Abstractions.Position position, Address address, string accessToken);

    }
}
