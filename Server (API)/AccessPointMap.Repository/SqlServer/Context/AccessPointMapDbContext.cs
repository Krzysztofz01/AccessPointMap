using AccessPointMap.Domain;
using AccessPointMap.Domain.Common;
using AccessPointMap.Repository.SqlServer.Maps;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Repository.SqlServer.Context
{
    public class AccessPointMapDbContext : DbContext
    {
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<AccessPoint> AccessPoints { get; set; }

        public AccessPointMapDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //User mapping
            new UserMap(modelBuilder.Entity<User>());

            //Refresh token mapping
            new RefreshTokenMap(modelBuilder.Entity<RefreshToken>());

            //AccessPoint mapping
            new Maps.AccessPointMap(modelBuilder.Entity<AccessPoint>());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entires = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entity in entires)
            {
                ((BaseEntity)entity.Entity).SetModified();

                if (entity.State == EntityState.Added)
                {
                    ((BaseEntity)entity.Entity).SetCreated();
                }

                ((BaseEntity)entity.Entity).Validate();
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
