using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Domain.Identities;
using System.Linq;

namespace AccessPointMap.Infrastructure.Core.Abstraction
{
    public interface IDataAccess
    {
        IQueryable<Identity> Identities { get; }
        IQueryable<AccessPoint> AccessPoints { get; }

        IQueryable<Identity> IdentitiesTracked { get; }
        IQueryable<AccessPoint> AccessPointsTracked { get; }
    }
}
