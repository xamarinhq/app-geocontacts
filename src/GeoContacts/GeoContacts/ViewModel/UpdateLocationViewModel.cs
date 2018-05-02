using GeoContacts.Model;
using GeoContacts.Resources;
using GeoContacts.Services;
using Microsoft.AppCenter.Analytics;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace GeoContacts.ViewModel
{
    public class UpdateLocationViewModel : ViewModelBase
    {

        public UpdateLocationViewModel()
        {
            UpdateLocationCommand = new Command(async () => await ExecuteUpdateLocationCommand());
            SyncCommand = new Command(async () => await ExecuteSyncCommand());
            AddMoodCommand = new Command(async () => await ExecuteAddMoodCommand());
        }

        string currentLocation;
        public string CurrentLocation
        {
            get => currentLocation;
            set => SetProperty(ref currentLocation, value);
        }

        public ICommand SyncCommand { get; }

        async Task ExecuteSyncCommand()
        {
            if (IsBusy)
                return;

            if (location == null)
                return;

            if (!await CheckConnectivityAsync())
                return;

            var authResult = await AuthenticationService.Login();
            if (authResult == null)
                return;

            try
            {
                IsBusy = true;

                UpdateMessage = AppResources.UpdateLocationBackend;

                //it is okay if we don't have the address we will send it to the backend to diagnose
                await DataService.UpdateLocationAsync(location, placemark, mood, authResult.AccessToken);

                UpdateMessage = AppResources.UpdatingLocationUpdated;

                Analytics.TrackEvent("LocationUpdates");
            }
            catch (Exception ex)
            {
                UpdateMessage = string.Empty;
                await Dialogs.AlertAsync(null, AppResources.UpdateLocationError, AppResources.OK);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public ICommand AddMoodCommand { get; }
        async Task ExecuteAddMoodCommand()
        {
            if (IsBusy)
                return;

            string result = "Error";
            MediaFile file = null;

            try
            {
                IsBusy = true;
                await CrossMedia.Current.Initialize();

                if (CrossMedia.Current.IsTakePhotoSupported)
                {
                    file = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions
                    {
                        Directory = "Sample",
                        Name = "face.jpg",
                        PhotoSize = PhotoSize.Medium,
                        DefaultCamera = CameraDevice.Front
                    });
                }
                else
                {
                    file = await CrossMedia.Current.PickPhotoAsync();
                }


                if (file == null)
                    result = "No photo taken.";
                else
                {
                    result = mood = await EmotionService.GetEmotionAsync(file.GetStream());
                }

                file.Dispose();
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }
            finally
            {
                IsBusy = false;
            }

            await Dialogs.AlertAsync(null, result, AppResources.OK);
        }
        public ICommand UpdateLocationCommand { get; }

        Xamarin.Essentials.Location location = null;
        Placemark placemark = null;
        string mood = string.Empty;

        async Task ExecuteUpdateLocationCommand()
        {
            if (IsBusy)
                return;
            
            try
            {
                IsBusy = true;

                UpdateMessage = AppResources.UpdatingLocation;

                var position = await GeolocationService.GetCurrentPositionAsync();

                if (position == null)
                    throw new Exception("Unable to get location.");

                CurrentLocation = $"{position.Latitude}, {position.Longitude}";

                UpdateMessage = AppResources.UpdateLocationGeocoding;

                var address = await GeolocationService.GetAddressAsync(position);

                if (address != null)
                    CurrentLocation = $"{address.Locality}, {address.AdminArea ?? string.Empty} {address.CountryCode}";

                UpdateMessage = string.Empty;
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
