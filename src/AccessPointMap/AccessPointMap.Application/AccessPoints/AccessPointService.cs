using AccessPointMap.Application.Abstraction;
using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Domain.Core.Events;
using AccessPointMap.Infrastructure.Core.Abstraction;
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

        public AccessPointService(IUnitOfWork unitOfWork, IScopeWrapperService scopeWrapperService)
        {
            _unitOfWork = unitOfWork ??
                throw new ArgumentNullException(nameof(unitOfWork));

            _scopeWrapperService = scopeWrapperService ??
                throw new ArgumentNullException(nameof(scopeWrapperService));
        }

        public async Task Handle(IApplicationCommand<AccessPoint> command)
        {
            switch (command)
            {
                case V1.Create c: await HandleCreate(c); break;

                case V1.Delete c: await Apply(c.Id, new AccessPointDeleted { Id = c.Id }); break;
                case V1.Update c: await Apply(c.Id, new AccessPointUpdated { Id = c.Id, Note = c.Note }); break;
                case V1.ChangeDisplayStatus c: await Apply(c.Id, new AccessPointDisplayStatusChanged { Id = c.Id, Status = c.Status }); break;
                case V1.MergeWithStamp c: await Apply(c.Id, new AccessPointMergedWithStamp { Id = c.Id, StampId = c.StampId }); break;
                case V1.DeleteStamp c: await Apply(c.Id, new AccessPointStampDeleted { Id = c.Id, StampId = c.Id }); break;

                default: throw new InvalidOperationException("This command is not supported.");
            }
        }

        private async Task Apply(Guid id, IEventBase @event)
        {
            var accessPoint = await _unitOfWork.AccessPointRepository.Get(id);

            accessPoint.Apply(@event);

            await _unitOfWork.Commit();
        }

        private async Task HandleCreate(V1.Create command)
        {
            var userId = _scopeWrapperService.GetUserId();

            //Iterate through all pushed access points
            foreach (var ap in command.AccessPoints)
            {
                //If bssid does not exist create aggregate, otherwise a new stamp in existing aggregate
                if (await _unitOfWork.AccessPointRepository.Exists(ap.Bssid))
                {
                    var accessPoint = await _unitOfWork.AccessPointRepository.Get(ap.Bssid);

                    accessPoint.Apply(new AccessPointStampCreated
                    {
                        Id = accessPoint.Id,
                        Ssid = ap.Ssid,
                        Frequency = ap.Frequency,
                        LowSignalLevel = ap.LowSignalLevel,
                        LowSignalLatitude = ap.LowSignalLatitude,
                        LowSignalLongitude = ap.LowSignalLongitude,
                        HighSignalLevel = ap.HighSignalLevel,
                        HighSignalLatitude = ap.HighSignalLatitude,
                        HighSignalLongitude = ap.HighSignalLongitude,
                        RawSecurityPayload = ap.RawSecurityPayload,
                        UserId = userId
                    });

                    await _unitOfWork.Commit();
                }
                else
                {
                    var accessPoint = AccessPoint.Factory.Create(new AccessPointCreated
                    {
                        Bssid = ap.Bssid,
                        Ssid = ap.Ssid,
                        Frequency = ap.Frequency,
                        LowSignalLevel = ap.LowSignalLevel,
                        LowSignalLatitude = ap.LowSignalLatitude,
                        LowSignalLongitude = ap.LowSignalLongitude,
                        HighSignalLevel = ap.HighSignalLevel,
                        HighSignalLatitude = ap.HighSignalLatitude,
                        HighSignalLongitude = ap.HighSignalLongitude,
                        RawSecurityPayload = ap.RawSecurityPayload,
                        UserId = userId
                    });

                    await _unitOfWork.AccessPointRepository.Add(accessPoint);

                    await _unitOfWork.Commit();
                }
            }
        }
    }
}
