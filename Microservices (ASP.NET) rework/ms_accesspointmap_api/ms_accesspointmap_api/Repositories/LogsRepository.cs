using Microsoft.EntityFrameworkCore;
using ms_accesspointmap_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ms_accesspointmap_api.Repositories
{
    public interface ILogsRepository
    {
        /*Task<IEnumerable<Logs>> GetAll();
        Task<IEnumerable<Logs>> GetAllFromLastDays(int days);
        Task LogMasterAccessPointAdd(string eventSource);
        Task LogGuestAccessPointAdd(string eventSource);
        Task LogUserRegistration(string eventSource);
        Task LogUserActivation(string eventSource);*/
    }

    public class LogsRepository //: ILogsRepository
    {
        /*private AccessPointMapContext context;

        public LogsRepository(
            AccessPointMapContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Logs>> GetAll()
        {
            return await context.Logs.ToListAsync();
        }

        public async Task<IEnumerable<Logs>> GetAllFromLastDays(int days)
        {
            return await context.Logs.Where(element => element.EventDate >= DateTime.Now.AddDays(days * -1)).ToListAsync();
        }

        public async Task LogGuestAccessPointAdd(string eventSource)
        {
            var log = new Logs();
            log.Status = "";
            log.Endpoint = "GUEST ADD";
            log.Description = eventSource;
            log.EventDate = DateTime.Now;
            await context.Logs.AddAsync(log);
        }

        public async Task LogMasterAccessPointAdd(string eventSource)
        {
            var log = new Logs();
            log.Status = "";
            log.Endpoint = "MASTER ADD";
            log.Description = eventSource;
            log.EventDate = DateTime.Now;
            await context.Logs.AddAsync(log);
        }

        public async Task LogUserActivation(string eventSource)
        {
            var log = new Logs();
            log.Status = "";
            log.Endpoint = "USER LOG";
            log.Description = eventSource;
            log.EventDate = DateTime.Now;
            await context.Logs.AddAsync(log);
        }

        public async Task LogUserRegistration(string eventSource)
        {
            var log = new Logs();
            log.Status = "";
            log.Endpoint = "USER REGISTER";
            log.Description = eventSource;
            log.EventDate = DateTime.Now;
            await context.Logs.AddAsync(log);
        }*/
    }
}
