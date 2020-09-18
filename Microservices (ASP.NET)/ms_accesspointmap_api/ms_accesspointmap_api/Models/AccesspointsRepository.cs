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

        public async Task CreateOrUpdate(Accesspoints accesspoint)
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
#pragma warning disable CS0168 // Variable is declared but never used
                catch (HttpRequestException e)
#pragma warning restore CS0168 // Variable is declared but never used
                {
                    accesspoint.Brand = "No brand info";
                }

                //Set the device type, currently no practical use for this variable, but i keep it for the future
                accesspoint.DeviceType = "default";

                //Add the object to the database
                await context.Accesspoints.AddAsync(accesspoint);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public async Task<Accesspoints> GetAccesspointById(int id)
        {
            return await context.Accesspoints.FirstOrDefaultAsync(accesspoint => accesspoint.Id == id);
        }

        public async Task<IEnumerable<Accesspoints>> GetAccesspoints()
        {
            return await context.Accesspoints.ToListAsync();
        }

        public async Task<int> Save()
        {
            return await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Accesspoints>> SearchAccesspoints(string ssid=null, int freq=0, string brand=null, string security=null)
        {
            return await context.Accesspoints.Where(element =>
                (element.Ssid.ToUpper() == ssid.ToUpper() || ssid == null) &&
                (element.Frequency == freq || freq == 0) &&
                (element.Brand.ToUpper().Contains(brand.ToUpper()) || brand == null) &&
                (element.SecurityData.ToUpper().Contains(security.ToUpper()) || security == null)
            ).ToListAsync();       
        }
    }
}
