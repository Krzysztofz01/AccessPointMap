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

        public async Task<string> Login(string login, string password)
        {
            var user = context.Users.SingleOrDefault(element => element.Login == login);

            if(user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password)) return null;
            
            return "Token";
   
        }

        //IDisposable
        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
