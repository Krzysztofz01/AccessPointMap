using AccessPointMap.Infrastructure.Core.Abstraction;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace AccessPointMap.API.Controllers.Base
{
    [ApiController]
    [Authorize]
    public abstract class QueryController : ControllerBase
    {
        protected readonly IDataAccess _dataAccess;
        protected readonly IMapper _mapper;
        protected readonly IMemoryCache _memoryCache;

        public QueryController(IDataAccess dataAccess, IMapper mapper, IMemoryCache memoryCache)
        {
            _dataAccess = dataAccess ??
                throw new ArgumentNullException(nameof(dataAccess));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _memoryCache = memoryCache ??
                throw new ArgumentNullException(nameof(memoryCache));
        }

        protected object ResolveFromCache()
        {
            var key = Request.GetEncodedPathAndQuery();

            if (_memoryCache.TryGetValue(key, out object cachedResponse)) return cachedResponse;

            return null;
        }

        protected void StoreToCache(object response)
        {
            var key = Request.GetEncodedPathAndQuery();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(15))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                .SetPriority(CacheItemPriority.High);

            _memoryCache.Set(key, response, cacheOptions);
        }

        protected object MapToDto<TDto>(object response) where TDto : class
        {
            return _mapper.Map<TDto>(response);
        }
    }
}
