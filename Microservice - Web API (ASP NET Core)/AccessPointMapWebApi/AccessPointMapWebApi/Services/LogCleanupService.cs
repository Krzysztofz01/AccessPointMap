using AccessPointMapWebApi.Repositories;
using System.Threading.Tasks;

namespace AccessPointMapWebApi.Services
{
    public interface ILogCleanupService
    {
        Task Cleanup();
    }

    public class LogCleanupService : ILogCleanupService
    {
        private readonly ILogsRepository logsRepository;

        public LogCleanupService(ILogsRepository logsRepository)
        {
            this.logsRepository = logsRepository;
        }

        public async Task Cleanup()
        {
            await logsRepository.Clear();
        }
    }
}
