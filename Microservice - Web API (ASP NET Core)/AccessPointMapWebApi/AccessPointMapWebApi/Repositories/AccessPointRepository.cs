using AccessPointMapWebApi.DatabaseContext;
using AccessPointMapWebApi.Models;
using AccessPointMapWebApi.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPointMapWebApi.Repositories
{
    public interface IAccessPointRepository
    {
        Task<IEnumerable<Accesspoint>> GetAll();
        Task<IEnumerable<Accesspoint>> GetAllAdmin();
        Task<Accesspoint> GetById(int id);
        Task<IEnumerable<Accesspoint>> SearchByParams(string ssid, int freq, string brand, string security);
        Task<IEnumerable<string>> GetBrandList();
        Task<IEnumerable<AccessPointBrandCountDto>> GetBrandListOrderedCount();
        Task<bool> UpdateBrand(IEnumerable<Accesspoint> accesspoints);
        Task<IEnumerable<Accesspoint>> GetAccesspointsNoBrand();
        Task<int> AddOrUpdate(List<Accesspoint> accesspoints);
        Task<bool> ChangeVisibility(int id, bool visible);
        Task<bool> Merge(List<int> accesspointsId);
        Task<bool> Delete(int id);
    }

    public class AccessPointRepository : IAccessPointRepository
    {
        private AccessPointMapContext context;
        private readonly IBrandRepository brandRepository;
        private readonly IGuestAccesspointRepository guestAccesspointsRepository;
        private readonly IGeocalculationService geocalculationService;

        public AccessPointRepository(
            AccessPointMapContext context,
            IBrandRepository brandRepository,
            IGuestAccesspointRepository guestAccesspointsRepository,
            IGeocalculationService geocalculationService)
        {
            this.context = context;
            this.brandRepository = brandRepository;
            this.guestAccesspointsRepository = guestAccesspointsRepository;
            this.geocalculationService = geocalculationService;
        }

        public async Task<int> AddOrUpdate(List<Accesspoint> accesspoints)
        {
            foreach (var element in accesspoints)
            {
                element.HighLatitude = Math.Round(element.HighLatitude, 7);
                element.HighLongitude = Math.Round(element.HighLongitude, 7);
                element.LowLatitude = Math.Round(element.LowLatitude, 7);
                element.LowLongitude = Math.Round(element.LowLongitude, 7);

                //Check if element with given BSSID is already in database
                if (context.Accesspoints.Any(accesspoint => accesspoint.Bssid == element.Bssid))
                {
                    //Update element
                    var accesspoint = context.Accesspoints.Where(accesspoint => accesspoint.Bssid == element.Bssid).FirstOrDefault();
                    bool locationChanged = false;
                    bool globalChanges = false;

                    //Low signal check, swap if weaker
                    if (element.LowSignalLevel < accesspoint.LowSignalLevel)
                    {
                        accesspoint.LowSignalLevel = element.LowSignalLevel;
                        accesspoint.LowLatitude = element.LowLatitude;
                        accesspoint.LowLongitude = element.LowLongitude;
                        locationChanged = true;
                        globalChanges = true;
                    }

                    //High signal check, swap if stronger
                    if (element.HighSignalLevel > accesspoint.HighSignalLevel)
                    {
                        accesspoint.HighSignalLevel = element.HighSignalLevel;
                        accesspoint.HighLatitude = element.HighLatitude;
                        accesspoint.HighLongitude = element.HighLongitude;
                        locationChanged = true;
                        globalChanges = true;
                    }

                    //Calculate location fields, if location changed
                    if (locationChanged)
                    {
                        accesspoint.SignalRadius = Math.Round(geocalculationService.getDistance(accesspoint.LowLatitude, accesspoint.LowLongitude, accesspoint.HighLatitude, accesspoint.HighLongitude), 4);
                        accesspoint.SignalArea = Math.Round(geocalculationService.getArea(accesspoint.SignalRadius), 4);
                    }

                    if (accesspoint.SecurityDataRaw != element.SecurityDataRaw)
                    {
                        accesspoint.SecurityDataRaw = element.SecurityDataRaw;
                        accesspoint.SecurityData = SecurityDataFormat(element.SecurityDataRaw);
                        globalChanges = true;
                    }

                    if (string.IsNullOrEmpty(accesspoint.SecurityData))
                    {
                        accesspoint.SecurityData = SecurityDataFormat(element.SecurityDataRaw);
                        globalChanges = true;
                    }

                    if (string.IsNullOrEmpty(accesspoint.Brand) || accesspoint.Brand == "No brand info")
                    {
                        accesspoint.Brand = await brandRepository.GetByBssid(accesspoint.Bssid);
                        globalChanges = true;
                    }

                    if (globalChanges)
                    {
                        accesspoint.UpdateDate = DateTime.Now;
                        context.Entry(accesspoint).State = EntityState.Modified;
                    }
                }
                else
                {
                    //Create element
                    element.SignalRadius = Math.Round(geocalculationService.getDistance(element.LowLatitude, element.LowLongitude, element.HighLatitude, element.HighLongitude), 4);
                    element.SignalArea = Math.Round(geocalculationService.getArea(element.SignalRadius), 4);
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
            var accesspoint = context.Accesspoints.Where(accesspoint => accesspoint.Id == id).FirstOrDefault();
            if (accesspoint != null)
            {
                accesspoint.Display = visible;
                context.Entry(accesspoint).State = EntityState.Modified;
                if (await context.SaveChangesAsync() > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<bool> Delete(int id)
        {
            var accesspoint = context.Accesspoints.Where(accesspoint => accesspoint.Id == id).FirstOrDefault();
            if (accesspoint != null)
            {
                context.Entry(accesspoint).State = EntityState.Deleted;
                if (await context.SaveChangesAsync() > 0)
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        public async Task<IEnumerable<Accesspoint>> GetAll()
        {
            return await context.Accesspoints.Where(accesspoint => accesspoint.Display == true).ToListAsync();
        }

        public async Task<IEnumerable<Accesspoint>> GetAllAdmin()
        {
            return await context.Accesspoints.ToListAsync();
        }

        public async Task<Accesspoint> GetById(int id)
        {
            return await context.Accesspoints.Where(accesspoint => accesspoint.Id == id && accesspoint.Display == true).FirstOrDefaultAsync();
        }

        public async Task<bool> Merge(List<int> accesspointsId)
        {
            foreach (var id in accesspointsId)
            {
                var guestAccessPoint = await guestAccesspointsRepository.GetById(id);
                var masterAccessPoint = context.Accesspoints.Where(element => element.Bssid == guestAccessPoint.Bssid).FirstOrDefault();
                if (masterAccessPoint != null)
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
                        masterAccessPoint.SignalRadius = Math.Round(geocalculationService.getDistance(masterAccessPoint.LowLatitude, masterAccessPoint.LowLongitude, masterAccessPoint.HighLatitude, masterAccessPoint.HighLongitude), 4);
                        masterAccessPoint.SignalArea = Math.Round(geocalculationService.getArea(masterAccessPoint.SignalRadius), 4);
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
                    var accesspoint = new Accesspoint()
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

                    accesspoint.SignalRadius = Math.Round(geocalculationService.getDistance(accesspoint.LowLatitude, accesspoint.LowLongitude, accesspoint.HighLatitude, accesspoint.HighLongitude), 4);
                    accesspoint.SignalArea = Math.Round(geocalculationService.getArea(accesspoint.SignalRadius), 4);
                    accesspoint.SecurityData = SecurityDataFormat(accesspoint.SecurityDataRaw);
                    accesspoint.Brand = await brandRepository.GetByBssid(accesspoint.Bssid);
                    accesspoint.DeviceType = "Default";

                    context.Accesspoints.Add(accesspoint);
                }

                await guestAccesspointsRepository.DeleteLazy(id);
            }

            if (await context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Accesspoint>> SearchByParams(string ssid, int freq, string brand, string security)
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

        public async Task<IEnumerable<AccessPointBrandCountDto>> GetBrandListOrderedCount()
        {
            return await context.Accesspoints
                .GroupBy(p => p.Brand)
                .Select(p => new AccessPointBrandCountDto() { Brand = p.Key, Count = p.Count() })
                .OrderByDescending(p => p.Count).ToListAsync();
        }

        public async Task<IEnumerable<Accesspoint>> GetAccesspointsNoBrand()
        {
            return await context.Accesspoints.Where(x => x.Brand == "No brand info" && x.Display == true).ToListAsync();
        }

        public async Task<bool> UpdateBrand(IEnumerable<Accesspoint> accesspoints)
        {
            foreach(var accesspoint in accesspoints)
            {
                var existingAccesspoint = context.Accesspoints.FirstOrDefault(x => x.Id == accesspoint.Id);
                if(existingAccesspoint != null)
                {
                    existingAccesspoint.Brand = accesspoint.Brand;
                    context.Entry(existingAccesspoint).State = EntityState.Modified;
                }
            }

            if(await context.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        private string SecurityDataFormat(string securityData)
        {
            string[] types = { "BSS", "ESS", "WEP", "WPA", "WPA2", "WPS" };
            List<string> outputTypes = new List<string>();
            foreach (string type in types)
            {
                if (securityData.Contains(type)) outputTypes.Add(type);
            }
            return JsonConvert.SerializeObject(outputTypes);
        }
    }
}
