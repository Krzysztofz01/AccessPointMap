﻿using AccessPointMap.Application.Core;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Pcap.Core
{
    public interface IPcapParsingService
    {
        public Task<Result<IDictionary<string, List<Packet>>>> MapPacketsToMacAddressesAsync(IFormFile pcapFile, CancellationToken cancellationToken = default);
    }
}
