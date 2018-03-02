using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeoContacts.Services
{
    public static class Geolocation
    {
        public static async Task<Position> GetCurrentPositionAsync()
        {
            Position position = null;
            try
            {
                var locator = CrossGeolocator.Current;
                locator.DesiredAccuracy = 100;

                position = await locator.GetLastKnownLocationAsync();

                if (position != null)
                {
                    //got a cahched position, so let's use it.
                    return position;
                }

                if (!locator.IsGeolocationAvailable || !locator.IsGeolocationEnabled)
                {
                    //not available or enabled
                    return null;
                }

                position = await locator.GetPositionAsync(TimeSpan.FromSeconds(20), null, true);

            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get location: " + ex);
            }

            if (position == null)
                return null;

            var output = string.Format("Time: {0} \nLat: {1} \nLong: {2} \nAltitude: {3} \nAltitude Accuracy: {4} \nAccuracy: {5} \nHeading: {6} \nSpeed: {7}",
                position.Timestamp, position.Latitude, position.Longitude,
                position.Altitude, position.AltitudeAccuracy, position.Accuracy, position.Heading, position.Speed);

            Debug.WriteLine(output);

            return position;
        }

        public static async Task<Address> GetAddressAsync(Position position)
        {
            try
            {
                var locator = CrossGeolocator.Current;
                var addresses = await locator.GetAddressesForPositionAsync(position,null);
                var address = addresses.FirstOrDefault();

                if (address == null)
                    Console.WriteLine("No address found for position.");
                else
                    Console.WriteLine("Addresss: {0} {1} {2}", address.Thoroughfare, address.Locality, address.CountryCode);

                return address;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to get address: " + ex);
            }

            return null;
        }
    }
}
