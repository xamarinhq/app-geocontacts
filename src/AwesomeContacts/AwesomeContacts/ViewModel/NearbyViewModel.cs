using System;
using System.Collections.Generic;
using System.Text;

namespace AwesomeContacts.ViewModel
{
    public class NearbyViewModel : ViewModelBase
    {
        public void GetNearby()
        {
            DataService.GetNearbyAsync();
        }
    }
}
