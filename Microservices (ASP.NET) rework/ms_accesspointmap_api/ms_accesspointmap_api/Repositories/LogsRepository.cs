using ms_accesspointmap_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ms_accesspointmap_api.Repositories
{
    public interface ILogsRepository
    {
        Task<IEnumerable<Logs>> GetAll();
        Task<IEnumerable<Logs>> Search(int id, string status, string endpoint, DateTime dateTime);
        Task Add(Logs log);
        Task<int> SaveChanges();
    }

    public class LogsRepository : ILogsRepository
    {
        private AccessPointMapContext context;

        public LogsRepository(
            AccessPointMapContext context)
        {
            this.context = context;
        }

        public Task Add(Logs log)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Logs>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Logs>> Search(int id, string status, string endpoint, DateTime dateTime)
        {
            throw new NotImplementedException();
        }
    }
}
