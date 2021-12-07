using AccessPointMap.Domain.AccessPoints;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Abstraction
{
    public interface IAccessPointService
    {
        Task Handle(IApplicationCommand<AccessPoint> command);
    }
}
