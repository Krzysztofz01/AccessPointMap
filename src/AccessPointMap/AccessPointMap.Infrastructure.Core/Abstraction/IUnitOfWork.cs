using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Domain.Identities;
using System.Threading.Tasks;

namespace AccessPointMap.Infrastructure.Core.Abstraction
{
    public interface IUnitOfWork
    {
        IIdentityRepository IdentityRepository { get; }
        IAccessPointRepository AccessPointRepository { get; }

        Task Commit();
    }
}
