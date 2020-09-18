using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ms_accesspointmap_api.Models
{
    public interface IAccesspointsRepository : IDisposable
    {
        Task<IEnumerable<Accesspoints>> GetAccesspoints();
        Task<IEnumerable<Accesspoints>> SearchAccesspoints(string ssid, int freq, string brand, string security);
        Task<Accesspoints> GetAccesspointById(int id);
        Task CreateOrUpdate(Accesspoints accesspoint);
        Task<int> Save();
    }
}
