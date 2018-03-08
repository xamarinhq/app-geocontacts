using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeoContacts.Cells
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ContactsGroupHeaderView : ContentView
	{
		public ContactsGroupHeaderView()
		{
			InitializeComponent ();
		}
	}

    public class ContactsGroupHeader : ViewCell
    {
        public ContactsGroupHeader()
        {
            View = new ContactsGroupHeaderView();
        }
    }
}