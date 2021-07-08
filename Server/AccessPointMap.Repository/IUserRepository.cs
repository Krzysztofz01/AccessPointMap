using AccessPointMap.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccessPointMap.Repository
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User> GetSingleUserByEmail(string email);
        Task<User> GetSingleUser(long userId);
        Task<User> GetCurrentSingleUser(long userId);
        IEnumerable<User> GetAllUsers();
        Task<User> GetUserWithToken(string token);
        Task<bool> EmailAvailable(string email);
    }
}
