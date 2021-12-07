using AccessPointMap.Domain.Identities;

namespace AccessPointMap.Infrastructure.Core.Abstraction
{
    public interface IAuthenticationWrapperService
    {
        bool VerifyPasswordHashes(string contractPassword, string storedPasswordHash);
        string HashPassword(string contractPasswordString);
        string HashString(string contractValue);
        bool ValidatePasswords(string contractPasswordString, string contractPasswordStringRepeat);
        string GenerateJsonWebToken(Identity identity);
        string GenerateRefreshToken();
    }
}
