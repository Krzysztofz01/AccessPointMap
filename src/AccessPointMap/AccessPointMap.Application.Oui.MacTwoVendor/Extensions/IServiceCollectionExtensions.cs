using AccessPointMap.Application.Oui.Core;
using AccessPointMap.Application.Oui.MacToVendor;
using AccessPointMap.Application.Oui.MacToVendor.Database;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace AccessPointMap.Application.Oui.MacTwoVendor.Extensions
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection AddMacTwoVendorOuiLookup(this IServiceCollection services)
        {
            var builder = new SqliteConnectionStringBuilder("Data Source=oui-database.sqlite");
            builder.DataSource = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, builder.DataSource);

            services.AddDbContext<MacTwoVendorDbContext>(options =>
                options.UseSqlite(builder.ToString()));
            
            services.AddScoped<IOuiLookupService, MacTwoVendorOuiLookupService>();

            return services;
        }
    }
}
