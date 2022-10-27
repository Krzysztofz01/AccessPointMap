using AccessPointMap.Domain.Core.Models;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Core.Abstraction
{
    public interface IApplicationService<TAggregateRoot> where TAggregateRoot : AggregateRoot
    {
        public Task<Result> HandleAsync(IApplicationCommand<TAggregateRoot> command, CancellationToken cancellationToken = default);
    }
}
