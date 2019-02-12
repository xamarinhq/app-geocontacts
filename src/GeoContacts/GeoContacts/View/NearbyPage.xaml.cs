using GeoContacts.Model;
using GeoContacts.Resources;
using GeoContacts.ViewModel;

using Xamarin.Forms;

namespace GeoContacts.View
{
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

        async void MyListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (!(e.SelectedItem is Contact contact))
                return;

            await Navigation.PushAsync(new DetailsPage(contact));
        }
    }
}
