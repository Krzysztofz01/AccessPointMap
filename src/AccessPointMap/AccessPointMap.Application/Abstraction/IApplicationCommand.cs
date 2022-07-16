using AccessPointMap.Domain.Core.Models;

namespace AccessPointMap.Application.Abstraction
{
    public interface IApplicationCommand<TAggreagreRoot> : ICommand where TAggreagreRoot : AggregateRoot 
    {
    }
}
