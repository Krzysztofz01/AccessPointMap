using AccessPointMap.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AccessPointMap.Repository.MySql.Maps
{
    public class UserMap
    {
        public UserMap(EntityTypeBuilder<User> entityBuilder)
        {
            //Base
            entityBuilder.HasKey(p => p.Id);
            entityBuilder.Property(p => p.AddDate).IsRequired();
            entityBuilder.Property(p => p.EditDate).IsRequired();
            entityBuilder.Property(p => p.DeleteDate).HasDefaultValue(null);

            //Props
            entityBuilder.Property(p => p.Name).IsRequired();
            entityBuilder.HasIndex(p => p.Email).IsUnique();
            entityBuilder.Property(p => p.Password).IsRequired();
            entityBuilder.Property(p => p.LastLoginDate).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
            entityBuilder.Property(p => p.LastLoginIp).HasDefaultValue(string.Empty);
            entityBuilder.Property(p => p.AdminPermission).IsRequired().HasDefaultValue(false);
            entityBuilder.Property(p => p.ModPermission).IsRequired().HasDefaultValue(false);
            entityBuilder.Property(p => p.IsActivated).IsRequired().HasDefaultValue(false);

            //Query filter
            entityBuilder.HasQueryFilter(e => e.DeleteDate == null);
        }
    }
}
