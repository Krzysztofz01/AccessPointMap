using AccessPointMap.Application.Core;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Authentication
{
    public interface IAuthenticationService
    {
        Task<Result<Responses.V1.Login>> Login(Requests.V1.Login request, CancellationToken cancellationToken = default);
        Task<Result<Responses.V1.Refresh>> Refresh(Requests.V1.Refresh request, CancellationToken cancellationToken = default);

        Task<Result> Logout(Requests.V1.Logout request, CancellationToken cancellationToken = default);
        Task<Result> PasswordReset(Requests.V1.PasswordReset requestm, CancellationToken cancellationToken = default);
        Task<Result> Register(Requests.V1.Register request, CancellationToken cancellationToken = default);
    }
}
