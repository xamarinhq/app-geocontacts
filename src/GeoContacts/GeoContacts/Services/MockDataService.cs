using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GeoContacts.Model;
using GeoContacts.Helpers;
using MvvmHelpers;
using GeoContacts.Resources;
using Xamarin.Essentials;
using Microsoft.AppCenter.Crashes;

namespace GeoContacts.Services
{
    public class MockDataService : IDataService
    {
        List<Contact> contacts;

        List<Grouping<string, Contact>> contactsGrouped;
        public MockDataService()
        {
            contacts = new List<Contact>
            {
                new Contact
                {
                    Name = "Matthew Soucoup",
                    Remarks = "Matthew Soucoup is a Senior Cloud Developer Advocate at Microsoft spreading the love of integrating Azure with Xamarin. Matt is also a Pluralsight author, a Telerik Developer Expert and prior to joining Microsoft a founder of a successful consulting firm targeting .NET and web development. Matt loves sharing his passion and insight for mobile and cloud development by blogging, writing articles, and presenting at conferences such as Xamarin Evolve, CodeMash, VS Live, and Mobile Era. When not behind a computer screen, Matt gardens hot peppers, rides bikes, and loves Wisconsin micro-brews and cheese. Follow Matt on Twitter at @codemillmatt and his personal blog at codemilltech.com.",
                    Blog = "http://codemilltech.com",
                    Hometown = new Model.Location
                    {
                        Display="Madison, WI"
                    },
                    Tagline = "Xamarin/.NET",
                    GitHub = "http://github.com/codemillmatt",
                    Twitter = "http://twitter.com/codemillmat",
                    TwitterHandle = "codemillmatt",
                    Id = "0",
                    LinkedIn = "http://linkedin.com/in/msoucoup",
                    PhotoUrl = "https://developer.microsoft.com/en-us/advocates/media/profiles/matthew-soucoup.png",
                }
            };

            contactsGrouped.Add(new Grouping<string, Contact>(AppResources.Hometown, contacts));
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

        public Task<IEnumerable<Grouping<string, Contact>>> GetNearbyAsync(double userLongitude, double userLatitude)
        {
            return Task.FromResult(contactsGrouped as IEnumerable<Grouping<string, Contact>>);
        }

        public async Task UpdateLocationAsync(Xamarin.Essentials.Location position, Placemark address, string mood, string accessToken)
        {
            try
            {
                var client = new System.Net.Http.HttpClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

                var location = new SharedModels.LocationUpdate
                {
                    Country = address.CountryCode,
                    Position = new Microsoft.Azure.Documents.Spatial.Point(position.Longitude, position.Latitude),
                    State = address.AdminArea,
                    Town = address.Locality,
                    Mood = mood
                };

                var json = Newtonsoft.Json.JsonConvert.SerializeObject(location);
                var content = new System.Net.Http.StringContent(json);
                var resp = await client.PostAsync(CommonConstants.FunctionUrl, content);

                var respBody = await resp.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Crashes.TrackError(ex);
                System.Diagnostics.Debug.WriteLine($"ERROR: {ex.Message}");
            }
        }
    }
}
