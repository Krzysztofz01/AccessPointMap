using AccessPointMap.Application.Core;
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
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
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
    public abstract class QueryController<TController> : ControllerBase where TController : QueryController<TController>
    {
        protected readonly IUnitOfWork _unitOfWork;
        protected readonly IMapper _mapper;
        protected readonly IMemoryCache _memoryCache;
        protected readonly ILogger<TController> _logger;

        public QueryController(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache memoryCache, ILogger<TController> logger)
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
            LogCurrentScope();

            if (useCaching)
            {
                if (_memoryCache.TryGetValue(GetCacheEntryKey(false), out object cachedValue))
                    return new OkObjectResult(cachedValue);
            }

            var result = await queryResultTask;

            if (result.IsFailure) return GetFailureResponse(result.Error);

            if (useCaching) StoreToMemoryCache(result.Value, cancellationToken);

            return new OkObjectResult(result.Value);
        }

        protected async Task<IActionResult> HandleQueryResult<T>(Task<Result<T>> queryResultTask, bool useCaching, Type mappingType, CancellationToken cancellationToken = default) where T : class
        {
            LogCurrentScope();

            if (useCaching)
            {
                if (_memoryCache.TryGetValue(GetCacheEntryKey(true), out object cachedValue))
                    return new OkObjectResult(cachedValue);
            }

            var result = await queryResultTask;

            if (result.IsFailure) return GetFailureResponse(result.Error);

            var mappedResultValue = _mapper.Map(result.Value, result.Value.GetType(), mappingType);

            if (useCaching) StoreToMemoryCache(mappedResultValue, cancellationToken);

            return new OkObjectResult(mappedResultValue);
        }

        protected async Task<IActionResult> HandleFileResult(ExportFile file, bool useCaching, string mimeType, CancellationToken cancellationToken = default)
        {
            LogCurrentScope();

            if (string.IsNullOrEmpty(mimeType.Trim()))
                throw new ArgumentException("Invalid mime type specified.", nameof(mimeType));

            Response.ContentType = new MediaTypeHeaderValue(mimeType).ToString();

            if (useCaching)
            {
                if (_memoryCache.TryGetValue(GetCacheEntryKey(false), out object cachedFileBuffer))
                {
                    return await Task.FromResult(File((byte[])cachedFileBuffer, mimeType));
                }

                StoreToMemoryCache(file.FileBuffer, cancellationToken);
            }

            return await Task.FromResult(File(file.FileBuffer, mimeType));
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

        private void StoreToMemoryCache<T>(T value, CancellationToken cancellationToken)
        {
            var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(15))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2))
                    .SetPriority(CacheItemPriority.High);

            cancellationToken.ThrowIfCancellationRequested();
            _memoryCache.Set(GetCacheEntryKey(false), value, cacheOptions);
        }

        protected IActionResult GetFailureResponse(Error error)
        {
            var problemDetails = ProblemDetailsFactory.CreateProblemDetails(HttpContext);
            problemDetails.Title = error.Message;
            problemDetails.Detail = null;

            if (error is NotFoundError)
            {
                problemDetails.Status = (int)HttpStatusCode.NotFound;
                return NotFound(problemDetails);
            }

            problemDetails.Status = (int)HttpStatusCode.BadRequest;
            return BadRequest(problemDetails);
        }

        private void LogCurrentScope()
        {
            string currentPath = Request.GetEncodedPathAndQuery() ?? string.Empty;
            string currentHost = Request.GetIpAddressString() ?? string.Empty;
            string currentIdentityId = Request.HttpContext.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
                "Anonymous";

            _logger.LogQueryController(currentPath, currentIdentityId, currentHost);
        }
    }
}
