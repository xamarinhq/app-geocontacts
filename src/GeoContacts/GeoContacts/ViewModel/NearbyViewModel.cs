using GeoContacts.Model;
using GeoContacts.Resources;
using GeoContacts.Services;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using Xamarin.Forms;
using Microsoft.AppCenter.Analytics;

namespace GeoContacts.ViewModel
{
    public class NearbyViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Grouping<string, Contact>> ContactsGrouped { get; }

        public ICommand RefreshCommand { get; }
        public ICommand ForceRefreshCommand { get; }

        public NearbyViewModel()
        {
            ContactsGrouped = new ObservableRangeCollection<Grouping<string, Contact>>();
            RefreshCommand = new Command(async () => await ExecuteRefreshCommand(false));
            ForceRefreshCommand = new Command(async () => await ExecuteRefreshCommand(true));
        }

        async Task ExecuteRefreshCommand(bool forceRefresh)
        {
            if (IsBusy)
                return;

            if (!await CheckConnectivityAsync())
                return;

            IsBusy = true;

            try
            {
                Analytics.TrackEvent("SerchedForNearby");
                var position = await Geolocation.GetCurrentPositionAsync();

                if (position == null)
                    throw new Exception("Unable to get location.");

                ContactsGrouped.Clear();
                var contacts = await DataService.GetNearbyAsync(position.Longitude, position.Latitude);
                if (contacts.Count() > 0)
                    ContactsGrouped.AddRange(contacts);
                else
                    await Dialogs.AlertAsync(null, AppResources.NoCDAsNearby, AppResources.OK);


            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"*** ERROR: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
