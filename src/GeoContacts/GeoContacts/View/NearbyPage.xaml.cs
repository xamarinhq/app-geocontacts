using GeoContacts.Model;
using GeoContacts.Resources;
using GeoContacts.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeoContacts.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NearbyPage : ContentPage
    {

        NearbyViewModel vm;

        public NearbyPage()
        {
            InitializeComponent();

            BindingContext = vm = new NearbyViewModel();
            if (vm.Settings.LoggedInMSFT)
            {
                // Allow MSFT to update location
                ToolbarItems.Add(new ToolbarItem
                {
                    Text = AppResources.ToolbarUpdateLocation,
                    Icon = "ic_location",
                    Command = new Command(async () => await Navigation.PushModalAsync(new NavigationPage(new UpdateLocationPage())))
                });
            }

            MyListView.ItemTapped += (sender, args) => MyListView.SelectedItem = null;
            MyListView.ItemSelected += MyListView_ItemSelected;
        }

        private async void MyListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var contact = e.SelectedItem as Contact;
            if (contact == null)
                return;

            await Navigation.PushAsync(new DetailsPage(contact));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
        }

    }
}
