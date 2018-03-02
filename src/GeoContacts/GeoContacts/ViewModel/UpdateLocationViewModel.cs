using GeoContacts.Resources;
using GeoContacts.Services;
using Microsoft.AppCenter.Analytics;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GeoContacts.ViewModel
{
    public class UpdateLocationViewModel : ViewModelBase
    {

        public UpdateLocationViewModel()
        {
            UpdateLocationCommand = new Command(async () => await ExecuteUpdateLocationCommand());
        }

        string currentLocation;
        public string CurrentLocation
        {
            get => currentLocation;
            set => SetProperty(ref currentLocation, value);
        }

        public ICommand UpdateLocationCommand { get; }

        async Task ExecuteUpdateLocationCommand()
        {
            if (IsBusy)
                return;

            if (!await CheckConnectivityAsync())
                return;

            var authResult = await AuthenticationService.Login();
            if (authResult == null)
                return;

            try
            {
                IsBusy = true;

                UpdateMessage = AppResources.UpdatingLocation;

                var position = await Geolocation.GetCurrentPositionAsync();

                if (position == null)
                    throw new Exception("Unable to get location.");

                CurrentLocation = $"{position.Latitude}, {position.Longitude}";

                UpdateMessage = AppResources.UpdateLocationGeocoding;

                var address = await Geolocation.GetAddressAsync(position);

                if (address != null)
                    CurrentLocation = $"{address.Locality}, {address.AdminArea ?? string.Empty} {address.CountryCode}";

                UpdateMessage = AppResources.UpdateLocationBackend;

                //it is okay if we don't have the address we will send it to the backend to diagnose
                await DataService.UpdateLocationAsync(position, address, authResult.AccessToken);

                UpdateMessage = AppResources.UpdatingLocationUpdated;

                Analytics.TrackEvent("LocationUpdates");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to get location: " + ex);
                UpdateMessage = string.Empty;
                await Dialogs.AlertAsync(null, AppResources.UpdateLocationError, AppResources.OK);
            }
            finally
            {
                IsBusy = false;
            }
        }


    }
}
