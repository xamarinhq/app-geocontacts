using System.Net;
using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using AwesomeContacts.SharedModels;
using BingMapsRESTToolkit;
using System;
using System.Linq;

namespace AwesomeContacts.Functions
{
    public static class UpdateGeolocation
    {
        [FunctionName("UpdateGeolocation")]
        public static async Task<object> Run([HttpTrigger(WebHookType = "genericJson")]HttpRequestMessage req, TraceWriter log)
        {
            log.Info($"Webhook was triggered!");

            

            string jsonContent = await req.Content.ReadAsStringAsync();
            LocationUpdate data = null;

            if (!string.IsNullOrWhiteSpace(jsonContent))
                data = JsonConvert.DeserializeObject<LocationUpdate>(jsonContent);
#if DEBUG
            else
                data = new LocationUpdate
                {
                    Latitude = 47.6451599,
                    Longitude = -122.130602,
                };
#endif


            var key = System.Environment.GetEnvironmentVariable("BingMapsKey", EnvironmentVariableTarget.Process);

            //if we can't detect the city in app, let's figure it out!
            if(string.IsNullOrWhiteSpace(data.Country) || string.IsNullOrWhiteSpace(data.State) || string.IsNullOrWhiteSpace(data.Town))
            {
                var  reverse = await ServiceManager.GetResponseAsync(new ReverseGeocodeRequest()
                {
                    BingMapsKey = key,
                    Point = new Coordinate(data.Latitude, data.Longitude),
                    IncludeEntityTypes = new System.Collections.Generic.List<EntityType>
                    {
                        EntityType.CountryRegion,
                        EntityType.Address,
                        EntityType.AdminDivision1
                    }
                });

                var result1 = reverse.ResourceSets.FirstOrDefault()?.Resources.FirstOrDefault() as Location;
                if(result1 != null)
                {
                    //update the city/state here
                    data.State = result1.Address.AdminDistrict;
                    data.Country = result1.Address.CountryRegion;
                    data.Town = result1.Address.Locality;
                }
            }

            //find the generic lat/long
            var r = await ServiceManager.GetResponseAsync(new GeocodeRequest()
            {
                BingMapsKey = key,
                Query = $"{data.Town}, {data.State} {data.Country}"
            });


            var result2 = r.ResourceSets.FirstOrDefault()?.Resources.FirstOrDefault() as Location;

            var point = result2?.GeocodePoints?.FirstOrDefault()?.GetCoordinate();
            if (point != null)
            {
                data.Latitude = point.Latitude;
                data.Longitude = point.Longitude;

            }
            else
            {
                //blank out lat/long
                data.Latitude = 47.6451600;
                data.Longitude = -122.1306030;
            }

            //update cosmos DB here


            return req.CreateResponse(HttpStatusCode.OK, new
            {
                greeting = $"Hello {data.Country} {data.Town}!"
            });
        }
    }
}
