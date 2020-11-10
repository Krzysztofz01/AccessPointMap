using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ms_accesspointmap_api.Models
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

        public virtual DbSet<Accesspoints> Accesspoints { get; set; }
        public virtual DbSet<GuestAccesspoints> GuestAccesspoints { get; set; }
        public virtual DbSet<Logs> Logs { get; set; }
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accesspoints>(entity =>
            {
                entity.HasIndex(e => e.Bssid)
                    .HasName("UQ__Accesspo__2AD8EB7F26EB63FB")
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
                    .HasColumnType("datetime2(6)")
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
                    .HasColumnType("datetime2(6)")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<GuestAccesspoints>(entity =>
            {
                entity.HasIndex(e => e.Bssid)
                    .HasName("UQ__GuestAcc__2AD8EB7FBD283DE0")
                    .IsUnique();

                entity.Property(e => e.Bssid)
                    .IsRequired()
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime2(6)")
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

            modelBuilder.Entity<Logs>(entity =>
            {
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false);

                entity.Property(e => e.Endpoint)
                    .IsRequired()
                    .HasMaxLength(35)
                    .IsUnicode(false);

                entity.Property(e => e.EventDate)
                    .HasColumnType("datetime2(6)")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.HasIndex(e => e.Login)
                    .HasName("UQ__Users__5E55825B9CBED1FA")
                    .IsUnique();

                entity.Property(e => e.Active).HasDefaultValueSql("((0))");

                entity.Property(e => e.CreateDate)
                    .HasColumnType("datetime2(6)")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.LastLoginDate)
                    .HasColumnType("datetime2(6)")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastLoginIp)
                    .IsRequired()
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(60)
                    .IsUnicode(false);

                entity.Property(e => e.Token)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
