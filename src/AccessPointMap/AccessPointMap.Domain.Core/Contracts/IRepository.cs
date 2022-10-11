using AccessPointMap.Domain.Core.Models;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Domain.Core.Contracts
{
    public interface IRepository<TAggregateRoot> where TAggregateRoot : AggregateRoot
    {
        Task AddAsync(TAggregateRoot entity, CancellationToken cancellationToken = default);
        Task<TAggregateRoot> GetAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IQueryable<TAggregateRoot>> QueryAsync(Expression<Func<TAggregateRoot, bool>> predicate, CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Guid id, CancellationToken cancellationToken = default);
    }
}