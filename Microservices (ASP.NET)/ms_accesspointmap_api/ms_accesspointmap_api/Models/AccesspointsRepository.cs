using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace ms_accesspointmap_api.Models
{
    public class AccesspointsRepository : IAccesspointsRepository, IDisposable
    {
        private AccessPointMapContext context;
        private static readonly HttpClient httpClient = new HttpClient();

        public AccesspointsRepository(AccessPointMapContext context)
        {
            this.context = context;
        }

        public async void CreateOrUpdate(Accesspoints accesspoint)
        {
            //Check if accesspoint with specific BSSID exists in the database, if not we create a new record but if the element is already stored we update the existing one
            if(context.Accesspoints.Any(element => element.Bssid == accesspoint.Bssid))
            {
                //Assign the existing accesspoint to a variable
                Accesspoints existingAccesspoint = context.Accesspoints.FirstOrDefault(element => element.Bssid == accesspoint.Bssid);

                //Check if the lowSignal strength is weaker, if yes update data
                if(accesspoint.LowSignalLevel < existingAccesspoint.LowSignalLevel)
                {
                    existingAccesspoint.LowLatitude = accesspoint.LowLatitude;
                    existingAccesspoint.LowLongitude = accesspoint.LowLongitude;
                    existingAccesspoint.LowSignalLevel = accesspoint.LowSignalLevel;
                }

                //Check if the highSignal strength is stronger, if yes update data
                if(accesspoint.HighSignalLevel > existingAccesspoint.HighSignalLevel)
                {
                    existingAccesspoint.HighLatitude = accesspoint.HighLatitude;
                    existingAccesspoint.HighLongitude = accesspoint.HighLongitude;
                    existingAccesspoint.HighSignalLevel = accesspoint.HighSignalLevel;
                }

                //Calculate the signal radius and the signal area
                double latitudeDistance = (existingAccesspoint.LowLatitude - existingAccesspoint.HighLatitude) * Math.PI / 180.0;
                double longitudeDistance = (existingAccesspoint.LowLongitude - existingAccesspoint.HighLongitude) * Math.PI / 180.0;

                double lowLatitudeRadian = existingAccesspoint.LowLatitude * Math.PI / 180.0;
                double highLatitudeRadian = existingAccesspoint.HighLatitude * Math.PI / 180.0;

                double a = Math.Pow(Math.Sin(latitudeDistance / 2.0), 2.0) +
                           Math.Pow(Math.Sin(longitudeDistance / 2.0), 2.0) *
                           Math.Cos(lowLatitudeRadian) * Math.Cos(highLatitudeRadian);

                double c = 2.0 * Math.Asin(Math.Sqrt(a));

                existingAccesspoint.SignalRadius = Math.Round((6371.0 * c) * 1000.0, 2);
                existingAccesspoint.SignalArea = Math.Round(Math.PI * Math.Pow(existingAccesspoint.SignalRadius, 2), 2);

                //Update the object in database
                context.Entry(existingAccesspoint).State = EntityState.Modified;
            }
            else
            {
                //Calculate the signal radius and the signal area
                double latitudeDistance = (accesspoint.LowLatitude - accesspoint.HighLatitude) * Math.PI / 180.0;
                double longitudeDistance = (accesspoint.LowLongitude - accesspoint.HighLongitude) * Math.PI / 180.0;

                double lowLatitudeRadian = accesspoint.LowLatitude * Math.PI / 180.0;
                double highLatitudeRadian = accesspoint.HighLatitude * Math.PI / 180.0;

                double a = Math.Pow(Math.Sin(latitudeDistance / 2.0), 2.0) +
                           Math.Pow(Math.Sin(longitudeDistance / 2.0), 2.0) *
                           Math.Cos(lowLatitudeRadian) * Math.Cos(highLatitudeRadian);

                double c = 2.0 * Math.Asin(Math.Sqrt(a));

                accesspoint.SignalRadius = Math.Round((6371.0 * c) * 1000.0, 2);
                accesspoint.SignalArea = Math.Round(Math.PI * Math.Pow(accesspoint.SignalRadius, 2), 2);

                //Call the https://macvendors.com API to get the brand name bassed on the MAC address
                try
                {
                    string response = await httpClient.GetStringAsync("https://api.macvendors.com/" + HttpUtility.UrlEncode(accesspoint.Bssid));
                    if (response.Contains("error", StringComparison.OrdinalIgnoreCase))
                    {
                        accesspoint.Brand = "No brand info";
                    }
                    else
                    {
                        accesspoint.Brand = response;
                    }
                }
                catch (HttpRequestException e)
                {
                    accesspoint.Brand = "No brand info";
                }

                //Set the device type, currently no practical use for this variable, but i keep it for the future
                accesspoint.DeviceType = "default";

                //Add the object to the database
                context.Accesspoints.Add(accesspoint);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Accesspoints GetAccesspointById(int id)
        {
            return context.Accesspoints.FirstOrDefault(accesspoint => accesspoint.Id == id);
        }

        public IEnumerable<Accesspoints> GetAccesspoints()
        {
            //Should those methods by async?
            return context.Accesspoints.ToList();
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public IEnumerable<Accesspoints> SearchAccesspoints(string ssid="", int freq=0, string brand="", string security="")
        {
            throw new NotImplementedException();
        }
    }
}
