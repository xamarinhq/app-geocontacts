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
        Task<IEnumerable<Contact>> GetAllAsync();
        Task<Contact> GetAsync(string id);
        Task<IEnumerable<Contact>> GetNearbyAsync();

        Task UpdateLocationAsync(Position position, Address address);

    }
}
