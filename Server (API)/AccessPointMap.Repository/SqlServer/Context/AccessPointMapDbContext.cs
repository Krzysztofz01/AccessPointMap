using AccessPointMap.Domain;
using AccessPointMap.Repository.SqlServer.Maps;
using Microsoft.EntityFrameworkCore;

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
    }
}
