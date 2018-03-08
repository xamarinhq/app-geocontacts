using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using GeoContacts.Helpers;
using GeoContacts.Resources;
using GeoContacts.Services;
using MvvmHelpers;
using Plugin.Connectivity;
using Plugin.Share;
using Xamarin.Forms;

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

            if (!CrossConnectivity.Current.IsConnected)
            {
                await Dialogs.AlertAsync(null, AppResources.NoInternet, AppResources.OK);
                return false;
            }

            return true;
        }

        public static void ExecuteGoToSiteExtCommand(string site)
        {
            var color = Color.FromHex("#b2169c");
            CrossShare.Current.OpenBrowser(site, new Plugin.Share.Abstractions.BrowserOptions
            {
                ChromeShowTitle = true,
                ChromeToolbarColor = new Plugin.Share.Abstractions.ShareColor
                {
                    A = 255,
                    R = (int)(color.R * 255),
                    G = (int)(color.G * 255),
                    B = (int)(color.B * 255)
                },
                SafariBarTintColor = new Plugin.Share.Abstractions.ShareColor
                {
                    A = 255,
                    R = (int)(color.R * 255),
                    G = (int)(color.G * 255),
                    B = (int)(color.B * 255)
                },
                SafariControlTintColor = new Plugin.Share.Abstractions.ShareColor
                {
                    A = 255,
                    R = 255,
                    G = 255,
                    B = 255
                },
                UseSafariReaderMode = false,
                UseSafariWebViewController = true
            });

        }
    }
}
