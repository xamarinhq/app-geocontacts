using GeoContacts.ViewModel;

using Xamarin.Forms;

namespace GeoContacts.View
{
    public partial class UpdateLocationPage : ContentPage
	{
        UpdateLocationViewModel vm;
		public UpdateLocationPage ()
		{
			InitializeComponent ();
            BindingContext = vm = new UpdateLocationViewModel();

           
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