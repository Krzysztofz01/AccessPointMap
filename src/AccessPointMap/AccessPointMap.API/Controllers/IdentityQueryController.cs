using AutoMapper;
using AccessPointMap.API.Utility;
using AccessPointMap.Application.Identities;
using AccessPointMap.Domain.Identities;
using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AccessPointMap.Application.Identities.Dto;

namespace AccessPointMap.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/identities")]
    [ApiVersion("1.0")]
    [Authorize(Roles = "Admin, Support")]
    public class IdentityQueryController : ControllerBase
    {
        private readonly IDataAccess _dataAccess;
        private readonly IMapper _mapper;

        public IdentityQueryController(IDataAccess dataAccess, IMapper mapper)
        {
            _dataAccess = dataAccess ??
                throw new ArgumentNullException(nameof(dataAccess));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            await RequestHandler.MapQuery<IEnumerable<Identity>, IEnumerable<IdentitySimple>>(_dataAccess.Identities.GetAllIdentities(), _mapper);

        [HttpGet("{identityId}")]
        public async Task<IActionResult> GetById(Guid identityId) =>
            await RequestHandler.MapQuery<Identity, IdentityDetails>(_dataAccess.Identities.GetIdentityById(identityId), _mapper);
    }
}
