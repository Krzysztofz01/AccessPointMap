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
        public virtual DbSet<Users> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
    
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Accesspoints>(entity =>
            {
                entity.ToTable("accesspoints");

                entity.HasIndex(e => e.Bssid)
                    .HasName("UQ__accesspo__6CCA71989AA50BB1")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Brand)
                    .HasColumnName("brand")
                    .HasMaxLength(60)
                    .IsUnicode(false)
                    .HasDefaultValueSql("('No brand info')");

                entity.Property(e => e.Bssid)
                    .IsRequired()
                    .HasColumnName("bssid")
                    .HasMaxLength(25)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDate)
                    .HasColumnName("createDate")
                    .HasColumnType("datetime2(6)")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeviceType)
                    .IsRequired()
                    .HasColumnName("deviceType")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Display)
                    .HasColumnName("display")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Frequency).HasColumnName("frequency");

                entity.Property(e => e.HighLatitude).HasColumnName("highLatitude");

                entity.Property(e => e.HighLongitude).HasColumnName("highLongitude");

                entity.Property(e => e.HighSignalLevel).HasColumnName("highSignalLevel");

                entity.Property(e => e.LowLatitude).HasColumnName("lowLatitude");

                entity.Property(e => e.LowLongitude).HasColumnName("lowLongitude");

                entity.Property(e => e.LowSignalLevel).HasColumnName("lowSignalLevel");

                entity.Property(e => e.SecurityData)
                    .IsRequired()
                    .HasColumnName("securityData")
                    .HasMaxLength(65)
                    .IsUnicode(false);

                entity.Property(e => e.SignalArea).HasColumnName("signalArea");

                entity.Property(e => e.SignalRadius).HasColumnName("signalRadius");

                entity.Property(e => e.Ssid)
                    .IsRequired()
                    .HasColumnName("ssid")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.UpdateDate)
                    .HasColumnName("updateDate")
                    .HasColumnType("datetime2(6)")
                    .HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users");

                entity.HasIndex(e => e.Login)
                    .HasName("UQ__users__7838F2722D354AF1")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CreateDate)
                    .HasColumnName("createDate")
                    .HasColumnType("datetime2(6)")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastLoginDate)
                    .HasColumnName("lastLoginDate")
                    .HasColumnType("datetime2(6)")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.LastLoginIp)
                    .IsRequired()
                    .HasColumnName("lastLoginIp")
                    .HasMaxLength(45)
                    .IsUnicode(false);

                entity.Property(e => e.Login)
                    .IsRequired()
                    .HasColumnName("login")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(50)
                    .IsFixedLength();

                entity.Property(e => e.ReadPermission).HasColumnName("readPermission");

                entity.Property(e => e.TokenExpiration).HasColumnName("tokenExpiration");

                entity.Property(e => e.WritePermission).HasColumnName("writePermission");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
