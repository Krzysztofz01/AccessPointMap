using AccessPointMap.Application.Abstraction;
using AccessPointMap.Application.Extensions;
using AccessPointMap.Application.Logging;
using AccessPointMap.Infrastructure.Core.Abstraction;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.API.Controllers.Base
{
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public abstract class QueryController : ControllerBase
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        protected readonly IMemoryCache _memoryCache;
        protected readonly ILogger<QueryController> _logger;

        public QueryController(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache memoryCache, ILogger<QueryController> logger)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _memoryCache = memoryCache ??
                throw new ArgumentNullException(nameof(memoryCache));

            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        protected async Task<IActionResult> HandleQueryResult<T>(Task<Result<T>> queryResultTask, bool useCaching, CancellationToken cancellationToken = default) where T : class
        {
            if (useCaching)
            {
                if (_memoryCache.TryGetValue(GetCacheEntryKey(false), out object cachedValue))
                    return new OkObjectResult(cachedValue);
            }

            var result = await queryResultTask;

            // TODO: Pass error message
            if (result.IsFailure)
            {
                if (result.Error is NotFoundError)
                    return new NotFoundResult();

                return new BadRequestResult();
            }

            if (useCaching)
            {
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(15))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                    .SetPriority(CacheItemPriority.High);

                cancellationToken.ThrowIfCancellationRequested();
                _memoryCache.Set(GetCacheEntryKey(false), result.Value, cacheOptions);
            }

            return new OkObjectResult(result.Value);
        }

        protected async Task<IActionResult> HandleQueryResult<T>(Task<Result<T>> queryResultTask, bool useCaching, Type mappingType, CancellationToken cancellationToken = default) where T : class
        {
            if (useCaching)
            {
                if (_memoryCache.TryGetValue(GetCacheEntryKey(true), out object cachedValue))
                    return new OkObjectResult(cachedValue);
            }

            var result = await queryResultTask;

            // TODO: Pass error message
            if (result.IsFailure)
            {
                if (result.Error is NotFoundError)
                    return new NotFoundResult();

                return new BadRequestResult();
            }

            var mappedResultValue = _mapper.Map(result.Value, mappingType);

            if (useCaching)
            {
                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(15))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                    .SetPriority(CacheItemPriority.High);

                cancellationToken.ThrowIfCancellationRequested();
                _memoryCache.Set(GetCacheEntryKey(true), mappedResultValue, cacheOptions);
            }

            return new OkObjectResult(mappedResultValue);
        }

        protected async Task<IActionResult> HandleFileResult(byte[] fileBuffer, bool useCaching, string mimeType, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(mimeType.Trim()))
                throw new ArgumentException("Invalid mime type specified.", nameof(mimeType));

            Response.ContentType = new MediaTypeHeaderValue(mimeType).ToString();

            if (useCaching)
            {
                if (_memoryCache.TryGetValue(GetCacheEntryKey(false), out object cachedFileBuffer))
                {
                    return await Task.FromResult(File((byte[])cachedFileBuffer, mimeType));
                }

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(15))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                    .SetPriority(CacheItemPriority.High);

                cancellationToken.ThrowIfCancellationRequested();
                _memoryCache.Set(GetCacheEntryKey(false), fileBuffer, cacheOptions);
            }

            return await Task.FromResult(File(fileBuffer, mimeType));
        }

        protected int NormalizePaginationLimit(int? limit)
        {
            if (!limit.HasValue) return default;

            return Math.Max(limit.Value, default);
        }

        private string GetCacheEntryKey(bool retrieveMapped)
        {
            return retrieveMapped
                ? $"cache-mapped-{Request.GetEncodedPathAndQuery()}"
                : $"cache-{Request.GetEncodedPathAndQuery()}";
        }

        [Obsolete("Use the overload with the CancellationToken")]
        protected object ResolveFromCache()
        {
            var key = Request.GetEncodedPathAndQuery();

            if (_memoryCache.TryGetValue(key, out object cachedResponse)) return cachedResponse;

            return null;
        }

        [Obsolete("Use the overload with the CancellationToken")]
        protected void StoreToCache(object response)
        {
            var key = Request.GetEncodedPathAndQuery();

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(DateTime.Now.AddMinutes(15))
                .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                .SetPriority(CacheItemPriority.High);

            _memoryCache.Set(key, response, cacheOptions);
        }

        [Obsolete("Use the overload with the CancellationToken")]
        protected object MapToDto<TDto>(object response) where TDto : class
        {
            return _mapper.Map<TDto>(response);
        }

        [Obsolete("Use the overload with the CancellationToken")]
        protected FileStreamResult MapToFile(Stream fileStream, string mimeType)
        {
            if (fileStream is null || string.IsNullOrEmpty(mimeType)) return null;

            Response.ContentType = new MediaTypeHeaderValue(mimeType).ToString();
            return File(fileStream, mimeType);
        }

        [Obsolete("Use the overload with the CancellationToken")]
        protected FileStreamResult MapToFile(byte[] fileBuffer, string mimeType)
        {
            return MapToFile(new MemoryStream(fileBuffer), mimeType);
        }

        [Obsolete("Use the overload with the CancellationToken")]
        public override OkObjectResult Ok([ActionResultObjectValue] object value)
        {
            _logger.LogQueryController(Request.GetEncodedPathAndQuery(), Request.GetIpAddressString());

            return base.Ok(value);
        }
    }
}
