using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AwesomeContacts.SharedModels
{
    public class LocationUpdate
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }

        [JsonProperty("position")]
        public Position Position { get; set; }

        public string Country { get; set; }
        public string Town { get; set; }
        public string State { get; set; }
        public string UserPrincipalName { get; set; }
        public DateTimeOffset InsertTime { get; set; }
    }

    public class Position
    {
        public string Type { get; set; }
        public decimal[] Coordinates { get; set; }
    }

    public class LocationUpdateCompare : IEqualityComparer<LocationUpdate>
    {
        public bool Equals(LocationUpdate locationUpdate1, LocationUpdate locationUpdate2)
        {
            return locationUpdate1.UserPrincipalName.Equals(locationUpdate2.UserPrincipalName,
                                                            StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(LocationUpdate obj)
        {
            return obj.UserPrincipalName.GetHashCode();
        }
    }
}
