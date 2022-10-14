using AccessPointMap.API.Controllers.Base;
using AccessPointMap.Application.AccessPoints;
using AccessPointMap.Application.Kml.Core;
using AccessPointMap.Infrastructure.Core.Abstraction;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public AccessPointQueryController(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IMemoryCache memoryCache,
            ILogger<AccessPointQueryController> logger,
            IKmlParsingService kmlParsingService) : base(unitOfWork, mapper, memoryCache, logger)
        {
            _kmlParsingService = kmlParsingService ??
                throw new ArgumentNullException(nameof(kmlParsingService));
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<AccessPointSimple>), StatusCodes.Status200OK)]
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

            var response = await _unitOfWork.AccessPointRepository.GetAllAccessPoints(startingData, endingDate, latitude, longitude, distance, keyword, page, pageSize);

            var mappedResponse = MapToDto<IEnumerable<AccessPointSimple>>(response);

            StoreToCache(mappedResponse);

            return Ok(mappedResponse);
        }

        [HttpGet("full")]
        [Authorize(Roles = "Admin, Support")]
        [ProducesResponseType(typeof(IEnumerable<AccessPointSimple>), StatusCodes.Status200OK)]
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
            var response = await _unitOfWork.AccessPointRepository.GetAllAccessPointsAdministration(startingData, endingDate, latitude, longitude, distance, keyword, page, pageSize);

            var mappedResponse = MapToDto<IEnumerable<AccessPointSimple>>(response);

            return Ok(mappedResponse);
        }

        [HttpGet("kml")]
        [Produces(_kmlContentType)]
        public async Task<IActionResult> GetAllInKml()
        {
            // TODO: Pass CancellationToken to the method
            // TODO: Implement klm specific query
            var response = await _kmlParsingService.GenerateKmlAsync(_dataAccess.AccessPoints.Where(a => a.DisplayStatus.Value));

            // TODO: Null check?
            return MapToFile(response, _kmlContentType);
        }

        [HttpGet("kml/full")]
        [Authorize(Roles = "Admin, Support")]
        [Produces(_kmlContentType)]
        public async Task<IActionResult> GetAllInKmlFull()
        {
            // TODO: Pass CancellationToken to the method
            // TODO: Implement klm specific query
            var response = await _kmlParsingService.GenerateKmlAsync(_dataAccess.AccessPoints);

            // TODO: Null check?
            return MapToFile(response, _kmlContentType);
        }

        [HttpGet("run")]
        [ProducesResponseType(typeof(IEnumerable<Guid>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllRunIds()
        {
            var response = await _unitOfWork.AccessPointRepository.GetAllAccessPointRunIds();

            return Ok(response);
        }

        [HttpGet("run/full")]
        [Authorize(Roles = "Admin, Support")]
        [ProducesResponseType(typeof(IEnumerable<Guid>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllRunIdsAdministration()
        {
            var response = await _unitOfWork.AccessPointRepository.GetAllAccessPointRunIdsAdministration();

            return Ok(response);
        }

        [HttpGet("run/{runId}")]
        [ProducesResponseType(typeof(IEnumerable<AccessPointSimple>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllByRunId(Guid runId)
        {
            var cachedResponse = ResolveFromCache();
            if (cachedResponse is not null) return Ok(cachedResponse);

            var response = await _unitOfWork.AccessPointRepository.GetAllAccessPointsByRunId(runId);

            var mappedResponse = MapToDto<IEnumerable<AccessPointSimple>>(response);

            StoreToCache(mappedResponse);

            return Ok(mappedResponse);
        }

        [HttpGet("run/{runId}/full")]
        [Authorize(Roles = "Admin, Support")]
        [ProducesResponseType(typeof(IEnumerable<AccessPointSimple>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllByRunIdFull(Guid runId)
        {
            var response = await _unitOfWork.AccessPointRepository.GetAllAccessPointsByRunIdAdministration(runId);

            var mappedResponse = MapToDto<IEnumerable<AccessPointSimple>>(response);

            return Ok(mappedResponse);
        }

        [HttpGet("stamps/run/{runId}")]
        [ProducesResponseType(typeof(IEnumerable<AccessPointStampSimple>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllStampsBysRunId(Guid runId)
        {
            var response = await _unitOfWork.AccessPointRepository.GetAllAccessPointStampsByRunId(runId);

            var mappedResponse = MapToDto<IEnumerable<AccessPointStampSimple>>(response);

            return Ok(mappedResponse);
        }

        [HttpGet("stamps/run/{runId}/full")]
        [Authorize(Roles = "Admin, Support")]
        [ProducesResponseType(typeof(IEnumerable<AccessPointStampSimple>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllStampsBysRunIdAdministration(Guid runId)
        {
            var response = await _unitOfWork.AccessPointRepository.GetAllAccessPointStampsByRunIdAdministration(runId);

            var mappedResponse = MapToDto<IEnumerable<AccessPointStampSimple>>(response);

            return Ok(mappedResponse);
        }

        [HttpGet("{accessPointId}")]
        [ProducesResponseType(typeof(AccessPointDetails), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(Guid accessPointId)
        {
            var cachedResponse = ResolveFromCache();
            if (cachedResponse is not null) return Ok(cachedResponse);

            var response = await _unitOfWork.AccessPointRepository.GetAccessPointById(accessPointId);

            var mappedResponse = MapToDto<AccessPointDetails>(response);

            StoreToCache(mappedResponse);

            return Ok(mappedResponse);
        }

        [HttpGet("{accessPointId}/full")]
        [Authorize(Roles = "Admin, Support")]
        [ProducesResponseType(typeof(AccessPointDetailsAdministration), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByIdFull(Guid accessPointId)
        {
            var response = await _unitOfWork.AccessPointRepository.GetAccessPointByIdAdministration(accessPointId);

            var mappedResponse = MapToDto<AccessPointDetailsAdministration>(response);

            return Ok(mappedResponse);
        }

        [HttpGet("{accessPointId}/packet")]
        [Authorize(Roles = "Admin, Support")]
        [ProducesResponseType(typeof(IEnumerable<AccessPointPacketDetails>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPackets(Guid accessPointId)
        {
            var response = await _unitOfWork.AccessPointRepository.GetAllAccessPointsAccessPointPackets(accessPointId);

            var mappedResponse = MapToDto<IEnumerable<AccessPointPacketDetails>>(response);

            return Ok(mappedResponse);
        }

        [HttpGet("{accessPointId}/packet/{accessPointPacketId}")]
        [Authorize(Roles = "Admin, Support")]
        [ProducesResponseType(typeof(IEnumerable<AccessPointPacketDetails>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPacketById(Guid accessPointId, Guid accessPointPacketId)
        {
            var response = await _unitOfWork.AccessPointRepository.GetAccessPointsAccessPointPacketById(accessPointId, accessPointPacketId);

            var mappedResponse = MapToDto<AccessPointPacketDetails>(response);

            return Ok(mappedResponse);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<AccessPointSimple>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByKeyword([FromQuery] string keyword)
        {
            var response = await _unitOfWork.AccessPointRepository.SearchByKeyword(keyword);

            var mappedResponse = MapToDto<IEnumerable<AccessPointSimple>>(response);

            return Ok(mappedResponse);
        }

        [HttpGet("match/stamp/{accessPointStampId}")]
        [ProducesResponseType(typeof(AccessPointDetails), StatusCodes.Status200OK)]
        public async Task<IActionResult> MatchByStampId(Guid accessPointStampId)
        {
            var response = await _unitOfWork.AccessPointRepository.MatchAccessPointByAccessPointStampId(accessPointStampId);

            var mappedResponse = MapToDto<AccessPointDetails>(response);

            return Ok(mappedResponse);
        }

        [HttpGet("match/stamp/{accessPointStampId}/full")]
        [Authorize(Roles = "Admin, Support")]
        [ProducesResponseType(typeof(AccessPointDetails), StatusCodes.Status200OK)]
        public async Task<IActionResult> MatchByStampIdFull(Guid accessPointStampId)
        {
            var response = await _unitOfWork.AccessPointRepository.MatchAccessPointByAccessPointStampIdAdministration(accessPointStampId);

            var mappedResponse = MapToDto<AccessPointDetailsAdministration>(response);

            return Ok(mappedResponse);
        }

        [HttpGet("match/packet/{accessPointPacketId}")]
        [Authorize(Roles = "Admin, Support")]
        [ProducesResponseType(typeof(AccessPointDetails), StatusCodes.Status200OK)]
        public async Task<IActionResult> MatchByPacketId(Guid accessPointPacketId)
        {
            var response = await _unitOfWork.AccessPointRepository.MatchAccessPointByAccessPointPacketId(accessPointPacketId);

            var mappedResponse = MapToDto<AccessPointDetails>(response);

            return Ok(mappedResponse);
        }

        [HttpGet("match/packet/{accessPointPacketId}/full")]
        [Authorize(Roles = "Admin, Support")]
        [ProducesResponseType(typeof(AccessPointDetails), StatusCodes.Status200OK)]
        public async Task<IActionResult> MatchByPacketIdFull(Guid accessPointPacketId)
        {
            var response = await _unitOfWork.AccessPointRepository.MatchAccessPointByAccessPointPacketIdAdministration(accessPointPacketId);

            var mappedResponse = MapToDto<AccessPointDetailsAdministration>(response);

            return Ok(mappedResponse);
        }

        [HttpGet("statistics/signal")]
        [ProducesResponseType(typeof(IEnumerable<AccessPointSimple>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStatisticsAccessPointWithGreaterSignalRange([FromQuery] int? limit)
        {
            var cachedResponse = ResolveFromCache();
            if (cachedResponse is not null) return Ok(cachedResponse);

            var response = await _unitOfWork.AccessPointRepository
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

            var response = await _unitOfWork.AccessPointRepository
                .GetMostCommonUsedFrequency(limit ?? _defaultLimit);

            StoreToCache(response);

            return Ok(response);
        }

        [HttpGet("statistics/manufacturer")]
        public async Task<IActionResult> GetStatisticsMostCommonUsedManufacturer([FromQuery] int? limit)
        {
            var cachedResponse = ResolveFromCache();
            if (cachedResponse is not null) return Ok(cachedResponse);

            var response = await _unitOfWork.AccessPointRepository
                .GetMostCommonUsedManufacturer(limit ?? _defaultLimit);

            StoreToCache(response);

            return Ok(response);
        }

        [HttpGet("statistics/encryption")]
        public async Task<IActionResult> GetStatisticsMostCommonUsedEncryption([FromQuery] int? limit)
        {
            var cachedResponse = ResolveFromCache();
            if (cachedResponse is not null) return Ok(cachedResponse);

            var response = await _unitOfWork.AccessPointRepository
                .GetMostCommonUsedEncryptionTypes(limit ?? _defaultLimit);

            StoreToCache(response);

            return Ok(response);
        }
    }
}
