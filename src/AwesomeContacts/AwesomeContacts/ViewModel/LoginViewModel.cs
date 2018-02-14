using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace AwesomeContacts.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new Command(async () => await AuthenticationService.Login());
        }
    }
}
