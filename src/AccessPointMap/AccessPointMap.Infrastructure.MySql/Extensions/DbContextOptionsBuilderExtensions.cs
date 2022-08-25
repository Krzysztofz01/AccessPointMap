using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace AccessPointMap.Infrastructure.MySql.Extensions
{
    public static class DbContextOptionsBuilderExtensions
    {
        public static DbContextOptionsBuilder UseMySql(this DbContextOptionsBuilder builder, string connectionString)
        {
            return builder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), options =>
            {
                options.SchemaBehavior(MySqlSchemaBehavior.Translate, (schema, table) => $"{schema}_{table}");

                options.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
            });
        }
    }
}
