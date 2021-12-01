using System;
using System.Threading.Tasks;

namespace AccessPointMap.Domain.Identities
{
    public interface IIdentityRepository
    {
        Task Add(Identity identity);
        Task<Identity> Get(Guid id);
        Task<bool> Exists(Guid id);
    }
}
