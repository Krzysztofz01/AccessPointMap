using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ms_accesspointmap_api.Models
{
    interface IAccesspointsRepository : IDisposable
    {
        IEnumerable<Accesspoints> GetAccesspoints();
        IEnumerable<Accesspoints> SearchAccesspoints(string ssid, int freq, string brand, string security);
        Accesspoints GetAccesspointById(int id);
        void CreateOrUpdate(Accesspoints accesspoint);
        void Save();
    }
}
