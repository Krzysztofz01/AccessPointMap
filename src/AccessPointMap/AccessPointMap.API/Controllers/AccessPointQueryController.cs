using AccessPointMap.API.Controllers.Base;
using AccessPointMap.Application.AccessPoints;
using AccessPointMap.Application.Kml.Core;
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
        private readonly IKmlParsingService _kmlParsingService;

        private const int _defaultLimit = 100;
        private const string _kmlContentType = "text/kml";

        public AccessPointQueryController(IDataAccess dataAccess, IMapper mapper, IMemoryCache memoryCache, IKmlParsingService kmlParsingService) : base(dataAccess, mapper, memoryCache)
        {
            _kmlParsingService = kmlParsingService ??
                throw new ArgumentNullException(nameof(kmlParsingService));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery]DateTime? startingData,
            [FromQuery]DateTime? endingDate,
            [FromQuery]double? latitude,
            [FromQuery]double? longitude,
            [FromQuery]double? distance,
            [FromQuery]string keyword,
            [FromQuery]int? page,
            [FromQuery]int? pageSize)
        {
            var cachedResponse = ResolveFromCache();
            if (cachedResponse is not null) return Ok(cachedResponse);

            var response = await _dataAccess.AccessPoints.GetAllAccessPoints(startingData, endingDate, latitude, longitude, distance, keyword, page, pageSize);

            var mappedResponse = MapToDto<IEnumerable<AccessPointSimple>>(response);

            StoreToCache(mappedResponse);

            return Ok(mappedResponse);
        }

        [HttpGet("full")]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> GetAllFull(
            [FromQuery] DateTime? startingData,
            [FromQuery] DateTime? endingDate,
            [FromQuery] double? latitude,
            [FromQuery] double? longitude,
            [FromQuery] double? distance,
            [FromQuery] string keyword,
            [FromQuery] int? page,
            [FromQuery] int? pageSize)
        {
            var response = await _dataAccess.AccessPoints.GetAllAccessPointsAdministration(startingData, endingDate, latitude, longitude, distance, keyword, page, pageSize);

            var mappedResponse = MapToDto<IEnumerable<AccessPointSimple>>(response);

            return Ok(mappedResponse);
        }

        [HttpGet("kml")]
        public async Task<IActionResult> GetAllInKml()
        {
            var response = await _kmlParsingService.GenerateKml(options => options.IncludeHiddenAccessPoints = false);

            return MapToFile(response.FileBuffer, _kmlContentType);
        }

        [HttpGet("kml/full")]
        public async Task<IActionResult> GetAllInKmlFull()
        {
            var response = await _kmlParsingService.GenerateKml(options => options.IncludeHiddenAccessPoints = true);

            return MapToFile(response.FileBuffer, _kmlContentType);
        }

        [HttpGet("run/{runId}")]
        public async Task<IActionResult> GetAllByRunId(Guid runId)
        {
            var response = await _dataAccess.AccessPoints.GetAllAccessPointsByRunId(runId);

            var mappedResponse = MapToDto<IEnumerable<AccessPointSimple>>(response);

            return Ok(mappedResponse);
        }

        [HttpGet("run/{runId}/full")]
        [Authorize(Roles = "Admin, Support")]
        public async Task<IActionResult> GetAllByRunIdFull(Guid runId)
        {
            var response = await _dataAccess.AccessPoints.GetAllAccessPointsByRunIdAdministration(runId);

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
