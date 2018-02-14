using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Microsoft.Identity.Client;

namespace AwesomeContacts.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        public ICommand LoginCommand { get; }

        AuthenticationResult authenticationResult;

        public LoginViewModel()
        {
            LoginCommand = new Command(async () =>
            {
                if (IsBusy)
                    return;

                if (!await CheckConnectivityAsync())
                    return;

                try
                {
                    IsBusy = true;

                    authenticationResult = await AuthenticationService.Login();


                    // TODO: Obvs we'll want to get rid of this in the real app
                    var displayName = authenticationResult?.User?.Name ?? "COULDN'T LOG IN";

                    await Application.Current.MainPage.DisplayAlert("Result", $"Hey {displayName}, glad to see you!", "ok");
                }
                finally
                {
                    IsBusy = false;
                }
            });
        }
    }
}
