using AwesomeContacts.Resources;
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
    public partial class NearbyPage : ContentPage
    {

        NearbyViewModel vm;

        public NearbyPage()
        {
            InitializeComponent();

            BindingContext = vm = new NearbyViewModel();
            if(vm.Settings.LoggedInMSFT)
            {
                // Allow MSFT to update location
                ToolbarItems.Add(new ToolbarItem
                {
                    Text = AppResources.ToolbarUpdateLocation,
                    Icon = "ic_location",
                    Command = new Command(async () => await Navigation.PushModalAsync(new NavigationPage(new UpdateLocationPage())))
                });
            }

          
        }
        
    }
}
