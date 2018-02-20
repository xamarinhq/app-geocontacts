using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AwesomeContacts.Model;
using Plugin.Geolocator.Abstractions;
using AwesomeContacts.Helpers;

namespace AwesomeContacts.Services
{
    public class MockDataService : IDataService
    {
        List<Contact> contacts;
        public MockDataService()
        {
            //contacts = new List<Contact>
            //{
            //    new Contact
            //    {
            //        FirstName = "Matthew",
            //        LastName = "Soucoup",
            //        Biography = "Matthew Soucoup is a Senior Cloud Developer Advocate at Microsoft spreading the love of integrating Azure with Xamarin. Matt is also a Pluralsight author, a Telerik Developer Expert and prior to joining Microsoft a founder of a successful consulting firm targeting .NET and web development. Matt loves sharing his passion and insight for mobile and cloud development by blogging, writing articles, and presenting at conferences such as Xamarin Evolve, CodeMash, VS Live, and Mobile Era. When not behind a computer screen, Matt gardens hot peppers, rides bikes, and loves Wisconsin micro-brews and cheese. Follow Matt on Twitter at @codemillmatt and his personal blog at codemilltech.com.",
            //        Blog = "http://codemilltech.com",
            //        CurrentTown = "Redmond,WA",
            //        FocusSkill = "Xamarin/.NET",
            //        GitHub = "codemillmatt",
            //        TwitterHandle = "codemillmatt",
            //        HomeTown = "Madison, WI USA",
            //        Id = "0",
            //        LinkedIn = "msoucoup",
            //        PhotoUrl = "https://developer.microsoft.com/en-us/advocates/media/profiles/matthew-soucoup.png",
            //        StackOverflow = "6621061"
            //    }
            //};

            contacts = new List<Contact>();

        }

        public Task Initialize()
        {
            return Task.CompletedTask;
        }

        public Task<IEnumerable<Contact>> GetAllAsync()
        {
            return Task.FromResult(contacts as IEnumerable<Contact>);
        }

        public Task<Contact> GetAsync(string id)
        {
            return Task.FromResult(contacts.ElementAt(0));
        }

        public Task<IEnumerable<Contact>> GetNearbyAsync(double userLongitude, double userLatitude)
        {
            return Task.FromResult(contacts as IEnumerable<Contact>);
        }

        public async Task UpdateLocationAsync(Plugin.Geolocator.Abstractions.Position position, Address address, string accessToken)
        {
            try
            {
                var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                var location = new AwesomeContacts.SharedModels.LocationUpdate
                {
                    Country = address.CountryCode,
                    Position = new Microsoft.Azure.Documents.Spatial.Point(position.Longitude, position.Latitude),
                    State = address.AdminArea,
                    Town = address.Locality
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(location);
                var content = new System.Net.Http.StringContent(json);
                var resp = await client.PostAsync(CommonConstants.FunctionUrl, content);

                var respBody = await resp.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"ERROR: {ex.Message}");
            }
        }
    }
}
