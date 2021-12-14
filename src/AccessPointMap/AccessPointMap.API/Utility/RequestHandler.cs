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

        public static async Task<IActionResult> MapQuery<TResponse, TResponseDto>(Task<TResponse> query, IMapper mapper)
        {
            var result = await query;

            if (result is null) return new NotFoundResult();

            var mappedResult = mapper.Map<TResponseDto>(result);

            return new OkObjectResult(mappedResult);
        }

        public static async Task<IActionResult> MapCachedQuery<TResponse, TResponseDto>(Task<TResponse> query, IMapper mapper, string endpoint, IMemoryCache memoryCache)
        {
            memoryCache.TryGetValue(endpoint, out string response);

            if (!string.IsNullOrEmpty(response)) return new OkObjectResult(response);

            var result = await query;

            var cacheOptions = new MemoryCacheEntryOptions
            {
                AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                Priority = CacheItemPriority.Normal,
                SlidingExpiration = TimeSpan.FromMinutes(2),
                Size = 4096
            };

            if (result is null)
            {
                memoryCache.Set(endpoint, result, cacheOptions);

                return new NotFoundResult();
            }

            var mappedResult = mapper.Map<TResponseDto>(result);

            memoryCache.Set(endpoint, mappedResult, cacheOptions);

            return new OkObjectResult(mappedResult);
        }

        public static async Task<IActionResult> IntegrationServiceCommand<TRequest>(TRequest request, Func<TRequest, Task> serviceMethod)
        {
            await serviceMethod(request);

            return new OkResult();
        }
    }
}
