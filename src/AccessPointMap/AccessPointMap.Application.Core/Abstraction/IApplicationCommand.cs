using AccessPointMap.Domain.Core.Models;

namespace AccessPointMap.Application.Core.Abstraction
{
    public interface IApplicationCommand<TAggreagreRoot> : ICommand where TAggreagreRoot : AggregateRoot
    {
    }
}
