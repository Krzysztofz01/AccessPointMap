using AccessPointMapWebApi.DatabaseContext;
using AccessPointMapWebApi.Models;
using AccessPointMapWebApi.Services;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPointMapWebApi.Repositories
{
    public interface IGuestAccesspointRepository
    {
        Task<IEnumerable<GuestAccesspoint>> GetAll();
        Task<GuestAccesspoint> GetById(int id);
        Task<int> Add(List<GuestAccesspoint> accesspoints);
        Task<bool> Delete(int id);
        Task DeleteLazy(int id);
    }
    public class GuestAccesspointRepository : IGuestAccesspointRepository
    {
        private AccessPointMapContext context;
        private IGeocalculationService geocalculationService;

        public GuestAccesspointRepository(
            AccessPointMapContext context,
            IGeocalculationService geocalculationService)
        {
            this.context = context;
            this.geocalculationService = geocalculationService;
        }

        public async Task<int> Add(List<GuestAccesspoint> accesspoints)
        {
            foreach (var accesspoint in accesspoints)
            {
                accesspoint.SignalRadius = geocalculationService.getDistance(accesspoint.LowLatitude, accesspoint.LowLongitude, accesspoint.HighLatitude, accesspoint.HighLongitude);
                accesspoint.SignalArea = geocalculationService.getArea(accesspoint.SignalRadius);
                accesspoint.DeviceType = "Default";

                context.GuestAccesspoints.Add(accesspoint);
            }

            return await context.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var accesspoint = context.GuestAccesspoints.Where(accesspoint => accesspoint.Id == id).FirstOrDefault();
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

        public async Task DeleteLazy(int id)
        {
            var accesspoint = await context.GuestAccesspoints.Where(accesspoint => accesspoint.Id == id).FirstOrDefaultAsync();
            if (accesspoint != null) context.Entry(accesspoint).State = EntityState.Deleted;
        }

        public async Task<IEnumerable<GuestAccesspoint>> GetAll()
        {
            return await context.GuestAccesspoints.ToListAsync();
        }

        public async Task<GuestAccesspoint> GetById(int id)
        {
            return await context.GuestAccesspoints.Where(element => element.Id == id).FirstOrDefaultAsync();
        }
    }
}
