using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GeoContacts.Cells
{
    public class ContactCell : ViewCell
    {
        public ContactCell()
        {
            View = new ContactCellView();
        }
    }

    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ContactCellView : ContentView
	{
		public ContactCellView()
		{
			InitializeComponent ();
		}
	}
}