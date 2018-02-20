using AwesomeContacts.Model;
using AwesomeContacts.Resources;
using AwesomeContacts.Services;
using MvvmHelpers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Linq;
using Xamarin.Forms;

namespace AwesomeContacts.ViewModel
{
    public class NearbyViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Contact> Contacts { get; }

        public ICommand RefreshCommand { get; }
        public ICommand ForceRefreshCommand { get; }

        public NearbyViewModel()
        {
            Contacts = new ObservableRangeCollection<Contact>();
            RefreshCommand = new Command(async () => await ExecuteRefreshCommand(false));
            ForceRefreshCommand = new Command(async () => await ExecuteRefreshCommand(true));
        }

        async Task ExecuteRefreshCommand(bool forceRefresh)
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var position = await Geolocation.GetCurrentPositionAsync();

                if (position == null)
                    throw new Exception("Unable to get location.");

                Contacts.Clear();
                var contacts = await DataService.GetNearbyAsync(position.Longitude, position.Latitude);

                if (contacts != null && contacts.Count() > 0)
                    Contacts.ReplaceRange(contacts);
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
