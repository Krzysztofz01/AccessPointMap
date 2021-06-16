using AccessPointMap.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessPointMap.Repository.Maps
{
    public class RefreshTokenMap
    {
        public RefreshTokenMap(EntityTypeBuilder<RefreshToken> entityBuilder)
        {
            //Base
            entityBuilder.HasKey(p => p.Id);
            entityBuilder.Property(p => p.Id).HasAnnotation("MySql:ValueGeneratedOnAdd", true).ValueGeneratedOnAdd();
            entityBuilder.Property(p => p.AddDate).IsRequired().HasDefaultValueSql("getdate()");
            entityBuilder.Property(p => p.EditDate).IsRequired().HasDefaultValueSql("getdate()");
            entityBuilder.Property(p => p.DeleteDate).HasDefaultValue(null);

            //Props
            entityBuilder.Property(p => p.Token).IsRequired();
            entityBuilder.Property(p => p.Expires).IsRequired();
            entityBuilder.Property(p => p.Created).IsRequired().HasDefaultValueSql("getdate()");
            entityBuilder.Property(p => p.CreatedByIp).IsRequired().HasDefaultValue(string.Empty);
            entityBuilder.Property(p => p.RevokedByIp).IsRequired().HasDefaultValue(string.Empty);
            entityBuilder.Property(p => p.IsRevoked).IsRequired().HasDefaultValue(false);

            //Relations
            entityBuilder
                .HasOne<User>(p => p.User)
                .WithMany(p => p.RefreshTokens)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
