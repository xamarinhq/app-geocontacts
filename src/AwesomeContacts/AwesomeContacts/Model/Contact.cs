using Microsoft.Azure.Documents.Spatial;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AwesomeContacts.Model
{
    public class Contact
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FocusSkill { get; set; }
        public string HomeTown { get; set; }
        public string CurrentTown { get; set; }
        public Position CurrentPosition { get; set; }
        public string Biography { get; set; }


        public string PhotoUrl { get; set; }

        #region Social
        public string TwitterHandle { get; set; }
        public string LinkedIn { get; set; }
        public string GitHub { get; set; }
        public string StackOverflow { get; set; }
        public string Blog { get; set; }
        #endregion


        [JsonIgnore]
        public string FullName => $"{FirstName} {LastName}";
    }
}
