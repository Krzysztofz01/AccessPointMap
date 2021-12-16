using AutoMapper;
using AccessPointMap.Application.Abstraction;
using AccessPointMap.Domain.Core.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace AccessPointMap.API.Utility
{
    public static class RequestHandler
    {
        public static async Task<IActionResult> Command<TAggregateRoot>(IApplicationCommand<TAggregateRoot> command, Func<IApplicationCommand<TAggregateRoot>, Task> serviceHandler) where TAggregateRoot : AggregateRoot
        {
            await serviceHandler(command);

            return new OkResult();
        }

        public static async Task<IActionResult> ExecuteService<TRequest>(TRequest request, Func<TRequest, Task> serviceHandler)
        {
            await serviceHandler(request);

            return new OkResult();
        }

        public static async Task<TResponse> CachedQuery<TResponse>(Task<TResponse> query, string key, IMemoryCache memoryCache) where TResponse : class
        {
            if (memoryCache.TryGetValue(key, out TResponse cachedResult)) return cachedResult;

            var queriedResult = await query;

            if (queriedResult is null) return null;

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(30))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                .SetPriority(CacheItemPriority.High);

            memoryCache.Set(key, queriedResult, cacheOptions);

            return queriedResult;
        }

        public static async Task<TResponse> DirectQuery<TResponse>(Task<TResponse> query) where TResponse : class
        {
            var result = await query;

            if (result is null) return null;

            return result;
        }

        public static async Task<IActionResult> Map<TResponse, TDto>(Task<TResponse> query, IMapper mapper)
        {
            var result = await query;

            if (result is null) return new NotFoundResult();

            var mappedResult = mapper.Map<TDto>(result);

            return new OkObjectResult(mappedResult);
        }

        [Obsolete]
        public static async Task<IActionResult> MapQuery<TResponse, TResponseDto>(Task<TResponse> query, IMapper mapper)
        {
            var result = await query;

            if (result is null) return new NotFoundResult();

            var mappedResult = mapper.Map<TResponseDto>(result);

            return new OkObjectResult(mappedResult);
        }

        [Obsolete]
        public static async Task<IActionResult> IntegrationServiceCommand<TRequest>(TRequest request, Func<TRequest, Task> serviceMethod)
        {
            await serviceMethod(request);

            return new OkResult();
        }
    }
}
