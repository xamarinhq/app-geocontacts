//using Microsoft.Azure.Documents.Spatial;
using Microsoft.Azure.Documents.Spatial;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

//namespace AwesomeContacts.Model
//{
//public class Contact
//{
//    public string Id { get; set; }
//    public string FirstName { get; set; }
//    public string LastName { get; set; }
//    public string FocusSkill { get; set; }
//    public string HomeTown { get; set; }
//    public string CurrentTown { get; set; }
//    public Position CurrentPosition { get; set; }
//    public string Biography { get; set; }


//    public string PhotoUrl { get; set; }

//    #region Social
//    public string TwitterHandle { get; set; }
//    public string LinkedIn { get; set; }
//    public string GitHub { get; set; }
//    public string StackOverflow { get; set; }
//    public string Blog { get; set; }
//    #endregion

//    [JsonIgnore]
//    public string FullName => $"{FirstName} {LastName}";
//}

// To parse this JSON data, add NuGet 'Newtonsoft.Json' then do:
//
//    using AwesomeContacts.Model;
//
//    var contact = Contact.FromJson(jsonString);

namespace AwesomeContacts.Model
{
    using System;
    using System.Net;
    using System.Collections.Generic;

    using Newtonsoft.Json;

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
        public object Facebook { get; set; }

        [JsonProperty("Instagram")]
        public object Instagram { get; set; }

        [JsonProperty("Twitch")]
        public object Twitch { get; set; }

        [JsonProperty("Podcast")]
        public object Podcast { get; set; }

        [JsonProperty("Location")]
        public Location Location { get; set; }

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

        [JsonIgnore]
        public string PhotoUrl { get; set; }

        [JsonIgnore]
        public string TwitterHandle { get; set; }
    }

    public partial class Location
    {
        [JsonProperty("Display")]
        public string Display { get; set; }

        [JsonProperty("Position")]
        public Position Position { get; set; }
    }

    public partial class Position
    {
        [JsonProperty("Type")]
        public string Type { get; set; }

        [JsonProperty("Coordinates")]
        public List<double> Coordinates { get; set; }
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
        public static Contact FromJson(string json) => JsonConvert.DeserializeObject<Contact>(json, AwesomeContacts.Model.Converter.Settings);
    }

    public static class Serialize
    {
        public static string ToJson(this Contact self) => JsonConvert.SerializeObject(self, AwesomeContacts.Model.Converter.Settings);
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
