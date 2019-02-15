using GeoContacts.View;
using GeoContacts.ViewModel;

using Xamarin.Forms;

namespace GeoContacts
{
    public class TestViewModel : ViewModelBase
    {

    }

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
                Navigation.PushModalAsync(new HomePage());
            };
            
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