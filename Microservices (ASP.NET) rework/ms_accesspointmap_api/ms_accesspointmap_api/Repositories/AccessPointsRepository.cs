using Microsoft.EntityFrameworkCore;
using ms_accesspointmap_api.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ms_accesspointmap_api.Repositories
{
    public interface IAccessPointsRepository
    {
        Task<IEnumerable<Accesspoints>> GetAll();
        Task<Accesspoints> GetById(int id);
        Task<IEnumerable<Accesspoints>> SearchByParams(string ssid, int freq, string brand, string security);
        Task<IEnumerable<string>> GetBrandList();
        Task<int> AddOrUpdate(List<Accesspoints> accesspoints);
        Task<bool> ChangeVisibility(int id, bool visible);
        Task<bool> Merge(List<int> accesspointsId);
        Task<bool> Delete(int id);
    }

    public class AccessPointsRepository : IAccessPointsRepository
    {
        private AccessPointMapContext context;
        private readonly IBrandRepository brandRepository;
        private readonly ILogsRepository logsRepository;
        private readonly IGuestAccesspointsRepository guestAccesspointsRepository;

        public AccessPointsRepository(
            AccessPointMapContext context,
            IBrandRepository brandRepository,
            ILogsRepository logsRepository,
            IGuestAccesspointsRepository guestAccesspointsRepository)
        {
            this.context = context;
            this.brandRepository = brandRepository;
            this.guestAccesspointsRepository = guestAccesspointsRepository;
        }

        public async Task<int> AddOrUpdate(List<Accesspoints> accesspoints)
        {
            foreach(var element in accesspoints)
            {
                //Check if element with given BSSID is already in database
                if(context.Accesspoints.Any(accesspoint => accesspoint.Bssid == element.Bssid))
                {
                    //Update element
                    var accesspoint = context.Accesspoints.Where(accesspoint => accesspoint.Bssid == element.Bssid).First();
                    bool locationChanged = false;
                    bool globalChanges = false;

                    //Low signal check, swap if weaker
                    if(element.LowSignalLevel < accesspoint.LowSignalLevel)
                    {
                        accesspoint.LowSignalLevel = element.LowSignalLevel;
                        accesspoint.LowLatitude = element.LowLatitude;
                        accesspoint.LowLongitude = element.LowLongitude;
                        locationChanged = true;
                        globalChanges = true;
                    }

                    //High signal check, swap if stronger
                    if(element.HighSignalLevel > accesspoint.HighSignalLevel)
                    {
                        accesspoint.HighSignalLevel = element.HighSignalLevel;
                        accesspoint.HighLatitude = element.HighLatitude;
                        accesspoint.HighLongitude = element.HighLongitude;
                        locationChanged = true;
                        globalChanges = true;
                    }

                    //Calculate location fields, if location changed
                    if(locationChanged)
                    {
                        accesspoint.SignalRadius = HaversineFormula(accesspoint.LowLatitude, accesspoint.LowLongitude, accesspoint.HighLatitude, accesspoint.HighLongitude);
                        accesspoint.SignalArea = Math.Round(3.1415 * Math.Pow(accesspoint.SignalRadius, 2.0), 2);
                    }

                    if(accesspoint.SecurityDataRaw != element.SecurityDataRaw)
                    {
                        accesspoint.SecurityDataRaw = element.SecurityDataRaw;
                        accesspoint.SecurityData = SecurityDataFormat(element.SecurityDataRaw);
                        globalChanges = true;
                    }

                    if(string.IsNullOrEmpty(accesspoint.SecurityData))
                    {
                        accesspoint.SecurityData = SecurityDataFormat(element.SecurityDataRaw);
                        globalChanges = true;
                    }

                    if(string.IsNullOrEmpty(accesspoint.Brand) || accesspoint.Brand == "No brand info")
                    {
                        accesspoint.Brand = await brandRepository.GetByBssid(accesspoint.Bssid);
                        globalChanges = true;
                    }

                    if(globalChanges)
                    {
                        accesspoint.UpdateDate = DateTime.Now;
                        context.Entry(accesspoint).State = EntityState.Modified;
                    }
                }
                else
                {
                    //Create element
                    element.SignalRadius = HaversineFormula(element.LowLatitude, element.LowLongitude, element.HighLatitude, element.HighLongitude);
                    element.SignalArea = AreaFormule(element.SignalRadius);
                    element.SecurityData = SecurityDataFormat(element.SecurityDataRaw);
                    element.Brand = await brandRepository.GetByBssid(element.Bssid);
                    element.DeviceType = "Default";

                    context.Accesspoints.Add(element);
                }
            }

            return await context.SaveChangesAsync();
        }

        public async Task<bool> ChangeVisibility(int id, bool visible)
        {
            var accesspoint = context.Accesspoints.Where(accesspoint => accesspoint.Id == id).First();
            if(accesspoint != null)
            {
                accesspoint.Display = visible;
                context.Entry(accesspoint).State = EntityState.Modified;
                if(await context.SaveChangesAsync() > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<bool> Delete(int id)
        {
            var accesspoint = context.Accesspoints.Where(accesspoint => accesspoint.Id == id).First();
            if(accesspoint != null)
            {
                context.Entry(accesspoint).State = EntityState.Deleted;
                if(await context.SaveChangesAsync() > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<IEnumerable<Accesspoints>> GetAll()
        {
            return await context.Accesspoints.Where(accesspoint => accesspoint.Display == true).ToListAsync();
        }

        public async Task<Accesspoints> GetById(int id)
        {
            return await context.Accesspoints.Where(accesspoint => accesspoint.Id == id && accesspoint.Display == true).FirstAsync(); 
        }

        public async Task<bool> Merge(List<int> accesspointsId)
        {
            foreach(var id in accesspointsId)
            {
                var guestAccessPoint = await guestAccesspointsRepository.GetById(id);
                var masterAccessPoint = context.Accesspoints.Where(element => element.Bssid == guestAccessPoint.Bssid).First();
                if(masterAccessPoint != null)
                {
                    //Update existing accesspoint
                    bool locationChanged = false;
                    bool globalChanges = false;

                    //Low signal check, swap if weaker
                    if (guestAccessPoint.LowSignalLevel < masterAccessPoint.LowSignalLevel)
                    {
                        masterAccessPoint.LowSignalLevel = guestAccessPoint.LowSignalLevel;
                        masterAccessPoint.LowLatitude = guestAccessPoint.LowLatitude;
                        masterAccessPoint.LowLongitude = guestAccessPoint.LowLongitude;
                        locationChanged = true;
                        globalChanges = true;
                    }

                    //High signal check, swap if stronger
                    if (guestAccessPoint.HighSignalLevel > masterAccessPoint.HighSignalLevel)
                    {
                        masterAccessPoint.HighSignalLevel = guestAccessPoint.HighSignalLevel;
                        masterAccessPoint.HighLatitude = guestAccessPoint.HighLatitude;
                        masterAccessPoint.HighLongitude = guestAccessPoint.HighLongitude;
                        locationChanged = true;
                        globalChanges = true;
                    }

                    //Calculate location fields, if location changed
                    if (locationChanged)
                    {
                        masterAccessPoint.SignalRadius = HaversineFormula(masterAccessPoint.LowLatitude, masterAccessPoint.LowLongitude, masterAccessPoint.HighLatitude, masterAccessPoint.HighLongitude);
                        masterAccessPoint.SignalArea = Math.Round(3.1415 * Math.Pow(masterAccessPoint.SignalRadius, 2.0), 2);
                    }

                    if (masterAccessPoint.SecurityDataRaw != guestAccessPoint.SecurityDataRaw)
                    {
                        masterAccessPoint.SecurityDataRaw = guestAccessPoint.SecurityDataRaw;
                        masterAccessPoint.SecurityData = SecurityDataFormat(guestAccessPoint.SecurityDataRaw);
                        globalChanges = true;
                    }

                    if (string.IsNullOrEmpty(masterAccessPoint.SecurityData))
                    {
                        masterAccessPoint.SecurityData = SecurityDataFormat(guestAccessPoint.SecurityDataRaw);
                        globalChanges = true;
                    }

                    if (globalChanges)
                    {
                        masterAccessPoint.UpdateDate = DateTime.Now;
                        context.Entry(masterAccessPoint).State = EntityState.Modified;
                    }
                }
                else
                {
                    //Create a new accesspoint
                    var accesspoint = new Accesspoints()
                    {
                        Bssid = guestAccessPoint.Bssid,
                        Ssid = guestAccessPoint.Ssid,
                        Frequency = guestAccessPoint.Frequency,
                        HighSignalLevel = guestAccessPoint.HighSignalLevel,
                        HighLatitude = guestAccessPoint.HighLatitude,
                        HighLongitude = guestAccessPoint.HighLongitude,
                        LowSignalLevel = guestAccessPoint.LowSignalLevel,
                        LowLatitude = guestAccessPoint.LowLatitude,
                        LowLongitude = guestAccessPoint.LowLongitude,
                        SecurityDataRaw = guestAccessPoint.SecurityDataRaw,
                        PostedBy = guestAccessPoint.PostedBy
                    };

                    accesspoint.SignalRadius = HaversineFormula(accesspoint.LowLatitude, accesspoint.LowLongitude, accesspoint.HighLatitude, accesspoint.HighLongitude);
                    accesspoint.SignalArea = AreaFormule(accesspoint.SignalRadius);
                    accesspoint.SecurityData = SecurityDataFormat(accesspoint.SecurityDataRaw);
                    accesspoint.Brand = await brandRepository.GetByBssid(accesspoint.Bssid);
                    accesspoint.DeviceType = "Default";

                    context.Accesspoints.Add(accesspoint);
                }
            }

            if (await context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Accesspoints>> SearchByParams(string ssid, int freq, string brand, string security)
        {
            return await context.Accesspoints.Where(element =>
                (element.Ssid.ToUpper().Contains((!string.IsNullOrEmpty(ssid)) ? ssid.ToUpper() : element.Ssid.ToUpper())) &&
                (element.Frequency == ((freq != 0) ? freq : element.Frequency)) &&
                (element.Brand.ToUpper().Contains((!string.IsNullOrEmpty(brand)) ? brand.ToUpper() : element.Brand.ToUpper())) &&
                (element.SecurityData.Contains((!string.IsNullOrEmpty(security)) ? security.ToUpper() : element.SecurityData)) &&
                (element.Display == true)
                ).ToListAsync();
        }

        public async Task<IEnumerable<string>> GetBrandList()
        {
            return await context.Accesspoints.Where(element => element.Display == true).Select(param => param.Brand).Distinct().ToListAsync();
        }

        private double HaversineFormula(double lat1, double lon1, double lat2, double lon2)
        {
            //Algorithm source: www.movable-type.co.uk/scripts/latlong.html
            const double pi = 3.1415;
            const double R = 6371e3;
            double o1 = lat1 * pi / 180.0;
            double o2 = lat2 * pi / 180.0;
            double so = (lat2 - lat1) * pi / 180.0;
            double sl = (lon2 - lon1) * pi / 180.0;
            double a = Math.Pow(Math.Sin(so / 2.0), 2.0) + Math.Cos(o1) * Math.Cos(o2) * Math.Pow(Math.Sin(sl / 2.0), 2.0);
            double c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));
            return R * c;
        }

        private double AreaFormule(double radius)
        {
            return Math.Round(3.1415 * Math.Pow(radius, 2.0), 2);
        }

        private string SecurityDataFormat(string securityData)
        {
            string[] types = { "BSS", "ESS", "WEP", "WPA", "WPA2", "WPS" };
            List<string> outputTypes = new List<string>();
            foreach(string type in types)
            {
                if (securityData.Contains(type)) outputTypes.Add(type);
            }
            return JsonConvert.SerializeObject(outputTypes);
        }
    }
}
