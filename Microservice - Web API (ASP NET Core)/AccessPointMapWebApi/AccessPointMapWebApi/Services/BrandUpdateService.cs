using AccessPointMapWebApi.Models;
using AccessPointMapWebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMapWebApi.Services
{
    public interface IBrandUpdateService
    {
        Task Update();
    }

    public class BrandUpdateService : IBrandUpdateService
    {
        private readonly IBrandRepository brandRepository;
        private readonly IAccessPointRepository accessPointRepository;

        public BrandUpdateService(
            IBrandRepository brandRepository,
            IAccessPointRepository accessPointRepository)
        {
            this.brandRepository = brandRepository;
            this.accessPointRepository = accessPointRepository;
        }

        public async Task Update()
        {
            var accesspoints = await accessPointRepository.GetAccesspointsNoBrand();
            var updatedAccesspoints = new List<Accesspoint>();
            int dailyRequestLimit = 1000;

            foreach(var accesspoint in accesspoints)
            {
                if (dailyRequestLimit <= 0) break;
                
                string brand = await brandRepository.GetByBssid(accesspoint.Bssid);
                if (brand != "No brand info")
                {
                    accesspoint.Brand = brand;
                    updatedAccesspoints.Add(accesspoint);
                }

                dailyRequestLimit--;
                Thread.Sleep(2500); 
            }
        }
    }
}
