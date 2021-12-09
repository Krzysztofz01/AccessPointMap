using AccessPointMap.Domain.Identities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessPointMap.Infrastructure.MySql.Builders
{
    public class IdentityTypeBuilder
    {
        public IdentityTypeBuilder(EntityTypeBuilder<Identity> builder)
        {
            builder.HasKey(e => e.Id);
            builder.Property(e => e.Id).ValueGeneratedNever();
            builder.OwnsOne(e => e.Name).Property(v => v.Value).HasMaxLength(40);
            builder.OwnsOne(e => e.Email).HasIndex(v => v.Value).IsUnique();
            builder.OwnsOne(e => e.PasswordHash);
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
