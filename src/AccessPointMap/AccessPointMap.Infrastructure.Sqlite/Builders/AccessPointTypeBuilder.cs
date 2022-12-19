using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Infrastructure.Core.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessPointMap.Infrastructure.Sqlite.Builders
{
    internal sealed class AccessPointTypeBuilder
    {
        public AccessPointTypeBuilder(EntityTypeBuilder<AccessPoint> builder)
        {
            builder.HasPublicKey();

            builder.OwnsOneValueObject(e => e.Bssid).HasIndex(v => v.Value).IsUnique();
            builder.OwnsOneValueObject(e => e.Manufacturer); 
            builder.OwnsOneValueObject(e => e.Ssid);
            builder.OwnsOneValueObject(e => e.Frequency);
            builder.OwnsOneValueObject(e => e.DeviceType);
            builder.OwnsOneSharedValueObject(e => e.ContributorId);
            builder.OwnsOneSharedValueObject(e => e.CreationTimestamp);
            builder.OwnsOneSharedValueObject(e => e.VersionTimestamp);
            builder.OwnsOneValueObject(e => e.Positioning);
            builder.OwnsOneValueObject(e => e.Security);
            builder.OwnsOneValueObject(e => e.Note);
            builder.OwnsOneValueObject(e => e.RunIdentifier);
            builder.OwnsOneValueObject(e => e.DisplayStatus);
            builder.OwnsOneValueObject(e => e.Presence);

            builder.OwnsMany(e => e.Stamps, e =>
            {
                e.HasPublicKey();

                e.OwnsOneValueObject(e => e.Ssid);
                e.OwnsOneValueObject(e => e.Frequency);
                e.OwnsOneValueObject(e => e.DeviceType);
                e.OwnsOneSharedValueObject(e => e.ContributorId);
                e.OwnsOneSharedValueObject(e => e.CreationTimestamp);
                e.OwnsOneValueObject(e => e.Positioning);
                e.OwnsOneValueObject(e => e.Security);
                e.OwnsOneValueObject(e => e.Status);
                e.OwnsOneValueObject(e => e.RunIdentifier);

                e.UseSoftDelete();
            });

            builder.OwnsMany(e => e.Adnnotations, e =>
            {
                e.HasPublicKey();

                e.OwnsOneValueObject(e => e.Title);
                e.OwnsOneValueObject(e => e.Content);
                e.OwnsOneSharedValueObject(e => e.Timestamp);

                e.UseSoftDelete();
            });

            builder.OwnsMany(e => e.Packets, e =>
            {
                e.HasPublicKey();

                e.OwnsOneValueObject(e => e.DestinationAddress);
                e.OwnsOneValueObject(e => e.FrameType);
                e.OwnsOneValueObject(e => e.Data);

                e.UseSoftDelete();
            });

            builder.UseSoftDelete();
        }
    }
}
