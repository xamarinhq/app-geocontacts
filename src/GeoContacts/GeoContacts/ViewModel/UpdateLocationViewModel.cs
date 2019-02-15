using GeoContacts.Helpers;
using GeoContacts.Resources;
using GeoContacts.Services;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
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
            SyncCommand = new Command(async () => await ExecuteSyncCommand(), () =>
            location != null && IsNotBusy);
            AddMoodCommand = new Command(async () => await ExecuteAddMoodCommand());
        }

       
        string currentLocation;
        public string CurrentLocation
        {
            get => currentLocation;
            set => SetProperty(ref currentLocation, value);
        }

        public Command SyncCommand { get; }

        async Task ExecuteSyncCommand()
        {
            if (IsBusy)
                return;

            if (location == null)
            {

                await Dialogs.AlertAsync(null, AppResources.ErrorNeedLocation, AppResources.OK);
                return;
            }

            if (!await CheckConnectivityAsync())
                return;

            var authResult = await AuthenticationService.Login();
            if (authResult == null)
                return;

            try
            {
                IsBusy = true;
                SyncCommand.ChangeCanExecute();

                UpdateMessage = AppResources.UpdateLocationBackend;

                //it is okay if we don't have the address we will send it to the backend to diagnose
                await DataService.UpdateLocationAsync(location, placemark, mood, authResult.AccessToken);

                UpdateMessage = AppResources.UpdatingLocationUpdated;

                Analytics.TrackEvent("LocationUpdates");
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                UpdateMessage = string.Empty;
                await Dialogs.AlertAsync(null, AppResources.UpdateLocationError, AppResources.OK);
            }
            finally
            {
                IsBusy = false;
                SyncCommand.ChangeCanExecute();
            }
        }

        public ICommand AddMoodCommand { get; }
        async Task ExecuteAddMoodCommand()
        {
            if (IsBusy)
                return;

            if(CommonConstants.FaceApiKey == "AC_FACE")
            {
                await Dialogs.AlertAsync(null, "Please set the Face API key from Azure Cognitive Services in CommonConstants.cs", AppResources.OK);
                return;
            }

            var result = "Error";
            MediaFile file = null;

            try
            {
                IsBusy = true;
                SyncCommand.ChangeCanExecute();
                await CrossMedia.Current.Initialize();

                if (CrossMedia.Current.IsTakePhotoSupported && DeviceInfo.DeviceType == DeviceType.Physical)
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
                Crashes.TrackError(ex);
                result = ex.Message;
            }
            finally
            {
                IsBusy = false;
                SyncCommand.ChangeCanExecute();
            }

            await Dialogs.AlertAsync(null, result, AppResources.OK);
        }
        public ICommand UpdateLocationCommand { get; }

        Xamarin.Essentials.Location location = null;
        readonly Placemark placemark = null;
        string mood = string.Empty;

        async Task ExecuteUpdateLocationCommand()
        {
            if (IsBusy)
                return;
            
            try
            {
                IsBusy = true;
                SyncCommand.ChangeCanExecute();

                UpdateMessage = AppResources.UpdatingLocation;

                location = await GeolocationService.GetCurrentPositionAsync();

                if (location == null)
                    throw new Exception("Unable to get location.");

                CurrentLocation = $"{location.Latitude}, {location.Longitude}";

                UpdateMessage = AppResources.UpdateLocationGeocoding;

                var address = await GeolocationService.GetAddressAsync(location);

                if (address != null)
                    CurrentLocation = $"{address.Locality}, {address.AdminArea ?? string.Empty} {address.CountryCode}";

                UpdateMessage = string.Empty;
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                Console.WriteLine("Unable to get location: " + ex);
                UpdateMessage = string.Empty;
                await Dialogs.AlertAsync(null, AppResources.UpdateLocationError, AppResources.OK);
            }
            finally
            {
                IsBusy = false;
                SyncCommand.ChangeCanExecute();
            }
        }


    }
}
