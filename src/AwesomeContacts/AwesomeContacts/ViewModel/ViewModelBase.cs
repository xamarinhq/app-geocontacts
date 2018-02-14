using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AwesomeContacts.Helpers;
using AwesomeContacts.Resources;
using AwesomeContacts.Services;
using MvvmHelpers;
using Plugin.Connectivity;
using Xamarin.Forms;

namespace AwesomeContacts.ViewModel
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
    }
}
