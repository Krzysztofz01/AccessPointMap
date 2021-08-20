using AccessPointMap.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessPointMap.Repository.MySql.Maps
{
    public class AccessPointMap
    {
        public AccessPointMap(EntityTypeBuilder<AccessPoint> entityBuilder)
        {
            //Base
            entityBuilder.HasKey(p => p.Id);
            entityBuilder.Property(p => p.AddDate).IsRequired();
            entityBuilder.Property(p => p.EditDate).IsRequired();

            entityBuilder.Property(p => p.Bssid).IsRequired();
            entityBuilder.Property(p => p.Ssid).IsRequired();
            entityBuilder.Property(p => p.Fingerprint).IsRequired();
            entityBuilder.Property(p => p.Frequency).IsRequired();
            entityBuilder.Property(p => p.MaxSignalLevel).IsRequired();
            entityBuilder.Property(p => p.MaxSignalLatitude).IsRequired();
            entityBuilder.Property(p => p.MaxSignalLongitude).IsRequired();
            entityBuilder.Property(p => p.MinSignalLevel).IsRequired();
            entityBuilder.Property(p => p.MinSignalLatitude).IsRequired();
            entityBuilder.Property(p => p.MinSignalLongitude).IsRequired();
            entityBuilder.Property(p => p.SignalRadius).IsRequired();
            entityBuilder.Property(p => p.SignalArea).IsRequired();
            entityBuilder.Property(p => p.FullSecurityData).IsRequired();
            entityBuilder.Property(p => p.SerializedSecurityData).IsRequired();
            entityBuilder.Property(p => p.Manufacturer).HasDefaultValue(null);
            entityBuilder.Property(p => p.DeviceType).HasDefaultValue(null);
            entityBuilder.Property(p => p.MasterGroup).IsRequired().HasDefaultValue(false);
            entityBuilder.Property(p => p.Display).IsRequired().HasDefaultValue(false);
            entityBuilder.Property(p => p.Note).HasDefaultValue(string.Empty);
            entityBuilder.Property(p => p.IsSecure).IsRequired().HasDefaultValue(false);
            entityBuilder.Property(p => p.IsHidden).IsRequired().HasDefaultValue(false);

            //Relations
            entityBuilder
                .HasOne<User>(p => p.UserAdded)
                .WithMany(p => p.AddedAccessPoints)
                .HasForeignKey(p => p.UserAddedId)
                .OnDelete(DeleteBehavior.NoAction);

            entityBuilder
                .HasOne<User>(p => p.UserModified)
                .WithMany(p => p.ModifiedAccessPoints)
                .HasForeignKey(p => p.UserModifiedId)
                .OnDelete(DeleteBehavior.NoAction);

            //Query filter
            entityBuilder.HasQueryFilter(e => !e.IsDeleted());
        }
    }
}
