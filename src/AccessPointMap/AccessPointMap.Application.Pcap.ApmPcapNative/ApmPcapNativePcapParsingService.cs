using AccessPointMap.Application.Core;
using AccessPointMap.Application.Pcap.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Pcap.ApmPcapNative
{
    internal sealed class ApmPcapNativePcapParsingService : IPcapParsingService
    {
        private readonly ILogger<ApmPcapNativePcapParsingService> _logger;
        
        private readonly string[] _allowedExtensions = new string[] { ".cap", ".pcap", "pcapng" };

        public ApmPcapNativePcapParsingService(ILogger<ApmPcapNativePcapParsingService> logger)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        public async Task<Result<IDictionary<string, List<Packet>>>> MapPacketsToMacAddressesAsync(IFormFile pcapFile, CancellationToken cancellationToken = default)
        {
            if (pcapFile is null)
                return Result.Failure<IDictionary<string, List<Packet>>>(PcapParserError.UploadedPcapFileIsNull);

            var extension = Path.GetExtension(pcapFile.FileName);
            if (!_allowedExtensions.Contains(extension.ToLower()))
                return Result.Failure<IDictionary<string, List<Packet>>>(PcapParserError.UploadedFileHasInvalidFormat);

            try
            {
                using var fileMemoryStream = new MemoryStream();
                await pcapFile.CopyToAsync(fileMemoryStream, cancellationToken);
                fileMemoryStream.Seek(0, SeekOrigin.Begin);

                using var pcapParser = new PcapParser(fileMemoryStream);
                var parsedPackets = pcapParser.ParsePackets(cancellationToken);

                if (parsedPackets is null)
                    return Result.Failure<IDictionary<string, List<Packet>>>(PcapParserError.FatalError);

                var mappedParsedPackets = parsedPackets
                    .GroupBy(k => k.SourceAddress)
                    .Where(k => k.Key != string.Empty)
                    .ToDictionary(k => k.Key, v => v.ToList());

                return Result.Success<IDictionary<string, List<Packet>>>(mappedParsedPackets);
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Application service: {ServiceName} failure.\n   {Exception}",
                    typeof(ApmPcapNativePcapParsingService).Name, ex);

                return Result.Failure<IDictionary<string, List<Packet>>>(PcapParserError.FatalError);
            }
        }
    }
}
