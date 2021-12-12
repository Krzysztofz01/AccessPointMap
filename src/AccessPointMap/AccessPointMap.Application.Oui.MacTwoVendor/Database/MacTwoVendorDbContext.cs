using AccessPointMap.Application.Oui.MacToVendor.Database.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;

namespace AccessPointMap.Application.Oui.MacToVendor.Database
{
    public class MacTwoVendorDbContext : DbContext
    {
        private readonly string _databasePath;
        private readonly string _databaseFileName = "oui-database.sqlite";

        public DbSet<Vendor> Vendors { get; set; }

        public MacTwoVendorDbContext()
        {
            var assemblyPath = AppDomain.CurrentDomain.BaseDirectory;

            _databasePath = Path.Combine(assemblyPath, _databaseFileName);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options) =>
            options.UseSqlite($"Data Source={_databasePath}");
    }
}
