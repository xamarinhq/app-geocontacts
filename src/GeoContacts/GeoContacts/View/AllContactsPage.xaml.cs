using GeoContacts.Model;
using GeoContacts.ViewModel;

using Xamarin.Forms;

namespace GeoContacts.View
{
    public partial class AllContactsPage : ContentPage
    {
        AllContactsViewModel vm;

        public AllContactsPage()
        {
            InitializeComponent();
            BindingContext = vm = new AllContactsViewModel();
            MyListView.ItemTapped += (sender, args) => MyListView.SelectedItem = null;
            MyListView.ItemSelected += MyListView_ItemSelected;
        }

        async void MyListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (!(e.SelectedItem is Contact contact))
                return;

            await Navigation.PushAsync(new DetailsPage(contact));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (vm.Contacts.Count == 0)
                vm.RefreshCommand.Execute(null);
        }
    }
}
