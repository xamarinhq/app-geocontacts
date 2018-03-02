using Microsoft.Azure.Documents.Spatial;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace GeoContacts.Model
{

    public partial class Contact
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Remarks")]
        public string Remarks { get; set; }

        [JsonProperty("MetaData")]
        public MetaData MetaData { get; set; }

        [JsonProperty("Tagline")]
        public string Tagline { get; set; }

        [JsonProperty("Image")]
        public Dictionary<string, string> Image { get; set; }

        [JsonProperty("Twitter")]
        public string Twitter { get; set; }

        [JsonProperty("GitHub")]
        public string GitHub { get; set; }

        [JsonProperty("Blog")]
        public string Blog { get; set; }

        [JsonProperty("StackOverflow")]
        public string StackOverflow { get; set; }

        [JsonProperty("LinkedIn")]
        public string LinkedIn { get; set; }

        [JsonProperty("Facebook")]
        public string Facebook { get; set; }

        [JsonProperty("Instagram")]
        public string Instagram { get; set; }

        [JsonProperty("Twitch")]
        public string Twitch { get; set; }

        [JsonProperty("Podcast")]
        public string Podcast { get; set; }

        [JsonProperty("Location")]
        public Location Hometown { get; set; }

        [JsonProperty("UserPrincipalName")]
        public string UserPrincipalName { get; set; }

        [JsonProperty("id")]
        public string ContactId { get; set; }

        [JsonProperty("_rid")]
        public string Rid { get; set; }

        [JsonProperty("_self")]
        public string Self { get; set; }

        [JsonProperty("_etag")]
        public string Etag { get; set; }

        [JsonProperty("_attachments")]
        public string Attachments { get; set; }

        [JsonProperty("_ts")]
        public long Ts { get; set; }

        public string PhotoUrl { get; set; }

        public string TwitterHandle { get; set; }

        [JsonIgnore]
        public Point CurrentLocation { get; set; }

        [JsonIgnore]
        public string MiniTagline
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Tagline))
                    return string.Empty;

                var index = Tagline.IndexOf('/');
                if (index <= 0)
                    return Tagline;

                return Tagline.Substring(0, index).Trim();
            }
        }
    }

    public partial class Location
    {
        [JsonProperty("Display")]
        public string Display { get; set; }

        [JsonProperty("Position")]
        public Point Position { get; set; }

        [JsonIgnore]
        public string City
        {
            get
            {
                if (string.IsNullOrWhiteSpace(Display))
                    return string.Empty;

                var index = Display.IndexOf(',');
                if (index <= 0)
                    return string.Empty;

                return Display.Substring(0, index).Trim();
            }
        }
    }

    public partial class MetaData
    {
        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }
    }

    public partial class Contact
    {
        public static Contact FromJson(string json) => JsonConvert.DeserializeObject<Contact>(json, GeoContacts.Model.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Contact self) => JsonConvert.SerializeObject(self, GeoContacts.Model.Converter.Settings);
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

//}
