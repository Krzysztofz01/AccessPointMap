using AccessPointMap.Domain.Identities;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Abstraction
{
    public interface IIdentityService
    {
        Task Handle(IApplicationCommand<Identity> command);
    }
}
