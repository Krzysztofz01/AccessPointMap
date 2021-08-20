using AccessPointMap.Service.Maintenance.Models;
using System.Threading.Tasks;

namespace AccessPointMap.Service.Maintenance
{
    public interface ITelemetryService
    {
        Task LogEvent(Report report);
        Task Ping();
    }
}
