using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ms_accesspointmap_api.Models
{
    public interface IUsersRepository : IDisposable
    {
        Task<string> login(string login, string password);
    }
}
