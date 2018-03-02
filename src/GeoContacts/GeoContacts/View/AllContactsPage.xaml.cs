using GeoContacts.Model;
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
            if (vm.Contacts.Count == 0)
                vm.RefreshCommand.Execute(null);
        }
    }
}
