
using Xamarin.Forms;
using GeoContacts.ViewModel;
using GeoContacts.Helpers;

namespace GeoContacts.View
{
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