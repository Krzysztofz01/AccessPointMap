using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Domain.Identities;
using System;
using System.Linq;

namespace AccessPointMap.Infrastructure.Core.Abstraction
{
    [Obsolete("The IDataAccess will be removed in the future. Use the the repository methods to query data from the database.")]
    public interface IDataAccess
    {
        IQueryable<Identity> Identities { get; }
        IQueryable<AccessPoint> AccessPoints { get; }

        IQueryable<Identity> IdentitiesTracked { get; }
        IQueryable<AccessPoint> AccessPointsTracked { get; }
    }
}
