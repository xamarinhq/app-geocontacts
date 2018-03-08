// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using GeoContacts.SharedModels;
//
//    var data = GraphInfo.FromJson(jsonString);

namespace GeoContacts.SharedModels
{
    using System;
    using System.Net;
    using System.Collections.Generic;

    using Newtonsoft.Json;

    public partial class GraphInfo
    {
        [JsonProperty("@odata.context")]
        public string OdataContext { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("businessPhones")]
        public List<string> BusinessPhones { get; set; }

        [JsonProperty("displayName")]
        public string DisplayName { get; set; }

        [JsonProperty("givenName")]
        public string GivenName { get; set; }

        [JsonProperty("jobTitle")]
        public string JobTitle { get; set; }

        [JsonProperty("mail")]
        public string Mail { get; set; }

        [JsonProperty("mobilePhone")]
        public object MobilePhone { get; set; }

        [JsonProperty("officeLocation")]
        public string OfficeLocation { get; set; }

        [JsonProperty("preferredLanguage")]
        public object PreferredLanguage { get; set; }

        [JsonProperty("surname")]
        public string Surname { get; set; }

        [JsonProperty("userPrincipalName")]
        public string UserPrincipalName { get; set; }
    }

    public partial class GraphInfo
    {
        public static GraphInfo FromJson(string json) => JsonConvert.DeserializeObject<GraphInfo>(json, GeoContacts.SharedModels.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this GraphInfo self) => JsonConvert.SerializeObject(self, GeoContacts.SharedModels.Converter.Settings);
    }

    public class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
        };
    }
}

