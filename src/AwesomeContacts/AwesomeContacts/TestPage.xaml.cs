using AwesomeContacts.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AwesomeContacts
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TestPage : ContentPage
	{
		public TestPage ()
		{
			InitializeComponent ();
            ButtonAllContacts.Clicked += (sender, args) => Navigation.PushAsync(new AllContactsPage());
            ButtonFaceAuth.Clicked += (sender, args) => Navigation.PushAsync(new FaceAuthPage());
            ButtonLogin.Clicked += (sender, args) => Navigation.PushAsync(new LoginPage());
            ButtonNearby.Clicked += (sender, args) => Navigation.PushAsync(new NearbyPage());
            ButtonUpdateLocation.Clicked += (sender, args) => Navigation.PushAsync(new UpdateLocationPage());
        }
	}
}