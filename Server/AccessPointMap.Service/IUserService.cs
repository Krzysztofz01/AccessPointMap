using AccessPointMap.Service.Dto;
using AccessPointMap.Service.Handlers;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AccessPointMap.Service
{
    public interface IUserService
    {
        Task<ServiceResult<AuthDto>> Authenticate(string email, string password, string ipAddress);
        Task<ServiceResult<AuthDto>> RefreshToken(string token, string ipAddress);
        Task<IServiceResult> RevokeToken(string token, string ipAddress);
        Task<IServiceResult> Register(string name, string email, string password, string ipAddress);
        Task<IServiceResult> Reset(string email, string password, string ipAddress);

        ServiceResult<IEnumerable<UserDto>> GetAll();
        Task<ServiceResult<UserDto>> Get(long userId);
        Task<IServiceResult> Delete(long userId);
        Task<IServiceResult> Activation(long userId);
        Task<IServiceResult> Update(UserDto user, long userId);

        long GetUserIdFromPayload(IEnumerable<Claim> claims);
        UserDto GetUserFromPayload(IEnumerable<Claim> claims);
    }
}
