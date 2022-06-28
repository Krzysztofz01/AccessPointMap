using AccessPointMap.API.Controllers.Base;
using AccessPointMap.Application.AccessPoints;
using AccessPointMap.Infrastructure.Core.Abstraction;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static AccessPointMap.Application.AccessPoints.Dto;

namespace AccessPointMap.API.Controllers
{
    [Route("api/v{version:apiVersion}/accesspoints")]
    [ApiVersion("1.0")]
    public class AccessPointQueryController : QueryController
    {
        private const int _defaultLimit = 100;

        public AccessPointQueryController(IDataAccess dataAccess, IMapper mapper, IMemoryCache memoryCache) : base(dataAccess, mapper, memoryCache)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var cachedResponse = ResolveFromCache();
            if (cachedResponse is not null) return Ok(cachedResponse);

            var response = await _dataAccess.AccessPoints.GetAllAccessPoints();

            var mappedResponse = MapToDto<IEnumerable<AccessPointSimple>>(response);

            StoreToCache(mappedResponse);

            return Ok(mappedResponse);
        }

        [HttpGet("full")]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> GetAllFull()
        {
            var response = await _dataAccess.AccessPoints.GetAllAccessPointsAdministration();

            var mappedResponse = MapToDto<IEnumerable<AccessPointSimple>>(response);

            return Ok(mappedResponse);
        }

        [HttpGet("{accessPointId}")]
        public async Task<IActionResult> GetById(Guid accessPointId)
        {
            var cachedResponse = ResolveFromCache();
            if (cachedResponse is not null) return Ok(cachedResponse);

            var response = await _dataAccess.AccessPoints.GetAccessPointById(accessPointId);

            var mappedResponse = MapToDto<AccessPointDetails>(response);

            StoreToCache(mappedResponse);

            return Ok(mappedResponse);
        }

        [HttpGet("{accessPointId}/full")]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> GetByIdFull(Guid accessPointId)
        {
            var response = await _dataAccess.AccessPoints.GetAccessPointByIdAdministration(accessPointId);

            var mappedResponse = MapToDto<AccessPointDetailsAdministration>(response);

            return Ok(mappedResponse);
        }

        [HttpGet("{accessPointId}/packet")]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> GetAllPackets(Guid accessPointId)
        {
            var response = await _dataAccess.AccessPoints.GetAllAccessPointsAccessPointPackets(accessPointId);

            var mappedResponse = MapToDto<IEnumerable<AccessPointPacketDetails>>(response);

            return Ok(mappedResponse);
        }

        [HttpGet("{accessPointId}/packet/{accessPointPacketId}")]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> GetPacketById(Guid accessPointId, Guid accessPointPacketId)
        {
            var response = await _dataAccess.AccessPoints.GetAccessPointsAccessPointPacketById(accessPointId, accessPointPacketId);

            var mappedResponse = MapToDto<AccessPointPacketDetails>(response);

            return Ok(mappedResponse);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetByKeyword([FromQuery] string keyword)
        {
            var response = await _dataAccess.AccessPoints.SearchByKeyword(keyword);

            var mappedResponse = MapToDto<IEnumerable<AccessPointSimple>>(response);

            return Ok(mappedResponse);
        }
            
        [HttpGet("statistics/signal")]
        public async Task<IActionResult> GetStatisticsAccessPointWithGreaterSignalRange([FromQuery] int? limit)
        {
            var cachedResponse = ResolveFromCache();
            if (cachedResponse is not null) return Ok(cachedResponse);

            var response = await _dataAccess.AccessPoints
                .GetAccessPointsWithGreatestSignalRange(limit ?? _defaultLimit);

            var mappedResponse = MapToDto<IEnumerable<AccessPointSimple>>(response);

            StoreToCache(mappedResponse);

            return Ok(mappedResponse);
        }

        [HttpGet("statistics/frequency")]
        public async Task<IActionResult> GetStatisticsMostCommonUsedFrequency([FromQuery] int? limit)
        {
            var cachedResponse = ResolveFromCache();
            if (cachedResponse is not null) return Ok(cachedResponse);

            var response = await _dataAccess.AccessPoints
                .GetMostCommonUsedFrequency(limit ?? _defaultLimit);

            StoreToCache(response);

            return Ok(response);
        }

        [HttpGet("statistics/manufacturer")]
        public async Task<IActionResult> GetStatisticsMostCommonUsedManufacturer([FromQuery] int? limit)
        {
            var cachedResponse = ResolveFromCache();
            if (cachedResponse is not null) return Ok(cachedResponse);

            var response = await _dataAccess.AccessPoints
                .GetMostCommonUsedManufacturer(limit ?? _defaultLimit);

            StoreToCache(response);

            return Ok(response);
        }

        [HttpGet("statistics/encryption")]
        public async Task<IActionResult> GetStatisticsMostCommonUsedEncryption([FromQuery] int? limit)
        {
            var cachedResponse = ResolveFromCache();
            if (cachedResponse is not null) return Ok(cachedResponse);

            var response = await _dataAccess.AccessPoints
                .GetMostCommonUsedEncryptionTypes(limit ?? _defaultLimit);

            StoreToCache(response);

            return Ok(response);
        }
    }
}
