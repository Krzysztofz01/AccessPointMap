using AccessPointMap.Domain.Identities;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Infrastructure.Core.Abstraction
{
    public interface IAuthenticationDataAccessService
    {
        Task<Identity> GetUserOrDefaultByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<Identity> GetUserOrDefaultByActiveRefreshTokenAsync(string refreshTokenHash, CancellationToken cancellationToken = default);
        Task<bool> AnyUserWithEmailExsitsAsync(string email, CancellationToken cancellationToken = default);
        Task<bool> AnyUserExistsAsync(CancellationToken cancellationToken = default);
    }
}
