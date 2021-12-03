using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Domain.Core.Models;
using AccessPointMap.Domain.Identities;
using AccessPointMap.Infrastructure.MySql.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Infrastructure.MySql
{
    public class AccessPointMapDbContext : DbContext
    {
        private const string _schemaName = "apm";

        public virtual DbSet<Identity> Identities { get; set; }
        public virtual DbSet<AccessPoint> AccessPoints { get; set; }

        public AccessPointMapDbContext(DbContextOptions<AccessPointMapDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema(_schemaName);

            new IdentityTypeBuilder(modelBuilder.Entity<Identity>());

            new AccessPointTypeBuilder(modelBuilder.Entity<AccessPoint>());
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entites = ChangeTracker
                .Entries()
                .Where(e => e.Entity is AuditableSubject &&
                    (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entity in entites)
            {
                ((AuditableSubject)entity.Entity).UpdatedAt = DateTime.Now;

                if (entity.State == EntityState.Added)
                {
                    ((AuditableSubject)entity.Entity).UpdatedAt = DateTime.Now;
                }
            }

            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
