using Microsoft.EntityFrameworkCore;
using AccessPointMapWebApi.Models;

#nullable disable

namespace AccessPointMapWebApi.DatabaseContext
{
    public partial class AccessPointMapContext : DbContext
    {
        public AccessPointMapContext()
        {
        }

        public AccessPointMapContext(DbContextOptions<AccessPointMapContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Accesspoint> Accesspoints { get; set; }
        public virtual DbSet<GuestAccesspoint> GuestAccesspoints { get; set; }
        public virtual DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accesspoint>(entity =>
            {
                entity.HasIndex(e => e.Bssid, "UQ__Accesspo__2AD8EB7FFD5D1F67")
                    .IsUnique();

                entity.Property(e => e.Brand)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('No brand info')");

                entity.Property(e => e.Bssid)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate)
                    .HasPrecision(6)
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeviceType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Display).HasDefaultValueSql("((1))");

                entity.Property(e => e.PostedBy)
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('Admin')");

                entity.Property(e => e.SecurityData)
                    .IsRequired()
                    .HasMaxLength(65)
                    .IsUnicode(false);

                entity.Property(e => e.SecurityDataRaw)
                    .IsRequired()
                    .HasMaxLength(65)
                    .IsUnicode(false);

                entity.Property(e => e.Ssid)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate)
                    .HasPrecision(6)
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<GuestAccesspoint>(entity =>
            {
                entity.HasIndex(e => e.Bssid, "UQ__GuestAcc__2AD8EB7F427200E8")
                    .IsUnique();

                entity.Property(e => e.Bssid)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate)
                    .HasPrecision(6)
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeviceType)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PostedBy)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.SecurityDataRaw)
                    .IsRequired()
                    .HasMaxLength(65)
                    .IsUnicode(false);

                entity.Property(e => e.Ssid)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasIndex(e => e.Email, "UQ__Users__A9D105347A599643")
                    .IsUnique();

                entity.Property(e => e.Active).HasDefaultValueSql("((0))");

                entity.Property(e => e.AdminPermission).HasDefaultValueSql("((0))");

                entity.Property(e => e.CreateDate)
                    .HasPrecision(6)
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(254)
                    .IsUnicode(false);

                entity.Property(e => e.LastLoginDate)
                    .HasPrecision(6)
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastLoginIp)
                    .HasMaxLength(45)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(128)
                    .IsUnicode(false);

                entity.Property(e => e.ReadPermission).HasDefaultValueSql("((0))");

                entity.Property(e => e.WritePermission).HasDefaultValueSql("((0))");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
