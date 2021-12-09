using AccessPointMap.Domain.Core.Models;

namespace AccessPointMap.Application.Abstraction
{
    public interface IApplicationCommand<TAggreagreRoot> where TAggreagreRoot : AggregateRoot
    {
    }
}
