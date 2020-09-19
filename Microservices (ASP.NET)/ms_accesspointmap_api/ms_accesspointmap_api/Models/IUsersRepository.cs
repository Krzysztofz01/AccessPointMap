using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ms_accesspointmap_api.Models
{
    public interface IUsersRepository : IDisposable
    {
        Task<string> Login(string login, string password);
    }
}
