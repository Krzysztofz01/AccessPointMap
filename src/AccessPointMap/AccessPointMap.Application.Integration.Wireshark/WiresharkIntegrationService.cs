using AccessPointMap.Application.Integration.Core;
using AccessPointMap.Application.Integration.Core.Exceptions;
using AccessPointMap.Application.Oui.Core;
using AccessPointMap.Application.Pcap.Core;
using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Infrastructure.Core.Abstraction;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Integration.Wireshark
{
    public class WiresharkIntegrationService : AccessPointIntegrationBase<WiresharkIntegrationService>, IWiresharkIntegrationService
    {
        private readonly string _adnnotationName = "Wireshark integration provided data";

        private const string _integrationName = "Wireshark";
        private const string _integrationDescription = "Integration for the world’s foremost and widely-used network protocol analyzer";
        private const string _integrationVersion = "0.0.0-alpha";

        protected override string IntegrationName => _integrationName;
        protected override string IntegrationDescription => _integrationDescription;
        protected override string IntegrationVersion => _integrationVersion;

        public WiresharkIntegrationService(
            IUnitOfWork unitOfWork,
            IDataAccess dataAccess,
            IScopeWrapperService scopeWrapperService,
            IPcapParsingService pcapParsingService,
            IOuiLookupService ouiLookupService) : base(unitOfWork, dataAccess, scopeWrapperService, pcapParsingService, ouiLookupService) { }

        public async Task Handle(IIntegrationCommand command)
        {
            switch (command)
            {
                case Commands.CreatePacketsFromPcapFile cmd: await HandleCommand(cmd); break;

                default: throw new IntegrationException($"This command is not supported by the {IntegrationName} integration.");
            }
        }

        public Task<object> Query(IIntegrationQuery query)
        {
            switch (query)
            {
                default: throw new IntegrationException($"This query is not supported by the {IntegrationName} integration.");
            }
        }

        private async Task HandleCommand(Commands.CreatePacketsFromPcapFile cmd)
        {
            if (cmd.ScanPcapFile is null)
                throw new ArgumentNullException(nameof(cmd));

            if (Path.GetExtension(cmd.ScanPcapFile.FileName).ToLower() != ".pcap")
                throw new ArgumentNullException(nameof(cmd));

            var packetMap = await PcapParsingService.MapPacketsToMacAddresses(cmd.ScanPcapFile);

            foreach (var map in packetMap)
            {
                await CreateAccessPointPackets(map.Key, map.Value);
            }

            await UnitOfWork.Commit();
        }

        private async Task CreateAccessPointPackets(string bssid, IEnumerable<Packet> packets)
        {
            // TODO: Pass the CancellationToken to the repository method
            if (!await UnitOfWork.AccessPointRepository.ExistsAsync(bssid)) return;

            // TODO: Pass the CancellationToken to the repository method
            var accessPoint = await UnitOfWork.AccessPointRepository.GetAsync(bssid);

            foreach (var packet in packets)
            {
                accessPoint.Apply(new Events.V1.AccessPointPacketCreated
                {
                    Id = accessPoint.Id,
                    SourceAddress = packet.SourceAddress,
                    DestinationAddress = packet.DestinationAddress,
                    FrameType = packet.FrameType,
                    Data = packet.Data
                });
            }

            accessPoint.Apply(new Events.V1.AccessPointAdnnotationCreated
            {
                Id = accessPoint.Id,
                Title = _adnnotationName,
                Content = $"Inserted {packets.Count()} IEEE 802.11 frames."
            });
        }
    }
}
