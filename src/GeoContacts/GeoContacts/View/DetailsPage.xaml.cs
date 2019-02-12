using GeoContacts.Model;
using GeoContacts.ViewModel;

using Xamarin.Forms;

namespace GeoContacts.View
{
    public partial class DetailsPage : ContentPage
	{
		public DetailsPage ()
		{
			InitializeComponent ();
		}

        DetailsViewModel vm;
        public DetailsPage(Contact contact)
        {
            InitializeComponent();
            BindingContext = vm = new DetailsViewModel(contact);
        }
    }
}