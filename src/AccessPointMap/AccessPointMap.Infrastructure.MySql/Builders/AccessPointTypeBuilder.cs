using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Infrastructure.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessPointMap.Infrastructure.MySql.Builders
{
    public class AccessPointTypeBuilder
    {
        public AccessPointTypeBuilder(EntityTypeBuilder<AccessPoint> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.OwnsOne(e => e.Bssid).HasIndex(v => v.Value).IsUnique();
            builder.Navigation(e => e.Bssid).IsRequired();

            builder.OwnsRequiredOne(e => e.Manufacturer);
            builder.OwnsRequiredOne(e => e.Ssid);  
            builder.OwnsOne(e => e.Frequency);
            builder.OwnsRequiredOne(e => e.DeviceType);
            builder.OwnsOne(e => e.ContributorId);
            builder.OwnsOne(e => e.CreationTimestamp);
            builder.OwnsOne(e => e.VersionTimestamp);
            builder.OwnsOne(e => e.Positioning);
            builder.OwnsOne(e => e.Security);
            builder.OwnsRequiredOne(e => e.Note);
            builder.OwnsOne(e => e.DisplayStatus);

            builder.OwnsMany(e => e.Stamps, e =>
            {
                e.WithOwner().HasForeignKey("accesspointId");
                e.HasKey(e => e.Id);
                e.Property(e => e.Id).ValueGeneratedNever();
                e.OwnsRequiredOne(e => e.Ssid);
                e.OwnsOne(e => e.Frequency);
                e.OwnsRequiredOne(e => e.DeviceType);
                e.OwnsOne(e => e.ContributorId);
                e.OwnsOne(e => e.CreationTimestamp);
                e.OwnsOne(e => e.Positioning);
                e.OwnsOne(e => e.Security);
                e.OwnsOne(e => e.Status);

                e.Property(e => e.DeletedAt).HasDefaultValue(null);
            });

            builder.OwnsMany(e => e.Adnnotations, e =>
            {
                e.WithOwner().HasForeignKey("accesspointId");
                e.HasKey(e => e.Id);
                e.Property(e => e.Id).ValueGeneratedNever();
                e.OwnsRequiredOne(e => e.Title);
                e.OwnsRequiredOne(e => e.Content);
                e.OwnsOne(e => e.Timestamp);

                e.Property(e => e.DeletedAt).HasDefaultValue(null);
            });

            builder.OwnsMany(e => e.Packets, e =>
            {
                e.WithOwner().HasForeignKey("accesspointId");
                e.HasKey(e => e.Id);
                e.Property(e => e.Id).ValueGeneratedNever();
                e.OwnsRequiredOne(e => e.DestinationAddress);
                e.OwnsRequiredOne(e => e.FrameType);
                e.OwnsRequiredOne(e => e.Data);

                e.Property(e => e.DeletedAt).HasDefaultValue(null);
            });

            builder.Property(e => e.DeletedAt).HasDefaultValue(null);

            builder.HasQueryFilter(e => e.DeletedAt == null);
        }
    }
}
