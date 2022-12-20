using AccessPointMap.Application.Oui.MacToVendor.Database.Models;
using Microsoft.EntityFrameworkCore;

namespace AccessPointMap.Application.Oui.MacToVendor.Database
{
    internal sealed class MacTwoVendorDbContext : DbContext
    {
        public DbSet<Vendor> Vendors { get; set; }

        public MacTwoVendorDbContext(DbContextOptions<MacTwoVendorDbContext> options) : base(options) { }
    }
}
