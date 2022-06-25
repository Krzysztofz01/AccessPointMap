using AccessPointMap.Application.Pcap.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Pcap.ApmPcapNative
{
    public class ApmPcapNativePcapParsingService : IPcapParsingService
    {
        public Task<IDictionary<string, IList<Packet>>> MapPacketsToMacAddresses(IFormFile pcapFile)
        {
            throw new NotImplementedException();
        }
    }
}
