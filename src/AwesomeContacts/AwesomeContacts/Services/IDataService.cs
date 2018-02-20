using AwesomeContacts.Model;
using Plugin.Geolocator.Abstractions;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AwesomeContacts.Services
{
    public interface IDataService
    {
        Task Initialize();
        Task<IEnumerable<Contact>> GetAllAsync();
        Task<Contact> GetAsync(string id);
        Task<IEnumerable<Contact>> GetNearbyAsync(double userLongitude, double userLatitude);

        Task UpdateLocationAsync(Position position, Address address, string accessToken);

    }
}
