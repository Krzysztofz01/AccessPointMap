using AccessPointMap.Domain.Identities;
using System.Threading.Tasks;

namespace AccessPointMap.Infrastructure.Core.Abstraction
{
    public interface IAuthenticationDataAccessService
    {
        Task<Identity> GetUserByEmail(string email);
        Task<Identity> GetUserByRefreshToken(string refreshTokenHash);
        Task<bool> UserWithEmailExsits(string email);
    }
}
