using AccessPointMap.Domain;
using AccessPointMap.Repository.Context;

namespace AccessPointMap.Repository
{
    public class AccessPointRepository : Repository<AccessPoint>, IAccessPointRepository
    {
        public AccessPointRepository(AccessPointMapDbContext context): base(context)
        {
        }
    }
}
