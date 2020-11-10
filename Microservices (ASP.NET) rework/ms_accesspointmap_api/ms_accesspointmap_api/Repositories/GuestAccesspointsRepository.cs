using Microsoft.EntityFrameworkCore;
using ms_accesspointmap_api.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ms_accesspointmap_api.Repositories
{
    public interface IGuestAccesspointsRepository
    {
        Task<IEnumerable<GuestAccesspoints>> GetAll();
        Task<GuestAccesspoints> GetById(int id);
        Task<int> Add(List<GuestAccesspoints> accesspoints);
        Task<bool> Delete(int id);
    }

    public class GuestAccesspointsRepository : IGuestAccesspointsRepository
    {
        private AccessPointMapContext context;

        public GuestAccesspointsRepository(
            AccessPointMapContext context)
        {
            this.context = context;
        }

        public async Task<int> Add(List<GuestAccesspoints> accesspoints)
        {
            foreach(var accesspoint in accesspoints)
            {
                context.GuestAccesspoints.Add(accesspoint);
            }

            return await context.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var accesspoint = context.GuestAccesspoints.Where(accesspoint => accesspoint.Id == id).First();
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

        public async Task<IEnumerable<GuestAccesspoints>> GetAll()
        {
            return await context.GuestAccesspoints.ToListAsync();
        }

        public async Task<GuestAccesspoints> GetById(int id)
        {
            return await context.GuestAccesspoints.Where(element => element.Id == id).FirstAsync();
        }
    }
}
