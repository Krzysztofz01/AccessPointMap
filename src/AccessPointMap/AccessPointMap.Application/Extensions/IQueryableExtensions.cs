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

        public static IQueryable<TEntity> Paginate<TEntity>(this IQueryable<TEntity> entities, int? page, int? elementsPerPage, bool skipIfParamsNotProvided = true)
        {
            if (!skipIfParamsNotProvided && (!page.HasValue || !elementsPerPage.HasValue)) return entities;

            if (page.Value <= 0) throw new ArgumentOutOfRangeException(nameof(page));

            return entities
                .Skip(page.Value * elementsPerPage.Value)
                .Take(elementsPerPage.Value);
        }
    }
}
