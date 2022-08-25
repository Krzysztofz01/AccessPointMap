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
using System.IO;
using System.Net.Http.Headers;

namespace AccessPointMap.API.Controllers.Base
{
    [ApiController]
    [Produces("application/json")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public abstract class QueryController : ControllerBase
    {
        protected readonly IDataAccess _dataAccess;
        protected readonly IMapper _mapper;
        protected readonly IMemoryCache _memoryCache;
        protected readonly ILogger<QueryController> _logger;

        public QueryController(IDataAccess dataAccess, IMapper mapper, IMemoryCache memoryCache, ILogger<QueryController> logger)
        {
            _dataAccess = dataAccess ??
                throw new ArgumentNullException(nameof(dataAccess));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));

            _memoryCache = memoryCache ??
                throw new ArgumentNullException(nameof(memoryCache));

            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
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

        protected FileStreamResult MapToFile(Stream fileStream, string mimeType)
        {
            if (fileStream is null || string.IsNullOrEmpty(mimeType)) return null;

            Response.ContentType = new MediaTypeHeaderValue(mimeType).ToString();
            return File(fileStream, mimeType);
        }

        protected FileStreamResult MapToFile(byte[] fileBuffer, string mimeType)
        {
            return MapToFile(new MemoryStream(fileBuffer), mimeType);
        }

        public override OkObjectResult Ok([ActionResultObjectValue] object value)
        {
            _logger.LogQueryController(Request.GetEncodedPathAndQuery(), Request.GetIpAddressString());

            return base.Ok(value);
        }
    }
}
