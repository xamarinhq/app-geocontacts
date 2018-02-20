using System;
using System.Collections.Generic;
using System.Text;

namespace AwesomeContacts.ViewModel
{
    public class NearbyViewModel : ViewModelBase
    {
        public void GetNearby()
        {
            double seattleLong = -122.3321;
            double seattleLat = 47.6062;

            DataService.GetNearbyAsync(seattleLong, seattleLat);
        }
    }
}
