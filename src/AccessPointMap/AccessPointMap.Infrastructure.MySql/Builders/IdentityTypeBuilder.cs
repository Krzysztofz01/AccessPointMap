using AccessPointMap.Domain.Identities;
using AccessPointMap.Infrastructure.Core.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessPointMap.Infrastructure.MySql.Builders
{
    internal sealed class IdentityTypeBuilder
    {
        public IdentityTypeBuilder(EntityTypeBuilder<Identity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.OwnsOne(e => e.Name).Property(v => v.Value).HasMaxLength(40);
            builder.Navigation(e => e.Name).IsRequired();
            builder.OwnsOne(e => e.Email).HasIndex(v => v.Value).IsUnique();
            builder.Navigation(e => e.Email).IsRequired();
            builder.OwnsRequiredOne(e => e.PasswordHash);
            builder.OwnsOne(e => e.LastLogin);
            builder.OwnsOne(e => e.Role);
            builder.OwnsOne(e => e.Activation);

            builder.OwnsMany(e => e.Tokens, e =>
            {
                e.WithOwner().HasForeignKey("identityId");
                e.HasKey(e => e.Id);
                e.Property(e => e.Id).ValueGeneratedNever();
                e.HasIndex(e => e.TokenHash).IsUnique();
                e.Property(e => e.DeletedAt).HasDefaultValue(null);
            });

            builder.Property(e => e.DeletedAt).HasDefaultValue(null);

            builder.HasQueryFilter(e => e.DeletedAt == null);
        }
    }
}
