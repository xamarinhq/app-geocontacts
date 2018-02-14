using System;

namespace AwesomeContacts.SharedModels
{
    public class LocationUpdate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        public string Country { get; set; }
        public string Town { get; set; }
        public string State { get; set; }
    }
}
