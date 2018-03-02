using GeoContacts.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeoContacts.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UpdateLocationPage : ContentPage
	{
        UpdateLocationViewModel vm;
		public UpdateLocationPage ()
		{
			InitializeComponent ();
            BindingContext = vm = new UpdateLocationViewModel();

            if (Device.RuntimePlatform == Device.iOS)
            {
                ToolbarItems.Add(new ToolbarItem
                {
                    Text = "Done",
                    Command = new Command(async (obj) =>
                    {
                        if (vm.IsBusy)
                            return;

                        await Navigation.PopModalAsync();
                    })
                });
            }
		}
	}
}