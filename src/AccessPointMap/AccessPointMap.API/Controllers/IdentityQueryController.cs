using AutoMapper;
using AccessPointMap.Application.Identities;
using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AccessPointMap.Application.Identities.Dto;
using AccessPointMap.API.Controllers.Base;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;

namespace AccessPointMap.API.Controllers
{
    [Route("api/v{version:apiVersion}/identities")]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin, Support")]
    public class IdentityQueryController : QueryController
    {
        public IdentityQueryController(IDataAccess dataAccess, IMapper mapper, IMemoryCache memoryCache, ILogger<IdentityQueryController> logger) : base(dataAccess, mapper, memoryCache, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<IdentitySimple>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll()
        {
            var cachedResponse = ResolveFromCache();
            if (cachedResponse is not null) return Ok(cachedResponse);

            var response = await _dataAccess.Identities.GetAllIdentities();

            var mappedResponse = MapToDto<IEnumerable<IdentitySimple>>(response);

            StoreToCache(mappedResponse);

            return Ok(mappedResponse);
        }

        [HttpGet("{identityId}")]
        [ProducesResponseType(typeof(IEnumerable<IdentityDetails>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid identityId)
        {
            var response = await _dataAccess.Identities.GetIdentityById(identityId);

            var mappedResponse = MapToDto<IdentityDetails>(response);

            StoreToCache(mappedResponse);

            return Ok(mappedResponse);
        }
    }
}
