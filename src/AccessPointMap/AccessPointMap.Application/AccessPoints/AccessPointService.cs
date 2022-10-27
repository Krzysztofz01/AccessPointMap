using AccessPointMap.Application.Core;
using AccessPointMap.Application.Core.Abstraction;
using AccessPointMap.Application.Logging;
using AccessPointMap.Application.Oui.Core;
using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Domain.Core.Events;
using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using static AccessPointMap.Application.AccessPoints.Commands;
using static AccessPointMap.Domain.AccessPoints.Events.V1;

namespace AccessPointMap.Application.AccessPoints
{
    public class AccessPointService : IAccessPointService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IScopeWrapperService _scopeWrapperService;
        private readonly IOuiLookupService _ouiLookupService;
        private readonly ILogger<AccessPointService> _logger;

        public AccessPointService(IUnitOfWork unitOfWork, IScopeWrapperService scopeWrapperService, IOuiLookupService ouiLookupService, ILogger<AccessPointService> logger)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));

            _scopeWrapperService = scopeWrapperService ??
                throw new ArgumentNullException(nameof(scopeWrapperService));

            _ouiLookupService = ouiLookupService ??
                throw new ArgumentNullException(nameof(ouiLookupService));

            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result> HandleAsync(IApplicationCommand<AccessPoint> command, CancellationToken cancellationToken = default)
        {
            try
            {
                _logger.LogApplicationCommand(command);

                switch (command)
                {
                    case V1.Create c: await HandleCreateAsync(c, cancellationToken); break;

                    case V1.Delete c: await ApplyAsync(c.Id, new AccessPointDeleted { Id = c.Id }, cancellationToken); break;
                    case V1.DeleteRange c: await ApplyDeleteRangeAsync(c.Ids, cancellationToken); break;
                    case V1.Update c: await ApplyAsync(c.Id, new AccessPointUpdated { Id = c.Id, Note = c.Note }, cancellationToken); break;
                    case V1.ChangeDisplayStatus c: await ApplyAsync(c.Id, new AccessPointDisplayStatusChanged { Id = c.Id, Status = c.Status.Value }, cancellationToken); break;
                    case V1.ChangeDisplayStatusRange c: await ApplyChangeDisplayStatusRangeAsync(c.Ids, c.Status.Value, cancellationToken); break;
                    case V1.MergeWithStamp c: await ApplyAsync(c.Id, new AccessPointMergedWithStamp { Id = c.Id, StampId = c.StampId, MergeSsid = c.MergeSsid.Value, MergeLowSignalLevel = c.MergeLowSignalLevel.Value, MergeHighSignalLevel = c.MergeHighSignalLevel.Value, MergeSecurityData = c.MergeSecurityData.Value }, cancellationToken); break;
                    case V1.DeleteStamp c: await ApplyAsync(c.Id, new AccessPointStampDeleted { Id = c.Id, StampId = c.StampId }, cancellationToken); break;
                    case V1.CreateAdnnotation c: await ApplyAsync(c.Id, new AccessPointAdnnotationCreated { Id = c.Id, Title = c.Title, Content = c.Content }, cancellationToken); break;
                    case V1.DeleteAdnnotation c: await ApplyAsync(c.Id, new AccessPointAdnnotationDeleted { Id = c.Id, AdnnotationId = c.AdnnotationId }, cancellationToken); break;

                    default: return Result.Failure(Error.FromString("This command is not supported."));
                }

                return Result.Success("Access point command applied successful.");
            }
            catch (DomainException ex)
            {
                var error = Error.FromException(ex);         
                
                return Result.Failure(error);
            }
            catch (TaskCanceledException)
            {
                throw;
            }
            catch
            {
                throw;
            }
        }

        private async Task ApplyAsync(Guid id, IEventBase @event, CancellationToken cancellationToken = default)
        {
            var accessPoint = await _unitOfWork.AccessPointRepository.GetAsync(id, cancellationToken);

            _logger.LogDomainEvent(@event);

            accessPoint.Apply(@event);

            await _unitOfWork.Commit(cancellationToken);
        }

        private async Task ApplyDeleteRangeAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken = default)
        {
            foreach (var id in ids)
            {
                var accessPoint = await _unitOfWork.AccessPointRepository.GetAsync(id, cancellationToken);

                var @event = new AccessPointDeleted
                {
                    Id = accessPoint.Id
                };

                _logger.LogDomainEvent(@event);

                accessPoint.Apply(@event);
            }

            await _unitOfWork.Commit(cancellationToken);
        }

        private async Task ApplyChangeDisplayStatusRangeAsync(IEnumerable<Guid> ids, bool displayStatus, CancellationToken cancellationToken = default)
        {
            foreach (var id in ids)
            {
                var accessPoint = await _unitOfWork.AccessPointRepository.GetAsync(id, cancellationToken);

                var @event = new AccessPointDisplayStatusChanged
                {
                    Id = accessPoint.Id,
                    Status = displayStatus
                };

                _logger.LogDomainEvent(@event);

                accessPoint.Apply(@event);
            }

            await _unitOfWork.Commit(cancellationToken);
        }

        private async Task HandleCreateAsync(V1.Create command, CancellationToken cancellationToken = default)
        {
            var userId = _scopeWrapperService.GetUserId();
            var runId = Guid.NewGuid();

            //Iterate through all pushed access points
            foreach (var ap in command.AccessPoints)
            {
                //If bssid does not exist create aggregate, otherwise a new stamp in existing aggregate
                if (await _unitOfWork.AccessPointRepository.ExistsAsync(ap.Bssid, cancellationToken))
                {
                    var accessPoint = await _unitOfWork.AccessPointRepository.GetAsync(ap.Bssid, cancellationToken);

                    var accessPointStampCreateEvent = new AccessPointStampCreated
                    {
                        Id = accessPoint.Id,
                        Ssid = ap.Ssid,
                        Frequency = ap.Frequency.Value,
                        LowSignalLevel = ap.LowSignalLevel.Value,
                        LowSignalLatitude = ap.LowSignalLatitude.Value,
                        LowSignalLongitude = ap.LowSignalLongitude.Value,
                        HighSignalLevel = ap.HighSignalLevel.Value,
                        HighSignalLatitude = ap.HighSignalLatitude.Value,
                        HighSignalLongitude = ap.HighSignalLongitude.Value,
                        RawSecurityPayload = ap.RawSecurityPayload,
                        UserId = userId,
                        ScanDate = command.ScanDate.Value,
                        RunIdentifier = runId
                    };

                    _logger.LogDomainEvent(accessPointStampCreateEvent);

                    accessPoint.Apply(accessPointStampCreateEvent);
                }
                else
                {
                    var accessPointCreateEvent = new AccessPointCreated
                    {
                        Bssid = ap.Bssid,
                        Ssid = ap.Ssid,
                        Frequency = ap.Frequency.Value,
                        LowSignalLevel = ap.LowSignalLevel.Value,
                        LowSignalLatitude = ap.LowSignalLatitude.Value,
                        LowSignalLongitude = ap.LowSignalLongitude.Value,
                        HighSignalLevel = ap.HighSignalLevel.Value,
                        HighSignalLatitude = ap.HighSignalLatitude.Value,
                        HighSignalLongitude = ap.HighSignalLongitude.Value,
                        RawSecurityPayload = ap.RawSecurityPayload,
                        UserId = userId,
                        ScanDate = command.ScanDate.Value,
                        RunIdentifier = runId
                    };

                    _logger.LogDomainEvent(accessPointCreateEvent);

                    var accessPoint = AccessPoint.Factory.Create(accessPointCreateEvent);

                    var ouiLookupResult = await _ouiLookupService.GetManufacturerNameAsync(accessPoint.Bssid, cancellationToken);

                    if (ouiLookupResult.IsSuccess)
                    {
                        var accessPointManufacturerChangedEvent = new AccessPointManufacturerChanged
                        {
                            Id = accessPoint.Id,
                            Manufacturer = ouiLookupResult.Value
                        };

                        _logger.LogDomainEvent(accessPointManufacturerChangedEvent);

                        accessPoint.Apply(accessPointManufacturerChangedEvent);
                    }

                    await _unitOfWork.AccessPointRepository.AddAsync(accessPoint, cancellationToken);
                }
            }

            await _unitOfWork.Commit(cancellationToken);
        }
    }
}
