using AccessPointMap.Application.Abstraction;
using AccessPointMap.Application.Integration.Core;
using AccessPointMap.Application.Integration.Core.Exceptions;
using AccessPointMap.Application.Oui.Core;
using AccessPointMap.Application.Pcap.Core;
using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Infrastructure.Core.Abstraction;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
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
            IScopeWrapperService scopeWrapperService,
            IPcapParsingService pcapParsingService,
            IOuiLookupService ouiLookupService) : base(unitOfWork, scopeWrapperService, pcapParsingService, ouiLookupService) { }

        public async Task<Result> HandleCommandAsync(IIntegrationCommand command, CancellationToken cancellationToken = default)
        {
            try
            {
                return command switch
                {
                    Commands.CreatePacketsFromPcapFile cmd => await HandleCommand(cmd, cancellationToken),
                    _ => throw new IntegrationException($"This command is not supported by the {IntegrationName} integration."),
                };
            }
            catch (DomainException ex)
            {
                return Result.Failure(IntegrationError.FromDomainException(ex));
            }
            catch (IntegrationException ex)
            {
                return Result.Failure(IntegrationError.FromIntegrationException(ex));
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

        public async Task<Result<object>> HandleQueryAsync(IIntegrationQuery query, CancellationToken cancellationToken = default)
        {
            try
            {
                return query switch
                {
                    _ => throw new IntegrationException($"This query is not supported by the {IntegrationName} integration."),
                };
            }
            catch (DomainException ex)
            {
                return await Task.FromResult(Result.Failure(IntegrationError.FromDomainException(ex)));
            }
            catch (IntegrationException ex)
            {
                return await Task.FromResult(Result.Failure(IntegrationError.FromIntegrationException(ex)));
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

        private async Task<Result> HandleCommand(Commands.CreatePacketsFromPcapFile cmd, CancellationToken cancellationToken = default)
        {
            if (cmd.ScanPcapFile is null)
                return Result.Failure(WiresharkIntegrationError.UploadedPcapFileIsNull);

            if (Path.GetExtension(cmd.ScanPcapFile.FileName).ToLower() != ".pcap")
                return Result.Failure(WiresharkIntegrationError.UploadedFileHasInvalidFormat);

            var packetMap = await PcapParsingService.MapPacketsToMacAddressesAsync(cmd.ScanPcapFile, cancellationToken);

            foreach (var map in packetMap)
            {
                cancellationToken.ThrowIfCancellationRequested();

                await CreateAccessPointPackets(map.Key, map.Value, cancellationToken);
            }

            await UnitOfWork.Commit(cancellationToken);

            return Result.Success();
        }

        private async Task CreateAccessPointPackets(string bssid, IEnumerable<Packet> packets, CancellationToken cancellationToken = default)
        {
            if (!await UnitOfWork.AccessPointRepository.ExistsAsync(bssid, cancellationToken)) return;

            var accessPoint = await UnitOfWork.AccessPointRepository.GetAsync(bssid, cancellationToken);

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
