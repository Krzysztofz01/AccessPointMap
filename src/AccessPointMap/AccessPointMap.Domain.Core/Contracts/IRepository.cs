using AccessPointMap.Domain.Core.Models;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Domain.Core.Contracts
{
    public interface IRepository<TAggregateRoot> where TAggregateRoot : AggregateRoot
    {
        IQueryable<TAggregateRoot> Entities { get; }

        Task AddAsync(TAggregateRoot entity, CancellationToken cancellationToken = default);
        Task<TAggregateRoot> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}