using AccessPointMap.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccessPointMap.Repository
{
    public interface IRepository<TEntity> where TEntity : BaseEntity
    {
        Task<TEntity> Get(long id);
        Task<IEnumerable<TEntity>> GetAll();
        Task Add(TEntity entity);
        Task AddRange(IEnumerable<TEntity> entitiesCollection);
        void Remove(TEntity entity, bool hard = false);
        void RemoveRange(IEnumerable<TEntity> entitiesCollection, bool hard = false);
        void UpdateState(TEntity entity);
        Task<int> Save();
    }
}
