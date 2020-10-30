using ms_accesspointmap_api.Models;
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
        Task AddOrUpdate(List<Accesspoints> accesspoint);
        Task ChangeVisibility(int id, bool visible);
        Task Merge(List<GuestAccesspoints> accesspoint);
        Task Delete(int id);
        Task<int> SaveChanges();
    }

    public class AccessPointsRepository : IAccessPointsRepository
    {
        private AccessPointMapContext context;

        public AccessPointsRepository(
            AccessPointMapContext context)
        {
            this.context = context;
        }

        public Task AddOrUpdate(Accesspoints accesspoint)
        {
            throw new NotImplementedException();
        }

        public Task AddOrUpdate(List<Accesspoints> accesspoint)
        {
            throw new NotImplementedException();
        }

        public Task ChangeVisibility(int id, bool visible)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Accesspoints>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Accesspoints> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task Merge(GuestAccesspoints accesspoint)
        {
            throw new NotImplementedException();
        }

        public Task Merge(List<GuestAccesspoints> accesspoint)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Accesspoints>> SearchByParams(string ssid, int freq, string brand, string security)
        {
            throw new NotImplementedException();
        }
    }
}
