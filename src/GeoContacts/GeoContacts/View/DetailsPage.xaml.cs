using GeoContacts.Model;
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