using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Domain.Identities;
using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AccessPointMap.Infrastructure.MySql
{
    public class DataAccess : IDataAccess
    {
        private readonly AccessPointMapDbContext _dbContext;
        public DataAccess(AccessPointMapDbContext dbContext) =>
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));


        public IQueryable<Identity> Identities => _dbContext.Identities.AsNoTracking();

        public IQueryable<AccessPoint> AccessPoints => _dbContext.AccessPoints.AsNoTracking();

        public IQueryable<Identity> IdentitiesTracked => _dbContext.Identities;

        public IQueryable<AccessPoint> AccessPointsTracked => _dbContext.AccessPoints;
    }
}
