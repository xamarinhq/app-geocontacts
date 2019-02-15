
using Xamarin.Forms;

namespace GeoContacts.Cells
{
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