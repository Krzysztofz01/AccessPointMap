using System;
using System.Linq;
using System.Linq.Expressions;

namespace AccessPointMap.Application.Extensions
{
    internal static class IQueryableExtensions
    {
        public static IQueryable<TEntity> WhereParamPresent<TEntity>(this IQueryable<TEntity> entities, bool paramPresent, Expression<Func<TEntity, bool>> predicate) where TEntity : class
        {
            return paramPresent
                ? entities.Where(predicate)
                : entities;
        }
    }
}
