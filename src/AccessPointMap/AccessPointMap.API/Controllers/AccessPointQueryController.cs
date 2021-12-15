using AccessPointMap.API.Utility;
using AccessPointMap.Application.AccessPoints;
using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Infrastructure.Core.Abstraction;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AccessPointMap.Application.AccessPoints.Dto;

namespace AccessPointMap.API.Controllers
{
    [ApiController]
    [Route("api/v{version:apiVersion}/accesspoints")]
    [ApiVersion("1.0")]
    [Authorize]
    public class AccessPointQueryController : ControllerBase
    {
        private readonly IDataAccess _dataAccess;
        private readonly IMapper _mapper;

        public AccessPointQueryController(IDataAccess dataAccess, IMapper mapper)
        {
            _dataAccess = dataAccess ??
                throw new ArgumentNullException(nameof(dataAccess));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            await RequestHandler.MapQuery<IEnumerable<AccessPoint>, IEnumerable<AccessPointSimple>>(_dataAccess.AccessPoints.GetAllAccessPoints(), _mapper);

        [HttpGet("full")]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> GetAllFull() =>
            await RequestHandler.MapQuery<IEnumerable<AccessPoint>, IEnumerable<AccessPointSimple>>(_dataAccess.AccessPoints.GetAllAccessPointsAdministration(), _mapper);

        [HttpGet("{accessPointId}")]
        public async Task<IActionResult> GetById(Guid accessPointId) =>
            await RequestHandler.MapQuery<AccessPoint, AccessPointDetails>(_dataAccess.AccessPoints.GetAccessPointById(accessPointId), _mapper);

        [HttpGet("{accessPointId}/full")]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> GetByIdFull(Guid accessPointId) =>
            await RequestHandler.MapQuery<AccessPoint, AccessPointDetailsAdministration>(_dataAccess.AccessPoints.GetAccessPointByIdAdministration(accessPointId), _mapper);

        [HttpGet("search/{keyword}")]
        public async Task<IActionResult> GetByKeyword(string keyword) =>
            await RequestHandler.MapQuery<IEnumerable<AccessPoint>, IEnumerable<AccessPointSimple>>(_dataAccess.AccessPoints.SearchByKeyword(keyword), _mapper);

        [HttpGet("statistics/signal")]
        public async Task<IActionResult> GetStatisticsAccessPointWithGreaterSignalRange([FromQuery] int? limit) =>
            throw new NotImplementedException();

        [HttpGet("statistics/frequency")]
        public async Task<IActionResult> GetStatisticsMostCommonUsedFrequency([FromQuery] int? limit) =>
            throw new NotImplementedException();

        [HttpGet("statistics/manufacturer")]
        public async Task<IActionResult> GetStatisticsMostCommonUsedManufacturer([FromQuery] int? limit) =>
            throw new NotImplementedException();

        [HttpGet("statistics/encryption")]
        public async Task<IActionResult> GetStatisticsMostCommonUsedEncryption([FromQuery] int? limit) =>
            throw new NotImplementedException();
    }
}
