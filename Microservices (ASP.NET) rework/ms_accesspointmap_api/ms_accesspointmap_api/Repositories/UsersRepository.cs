using ms_accesspointmap_api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ms_accesspointmap_api.Repositories
{
    public interface IUserRepository
    {
        Task<string> Login(string email, string password);
        Task<bool> Register(Users user);
        Task Update(Users user);
        Task Activate(int id, bool activate);
        Task<IEnumerable<Users>> GetAll();
        Task<Users> GetById(int id);
        Task Delete(int id);
        Task<int> SaveChanges();
    }

    public class UsersRepository : IUserRepository
    {
        private AccessPointMapContext context;

        public UsersRepository(
            AccessPointMapContext context)
        {
            this.context = context;
        }

        public Task Activate(int id, bool activate)
        {
            throw new NotImplementedException();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Users>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Users> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<string> Login(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Task Register(Users user)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChanges()
        {
            throw new NotImplementedException();
        }

        public Task Update(Users user)
        {
            throw new NotImplementedException();
        }

        Task<bool> IUserRepository.Register(Users user)
        {
            throw new NotImplementedException();
        }
    }
}
