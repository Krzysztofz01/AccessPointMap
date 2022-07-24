using System;
using System.Collections.Generic;
using System.Linq;

namespace AccessPointMap.Application.Extensions
{
    internal static class IEnumerableExtensions
    {
        public static IEnumerable<TEntity> WhereParamPresent<TEntity>(this IEnumerable<TEntity> entities, bool paramPresent, Func<TEntity, bool> predicate) where TEntity : class
        {
            return paramPresent
                ? entities.Where(predicate)
                : entities;
        }

        public static IEnumerable<TEntity> Paginate<TEntity>(this IEnumerable<TEntity> entities, int? page, int? elementsPerPage, bool skipIfParamsNotProvided = true)
        {
            if (skipIfParamsNotProvided && (!page.HasValue || !elementsPerPage.HasValue)) return entities;

            if (page.Value <= 0) throw new ArgumentOutOfRangeException(nameof(page));

            return entities
                .Skip((page.Value - 1) * elementsPerPage.Value)
                .Take(elementsPerPage.Value);
        }
    }
}
