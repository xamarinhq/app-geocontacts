using GeoContacts.Resources;
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
    public partial class HomePage : TabbedPage
    {
        public HomePage ()
        {
            InitializeComponent();
            Title = AppResources.TitleAllContacts;
            this.CurrentPageChanged += HomePage_CurrentPageChanged;
        }

        private void HomePage_CurrentPageChanged(object sender, EventArgs e)
        {
            switch(CurrentPage)
            {
                case AllContactsPage a:
                    Title = AppResources.TitleAllContacts;
                    break;
                case NearbyPage b:
                    Title = AppResources.TitleNearby;
                    break;

            }
        }
    }
}