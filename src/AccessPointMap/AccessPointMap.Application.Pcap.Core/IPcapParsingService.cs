using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Pcap.Core
{
    public interface IPcapParsingService
    {
        public Task<IDictionary<string, IList<Packet>>> MapPacketsToMacAddresses(IFormFile pcapFile);
    }
}
