using AccessPointMap.Application.Abstraction;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Extensions
{
    internal static class ResultExtensions
    {
        public static Result<T> ToResultObject<T>(this T obj) where T : class
        {
            if (obj is null) return Result.Failure<T>(NotFoundError.Default);

            return Result.Success<T>(obj);
        }

        public static async Task<Result<T>> ToResultObjectAsync<T>(this Task<T> obj) where T : class
        {
            var objResult = await obj;
            return objResult.ToResultObject();
        }

        public static Result<IEnumerable<TEntity>> ToResultObject<TEntity>(this IEnumerable<TEntity> entities) where TEntity : class
        {
            if (!entities.Any()) Result.Failure<IEnumerable<TEntity>>(NotFoundError.Default);

            return Result.Success<IEnumerable<TEntity>>(entities);
        }

        public static Result<IEnumerable<TEntity>> ToResultObject<TEntity>(this List<TEntity> entities) where TEntity : class
        {
            if (entities.Count == 0) Result.Failure<IEnumerable<TEntity>>(NotFoundError.Default);

            return Result.Success<IEnumerable<TEntity>>(entities);
        }

        public static async Task<Result<IEnumerable<TEntity>>> ToResultObjectAsync<TEntity>(this Task<IEnumerable<TEntity>> entities) where TEntity : class
        {
            var entitiesResult = await entities;
            return entitiesResult.ToResultObject();
        }

        public static async Task<Result<IEnumerable<TEntity>>> ToResultObjectAsync<TEntity>(this Task<List<TEntity>> entities) where TEntity : class
        {
            var entitiesResult = await entities;
            return entitiesResult.ToResultObject();
        }
    }
}
