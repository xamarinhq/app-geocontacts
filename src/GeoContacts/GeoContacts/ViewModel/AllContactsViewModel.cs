using GeoContacts.Model;
using Microsoft.AppCenter.Crashes;
using MvvmHelpers;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace GeoContacts.ViewModel
{
    public class AllContactsViewModel : ViewModelBase
    {
        public ObservableRangeCollection<Contact> Contacts { get; }

        public ICommand RefreshCommand { get; }
        public ICommand ForceRefreshCommand { get; }

        public AllContactsViewModel()
        {
            Contacts = new ObservableRangeCollection<Contact>();
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
                var contacts = await DataService.GetAllAsync();

                if (contacts != null && contacts.Count() > 0)
                    Contacts.ReplaceRange(contacts);
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                System.Diagnostics.Debug.WriteLine($"*** ERROR: {ex.Message}");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
