using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents.Spatial;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AwesomeContacts.SharedModels
{
    public class LocationUpdate
    {
        [JsonProperty("position")]
        public Point Position { get; set; }

        public string Country { get; set; }
        public string Town { get; set; }
        public string State { get; set; }
        public string UserPrincipalName { get; set; }
        public DateTimeOffset InsertTime { get; set; }
    }
}
