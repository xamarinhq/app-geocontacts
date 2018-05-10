using System.Threading.Tasks;
using GeoContacts.Helpers;
using GeoContacts.Resources;
using GeoContacts.Services;
using MvvmHelpers;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace GeoContacts.ViewModel
{
    public class ViewModelBase : BaseViewModel
    {
        public Settings Settings => Settings.Current;

        IDataService dataService;
        public IDataService DataService => dataService ?? (dataService = DependencyService.Get<IDataService>());
        IDialogs dialogs;
        public IDialogs Dialogs => dialogs ?? (dialogs = DependencyService.Get<IDialogs>());
        IAuthenticationService authenticationService;
        public IAuthenticationService AuthenticationService => authenticationService ??
            (authenticationService = DependencyService.Get<IAuthenticationService>());

        string updateMessage;
        public string UpdateMessage
        {
            get => updateMessage;
            set => SetProperty(ref updateMessage, value);
        }

        public async Task<bool> CheckConnectivityAsync()
        {

            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Dialogs.AlertAsync(null, AppResources.NoInternet, AppResources.OK);
                return false;
            }

            return true;
        }

        public static Task ExecuteGoToSiteExtCommand(string site) =>
            Browser.OpenAsync(site, BrowserLaunchType.SystemPreferred);
        
    }
}
