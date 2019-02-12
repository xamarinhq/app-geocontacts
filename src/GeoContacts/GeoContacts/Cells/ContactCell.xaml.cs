
using Xamarin.Forms;

namespace GeoContacts.Cells
{
    public class ContactCell : ViewCell
    {
        public ContactCell()
        {
            View = new ContactCellView();
        }
    }
	public partial class ContactCellView : ContentView
	{
		public ContactCellView()
		{
			InitializeComponent ();
		}
	}
}