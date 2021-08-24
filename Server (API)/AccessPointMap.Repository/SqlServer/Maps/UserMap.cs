using AccessPointMap.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;

namespace AccessPointMap.Repository.SqlServer.Maps
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
            entityBuilder.Property(p => p.LastLoginDate).IsRequired().HasDefaultValueSql("getdate()");
            entityBuilder.Property(p => p.LastLoginIp).IsRequired().HasDefaultValue(string.Empty);
            entityBuilder.Property(p => p.AdminPermission).IsRequired().HasDefaultValue(false);
            entityBuilder.Property(p => p.ModPermission).IsRequired().HasDefaultValue(false);
            entityBuilder.Property(p => p.IsActivated).IsRequired().HasDefaultValue(false);

            //Query filter
            entityBuilder.HasQueryFilter(e => e.DeleteDate == null);

            //Default data
            entityBuilder.HasData(new User[]
            {
                new User()
                {
                    Id = 1,
                    AddDate = DateTime.Now,
                    EditDate = DateTime.Now,
                    Name = "Administrator",
                    Email = "admin@apm.com",
                    Password = "$05$feN415S/rRMOaPcaiobkEeo5JTPoxY7PPMCwVGkbrbItw/mj19CBS",
                    LastLoginIp = string.Empty,
                    LastLoginDate = DateTime.Now,
                    AdminPermission = true,
                    ModPermission = false,
                    IsActivated =  true,
                    DeleteDate = null
                }
            });
        }
    }
}
