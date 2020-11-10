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
        Task<IEnumerable<Logs>> GetAllFromLastDays(int days);
        Task LogMasterAccessPointAdd();
        Task LogGuestAccessPointAdd();
        Task LogUserRegistration();
        Task LogUserActivation();
    }

    public class LogsRepository
    {
        private AccessPointMapContext context;

        public LogsRepository(
            AccessPointMapContext context)
        {
            this.context = context;
        }
    }
}
