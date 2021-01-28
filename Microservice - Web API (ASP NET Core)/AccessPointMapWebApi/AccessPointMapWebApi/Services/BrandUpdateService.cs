using AccessPointMapWebApi.Models;
using AccessPointMapWebApi.Repositories;
using System.Collections.Generic;
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
        private readonly ILogsRepository logsRepository;

        public BrandUpdateService(
            IBrandRepository brandRepository,
            IAccessPointRepository accessPointRepository,
            ILogsRepository logsRepository)
        {
            this.brandRepository = brandRepository;
            this.accessPointRepository = accessPointRepository;
            this.logsRepository = logsRepository;
        }

        public async Task Update()
        {
            await logsRepository.Create("BrandUpdateService started");

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

            await logsRepository.Create("BrandUpdateService finished");
        }
    }
}
