﻿using AccessPointMap.Application.Abstraction;
using AccessPointMap.Application.Logging;
using AccessPointMap.Application.Oui.Core;
using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Domain.Core.Events;
using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.Extensions.Logging;
using System;
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

        public async Task Handle(IApplicationCommand<AccessPoint> command)
        {
            _logger.LogApplicationCommand(command);

            switch (command)
            {
                case V1.Create c: await HandleCreate(c); break;

                case V1.Delete c: await Apply(c.Id, new AccessPointDeleted { Id = c.Id }); break;
                case V1.Update c: await Apply(c.Id, new AccessPointUpdated { Id = c.Id, Note = c.Note }); break;
                case V1.ChangeDisplayStatus c: await Apply(c.Id, new AccessPointDisplayStatusChanged { Id = c.Id, Status = c.Status.Value }); break;
                case V1.MergeWithStamp c: await Apply(c.Id, new AccessPointMergedWithStamp { Id = c.Id, StampId = c.StampId, MergeSsid = c.MergeSsid.Value, MergeLowSignalLevel = c.MergeLowSignalLevel.Value, MergeHighSignalLevel = c.MergeHighSignalLevel.Value, MergeSecurityData = c.MergeSecurityData.Value }); break;
                case V1.DeleteStamp c: await Apply(c.Id, new AccessPointStampDeleted { Id = c.Id, StampId = c.StampId }); break;
                case V1.CreateAdnnotation c: await Apply(c.Id, new AccessPointAdnnotationCreated { Id = c.Id, Title = c.Title, Content = c.Content }); break;
                case V1.DeleteAdnnotation c: await Apply(c.Id, new AccessPointAdnnotationDeleted { Id = c.Id, AdnnotationId = c.AdnnotationId }); break;

                default: throw new InvalidOperationException("This command is not supported.");
            }
        }

        private async Task Apply(Guid id, IEventBase @event)
        {
            var accessPoint = await _unitOfWork.AccessPointRepository.Get(id);

            _logger.LogDomainEvent(@event);

            accessPoint.Apply(@event);

            await _unitOfWork.Commit();
        }

        private async Task HandleCreate(V1.Create command)
        {
            var userId = _scopeWrapperService.GetUserId();
            var runId = Guid.NewGuid();

            //Iterate through all pushed access points
            foreach (var ap in command.AccessPoints)
            {
                //If bssid does not exist create aggregate, otherwise a new stamp in existing aggregate
                if (await _unitOfWork.AccessPointRepository.Exists(ap.Bssid))
                {
                    var accessPoint = await _unitOfWork.AccessPointRepository.Get(ap.Bssid);

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

                    await _unitOfWork.Commit();
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

                    var accessPointManufacturerChangedEvent = new AccessPointManufacturerChanged
                    {
                        Id = accessPoint.Id,
                        Manufacturer = await ResolveManufacturer(accessPoint.Bssid)
                    };

                    _logger.LogDomainEvent(accessPointManufacturerChangedEvent);

                    accessPoint.Apply(accessPointManufacturerChangedEvent);

                    await _unitOfWork.AccessPointRepository.Add(accessPoint);

                    await _unitOfWork.Commit();
                }
            }
        }

        private async Task<string> ResolveManufacturer(string bssid)
        {
            return await _ouiLookupService.GetManufacturerName(bssid);
        }
    }
}
