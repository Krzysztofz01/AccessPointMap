using ms_accesspointmap_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ms_accesspointmap_api.Repositories
{
    public interface IGuestAccesspointsRepository
    {
        Task<IEnumerable<GuestAccesspoints>> GetAll();
        Task<GuestAccesspoints> GetById(int id);
        Task AddOrUpdate(List<GuestAccesspoints> accesspoints);
        Task Delete(int id);
        Task<int> SaveChanges();
    }

    public class GuestAccesspointsRepository : IGuestAccesspointsRepository
    {
        private AccessPointMapContext context;

        public GuestAccesspointsRepository(
            AccessPointMapContext context)
        {
            this.context = context;
        }

        public Task AddOrUpdate(GuestAccesspoints accesspoints)
        {
            throw new NotImplementedException();
        }

        public Task AddOrUpdate(List<GuestAccesspoints> accesspoints)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<GuestAccesspoints>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<GuestAccesspoints> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
