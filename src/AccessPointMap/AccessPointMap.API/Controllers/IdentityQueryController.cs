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
using System.Threading;

namespace AccessPointMap.API.Controllers
{
    [Route("api/v{version:apiVersion}/identities")]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin, Support")]
    public class IdentityQueryController : QueryController<IdentityQueryController>
    {
        public IdentityQueryController(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache memoryCache, ILogger<IdentityQueryController> logger) : base(unitOfWork, mapper, memoryCache, logger)
        {
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<IdentitySimple>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAll(
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.IdentityRepository.GetAllIdentities(cancellationToken);

            return await HandleQueryResult(query, true, typeof(IEnumerable<IdentitySimple>), cancellationToken);
        }

        [HttpGet("{identityId}")]
        [ProducesResponseType(typeof(IEnumerable<IdentityDetails>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(
            Guid identityId,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.IdentityRepository.GetIdentityById(identityId, cancellationToken);

            return await HandleQueryResult(query, false, typeof(IdentityDetails), cancellationToken);
        }
    }
}
