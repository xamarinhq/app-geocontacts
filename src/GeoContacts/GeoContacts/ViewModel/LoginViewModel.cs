using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Microsoft.Identity.Client;
using System.Threading.Tasks;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;

namespace GeoContacts.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        public ICommand LoginCommand { get; }
        public ICommand GuestCommand { get; }

        AuthenticationResult authenticationResult;

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await ExecuteLoginCommand());
            GuestCommand = new Command(() =>
            {
                Settings.InGuestMode = true;
                Settings.LoggedInMSFT = false;
                App.GoHome();
            });
        }

        async Task ExecuteLoginCommand()
        {
            if (IsBusy)
                return;

            if (!await CheckConnectivityAsync())
                return;

            try
            {
                IsBusy = true;
                Analytics.TrackEvent("CDA-Login");

                // Force a logout first - just to be sure the silent login fails during this portion
                AuthenticationService.Logout();

                authenticationResult = await AuthenticationService.Login();


                var displayName = authenticationResult?.User?.Name;
                if (string.IsNullOrWhiteSpace(displayName))
                {
                    //TODO: Unable to login
                }
                else
                {
                    Settings.InGuestMode = false;
                    Settings.LoggedInMSFT = true;
                    App.GoHome();
                }
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
