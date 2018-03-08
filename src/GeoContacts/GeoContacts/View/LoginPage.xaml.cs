using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using GeoContacts.ViewModel;
using GeoContacts.Helpers;

namespace GeoContacts.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        LoginViewModel vm;
        public LoginPage()
        {
            InitializeComponent();

            BindingContext = vm = new LoginViewModel();

            if(CommonConstants.ShowLogin != "AC_SHOWLOGIN")
            {
                ButtonLogin.IsVisible = false;
            }
        }
    }
}