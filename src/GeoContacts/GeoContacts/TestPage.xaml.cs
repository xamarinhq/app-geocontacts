using GeoContacts.View;
using GeoContacts.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeoContacts
{
    public class TestViewModel : ViewModelBase
    {

    }
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TestPage : ContentPage
    {
        TestViewModel vm;
        public TestPage()
        {
            InitializeComponent();
            vm = new TestViewModel();
            BindingContext = vm;

            ButtonAllContacts.Clicked += (sender, args) => Navigation.PushAsync(new AllContactsPage());

            ButtonHomePage.Clicked += (sender, args) =>
            {
                if (Device.RuntimePlatform == Device.iOS)
                    Navigation.PushModalAsync(new HomePageiOS());
                else
                    Navigation.PushModalAsync(new NavigationPage(new HomePage()));
            };

            ButtonFaceAuth.Clicked += (sender, args) => Navigation.PushAsync(new FaceAuthPage());

            ButtonLogin.Clicked += async (sender, args) =>
            {
                if (!await vm.AuthenticationService.IsLoggedIn())
                    await Navigation.PushAsync(new LoginPage());
                else
                    await DisplayAlert("Already logged in", "You're already logged in!", "OK");
            };
            ButtonLogout.Clicked += (sender, args) => vm.AuthenticationService.Logout();

            ButtonNearby.Clicked += (sender, args) => Navigation.PushAsync(new NearbyPage());
            ButtonUpdateLocation.Clicked += (sender, args) => Navigation.PushAsync(new UpdateLocationPage());
            ButtonDetails.Clicked += (sender, args) => Navigation.PushAsync(new DetailsPage());
        }
    }
}