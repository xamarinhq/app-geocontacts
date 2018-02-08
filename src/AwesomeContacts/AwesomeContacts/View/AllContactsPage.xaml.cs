using AwesomeContacts.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AwesomeContacts.View
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
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            if (vm.Contacts.Count == 0)
                vm.RefreshCommand.Execute(null);
        }
    }
}
