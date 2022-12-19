using AccessPointMap.Domain.Identities;
using AccessPointMap.Infrastructure.Core.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessPointMap.Infrastructure.Sqlite.Builders
{
    internal sealed class IdentityTypeBuilder
    {
        public IdentityTypeBuilder(EntityTypeBuilder<Identity> builder)
        {
            builder.HasPublicKey();

            builder.OwnsOneValueObject(e => e.Name).Property(v => v.Value).HasMaxLength(40);
            builder.OwnsOneValueObject(e => e.Email).HasIndex(v => v.Value).IsUnique();
            builder.OwnsOneValueObject(e => e.PasswordHash);
            builder.OwnsOneValueObject(e => e.LastLogin);
            builder.OwnsOneValueObject(e => e.Role);
            builder.OwnsOneValueObject(e => e.Activation);

            builder.OwnsMany(e => e.Tokens, e =>
            {
                e.HasPublicKey();

                // TODO: Other values should be also included here in order to be more verbose, but the default config works
                e.HasIndex(e => e.TokenHash).IsUnique();

                e.UseSoftDelete();
            });

            builder.UseSoftDelete();
        }
    }
}
