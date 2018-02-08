using AwesomeContacts.View;
using AwesomeContacts.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AwesomeContacts
{
    public class TestViewModel : ViewModelBase
    {

    }
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TestPage : ContentPage
	{
		public TestPage ()
		{
			InitializeComponent ();
            BindingContext = new TestViewModel();

            ButtonAllContacts.Clicked += (sender, args) => Navigation.PushAsync(new AllContactsPage());

            ButtonHomePage.Clicked += (sender, args) =>
            {
                if (Device.RuntimePlatform == Device.iOS)
                    Navigation.PushModalAsync(new HomePageiOS());
                else
                    Navigation.PushModalAsync(new NavigationPage(new HomePage()));
            };

            ButtonFaceAuth.Clicked += (sender, args) => Navigation.PushAsync(new FaceAuthPage());
            ButtonLogin.Clicked += (sender, args) => Navigation.PushAsync(new LoginPage());
            ButtonNearby.Clicked += (sender, args) => Navigation.PushAsync(new NearbyPage());
            ButtonUpdateLocation.Clicked += (sender, args) => Navigation.PushAsync(new UpdateLocationPage());
            ButtonDetails.Clicked += (sender, args) => Navigation.PushAsync(new DetailsPage());
        }
	}
}