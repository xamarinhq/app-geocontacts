using GeoContacts.Model;
using MvvmHelpers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace GeoContacts.Services
{
    public interface IDataService
    {
        Task Initialize();
        Task<IEnumerable<Contact>> GetAllAsync();
        Task<Contact> GetAsync(string id);
        Task<IEnumerable<Grouping<string, Contact>>> GetNearbyAsync(double userLongitude, double userLatitude);

        Task UpdateLocationAsync(Xamarin.Essentials.Location position, Placemark address, string mood, string accessToken);

    }
}
