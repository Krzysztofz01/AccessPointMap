using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ms_accesspointmap_api.Models
{
    public class UsersRepository : IUsersRepository, IDisposable
    {
        private AccessPointMapContext context;

        public UsersRepository(AccessPointMapContext context)
        {
            this.context = context;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public string login(string login, string password)
        {
            throw new NotImplementedException();
        }
    }
}
