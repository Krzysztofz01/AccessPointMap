using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ms_accesspointmap_api.Models
{
    interface IUsersRepository : IDisposable
    {
        string login(string login, string password);
    }
}
