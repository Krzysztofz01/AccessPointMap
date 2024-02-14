using AccessPointMap.API.Controllers.Base;
using AccessPointMap.Application.AccessPoints;
using AccessPointMap.Application.Kml.Core;
using AccessPointMap.Infrastructure.Core.Abstraction;
using Asp.Versioning;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static AccessPointMap.Application.AccessPoints.Dto;

namespace AccessPointMap.API.Controllers
{
    [Route("api/v{version:apiVersion}/accesspoints")]
    [ApiVersion("1.0")]
    public class AccessPointQueryController : QueryController<AccessPointQueryController>
    {
        private readonly IKmlParsingService _kmlParsingService;

        private const string KmlContentMimeType = "text/kml";

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
            [FromQuery]int? pageSize,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.AccessPointRepository
                .GetAllAccessPoints(startingData, endingDate, latitude, longitude, distance, keyword, page, pageSize, cancellationToken);

            return await HandleQueryResult(query, true, typeof(IEnumerable<AccessPointSimple>), cancellationToken);
        }

        [HttpGet("full")]
        [Authorize(Roles = "Admin, Support")]
        [ProducesResponseType(typeof(IEnumerable<AccessPointSimpleAdministration>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllFull(
            [FromQuery] DateTime? startingData,
            [FromQuery] DateTime? endingDate,
            [FromQuery] double? latitude,
            [FromQuery] double? longitude,
            [FromQuery] double? distance,
            [FromQuery] string keyword,
            [FromQuery] int? page,
            [FromQuery] int? pageSize,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.AccessPointRepository
                .GetAllAccessPointsAdministration(startingData, endingDate, latitude, longitude, distance, keyword, page, pageSize, cancellationToken);

            return await HandleQueryResult(query, false, typeof(IEnumerable<AccessPointSimpleAdministration>), cancellationToken);
        }

        [HttpGet("kml")]
        [Produces(KmlContentMimeType)]
        public async Task<IActionResult> GetAllInKml(
            CancellationToken cancellationToken)
        {
            var accessPointsResult = await _unitOfWork.AccessPointRepository
                .GetAllAccessPoints(null, null, null, null, null, null, null, null, cancellationToken);

            if (accessPointsResult.IsFailure) return GetFailureResponse(accessPointsResult.Error);

            var kmlFileResult = await _kmlParsingService.GenerateKmlAsync(accessPointsResult.Value, cancellationToken);

            if (kmlFileResult.IsFailure) return GetFailureResponse(accessPointsResult.Error);

            return await HandleFileResult(kmlFileResult.Value, true, KmlContentMimeType, cancellationToken);
        }

        [HttpGet("kml/full")]
        [Authorize(Roles = "Admin, Support")]
        [Produces(KmlContentMimeType)]
        public async Task<IActionResult> GetAllInKmlFull(
            CancellationToken cancellationToken)
        {
            var accessPointsResult = await _unitOfWork.AccessPointRepository
                .GetAllAccessPointsAdministration(null, null, null, null, null, null, null, null, cancellationToken);

            if (accessPointsResult.IsFailure) return GetFailureResponse(accessPointsResult.Error);

            var kmlFileResult = await _kmlParsingService.GenerateKmlAsync(accessPointsResult.Value, cancellationToken);

            if (kmlFileResult.IsFailure) return GetFailureResponse(accessPointsResult.Error);

            return await HandleFileResult(kmlFileResult.Value, true, KmlContentMimeType, cancellationToken);
        }

        [HttpGet("run")]
        [ProducesResponseType(typeof(IEnumerable<Guid>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllRunIds(
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.AccessPointRepository
                .GetAllAccessPointRunIds(cancellationToken);

            return await HandleQueryResult(query, true, cancellationToken);
        }

        [HttpGet("run/full")]
        [Authorize(Roles = "Admin, Support")]
        [ProducesResponseType(typeof(IEnumerable<Guid>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllRunIdsAdministration(
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.AccessPointRepository
                .GetAllAccessPointRunIdsAdministration(cancellationToken);

            return await HandleQueryResult(query, false, cancellationToken);
        }

        [HttpGet("run/{runId}")]
        [ProducesResponseType(typeof(IEnumerable<AccessPointSimple>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllByRunId(
            Guid runId,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.AccessPointRepository
                .GetAllAccessPointsByRunId(runId, cancellationToken);

            return await HandleQueryResult(query, true, typeof(IEnumerable<AccessPointSimple>), cancellationToken);
        }

        [HttpGet("run/{runId}/full")]
        [Authorize(Roles = "Admin, Support")]
        [ProducesResponseType(typeof(IEnumerable<AccessPointSimpleAdministration>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllByRunIdFull(
            Guid runId,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.AccessPointRepository
                .GetAllAccessPointsByRunIdAdministration(runId, cancellationToken);

            return await HandleQueryResult(query, false, typeof(IEnumerable<AccessPointSimpleAdministration>), cancellationToken);
        }

        [HttpGet("stamps/run/{runId}")]
        [ProducesResponseType(typeof(IEnumerable<AccessPointStampSimple>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllStampsBysRunId(
            Guid runId,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.AccessPointRepository
                .GetAllAccessPointStampsByRunId(runId, cancellationToken);

            return await HandleQueryResult(query, true, typeof(IEnumerable<AccessPointStampSimple>), cancellationToken);
        }

        [HttpGet("stamps/run/{runId}/full")]
        [Authorize(Roles = "Admin, Support")]
        [ProducesResponseType(typeof(IEnumerable<AccessPointStampSimple>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllStampsBysRunIdAdministration(
            Guid runId,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.AccessPointRepository
                .GetAllAccessPointStampsByRunIdAdministration(runId, cancellationToken);

            return await HandleQueryResult(query, false, typeof(IEnumerable<AccessPointStampSimple>), cancellationToken);
        }

        [HttpGet("{accessPointId}")]
        [ProducesResponseType(typeof(AccessPointDetails), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetById(
            Guid accessPointId,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.AccessPointRepository
                .GetAccessPointById(accessPointId, cancellationToken);

            return await HandleQueryResult(query, true, typeof(AccessPointDetails), cancellationToken);
        }

        [HttpGet("{accessPointId}/full")]
        [Authorize(Roles = "Admin, Support")]
        [ProducesResponseType(typeof(AccessPointDetailsAdministration), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetByIdFull(
            Guid accessPointId,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.AccessPointRepository
                .GetAccessPointByIdAdministration(accessPointId, cancellationToken);

            return await HandleQueryResult(query, false, typeof(AccessPointDetailsAdministration), cancellationToken);
        }

        [HttpGet("{accessPointId}/packet")]
        [Authorize(Roles = "Admin, Support")]
        [ProducesResponseType(typeof(IEnumerable<AccessPointPacketDetails>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetAllPackets(
            Guid accessPointId,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.AccessPointRepository
                .GetAllAccessPointsAccessPointPackets(accessPointId, cancellationToken);

            return await HandleQueryResult(query, false, typeof(IEnumerable<AccessPointPacketDetails>), cancellationToken);
        }

        [HttpGet("{accessPointId}/packet/{accessPointPacketId}")]
        [Authorize(Roles = "Admin, Support")]
        [ProducesResponseType(typeof(IEnumerable<AccessPointPacketDetails>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetPacketById(
            Guid accessPointId,
            Guid accessPointPacketId,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.AccessPointRepository
                .GetAccessPointsAccessPointPacketById(accessPointId, accessPointPacketId, cancellationToken);

            return await HandleQueryResult(query, false, typeof(AccessPointPacketDetails), cancellationToken);
        }

        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<AccessPointSimple>), StatusCodes.Status200OK)]
        [Obsolete("This endpoint will be removed in the upcoming major release.")]
        public async Task<IActionResult> GetByKeyword(
            [FromQuery] string keyword,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.AccessPointRepository
                .SearchByKeyword(keyword, cancellationToken);

            return await HandleQueryResult(query, false, typeof(IEnumerable<AccessPointSimple>), cancellationToken);
        }

        [HttpGet("match/stamp/{accessPointStampId}")]
        [ProducesResponseType(typeof(AccessPointDetails), StatusCodes.Status200OK)]
        public async Task<IActionResult> MatchByStampId(
            Guid accessPointStampId,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.AccessPointRepository
                .MatchAccessPointByAccessPointStampId(accessPointStampId, cancellationToken);

            return await HandleQueryResult(query, false, typeof(AccessPointDetails), cancellationToken);
        }

        [HttpGet("match/stamp/{accessPointStampId}/full")]
        [Authorize(Roles = "Admin, Support")]
        [ProducesResponseType(typeof(AccessPointDetailsAdministration), StatusCodes.Status200OK)]
        public async Task<IActionResult> MatchByStampIdFull(
            Guid accessPointStampId,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.AccessPointRepository
                .MatchAccessPointByAccessPointStampIdAdministration(accessPointStampId, cancellationToken);

            return await HandleQueryResult(query, false, typeof(AccessPointDetailsAdministration), cancellationToken);
        }

        [HttpGet("match/packet/{accessPointPacketId}")]
        [ProducesResponseType(typeof(AccessPointDetails), StatusCodes.Status200OK)]
        public async Task<IActionResult> MatchByPacketId(
            Guid accessPointPacketId,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.AccessPointRepository
                .MatchAccessPointByAccessPointPacketId(accessPointPacketId, cancellationToken);

            return await HandleQueryResult(query, false, typeof(AccessPointDetails), cancellationToken);
        }

        [HttpGet("match/packet/{accessPointPacketId}/full")]
        [Authorize(Roles = "Admin, Support")]
        [ProducesResponseType(typeof(AccessPointDetailsAdministration), StatusCodes.Status200OK)]
        public async Task<IActionResult> MatchByPacketIdFull(
            Guid accessPointPacketId,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.AccessPointRepository
                .MatchAccessPointByAccessPointPacketIdAdministration(accessPointPacketId, cancellationToken);

            return await HandleQueryResult(query, false, typeof(AccessPointDetailsAdministration), cancellationToken);
        }

        [HttpGet("statistics/signal")]
        [ProducesResponseType(typeof(IEnumerable<AccessPointSimple>), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetStatisticsAccessPointWithGreaterSignalRange(
            [FromQuery] int? limit,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.AccessPointRepository
                .GetAccessPointsWithGreatestSignalRange(NormalizePaginationLimit(limit), cancellationToken);

            return await HandleQueryResult(query, true, typeof(IEnumerable<AccessPointSimple>), cancellationToken);
        }

        [HttpGet("statistics/frequency")]
        public async Task<IActionResult> GetStatisticsMostCommonUsedFrequency(
            [FromQuery] int? limit,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.AccessPointRepository
                .GetMostCommonUsedFrequency(NormalizePaginationLimit(limit), cancellationToken);

            return await HandleQueryResult(query, true, cancellationToken);
        }

        [HttpGet("statistics/manufacturer")]
        public async Task<IActionResult> GetStatisticsMostCommonUsedManufacturer(
            [FromQuery] int? limit,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.AccessPointRepository
                .GetMostCommonUsedManufacturer(NormalizePaginationLimit(limit), cancellationToken);

            return await HandleQueryResult(query, true, cancellationToken);
        }

        [HttpGet("statistics/encryption")]
        public async Task<IActionResult> GetStatisticsMostCommonUsedEncryption(
            [FromQuery] int? limit,
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.AccessPointRepository
                .GetMostCommonUsedEncryptionTypes(NormalizePaginationLimit(limit), cancellationToken);

            return await HandleQueryResult(query, true, cancellationToken);
        }

        [HttpGet("map/center")]
        public async Task<IActionResult> GetMapCenterLocation(
            CancellationToken cancellationToken)
        {
            var query = _unitOfWork.AccessPointRepository
                .GetAveragePublicAccessPointsLocation(cancellationToken);

            return await HandleQueryResult(query, true, cancellationToken);
        }
    }
}
