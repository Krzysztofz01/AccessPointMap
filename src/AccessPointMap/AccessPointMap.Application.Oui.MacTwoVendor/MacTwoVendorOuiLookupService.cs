using AccessPointMap.Application.Core;
using AccessPointMap.Application.Oui.Core;
using AccessPointMap.Application.Oui.MacToVendor.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Oui.MacToVendor
{
    internal sealed class MacTwoVendorOuiLookupService : IOuiLookupService
    {
        private readonly MacTwoVendorDbContext _dbContext;

        public MacTwoVendorOuiLookupService(MacTwoVendorDbContext dbContext)
        {
            _dbContext = dbContext ??
                throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<Result<IDictionary<string, string>>> GetManufacturerLookupDictionaryAsync(IEnumerable<string> macAddresses, CancellationToken cancellationToken = default)
        {
            var distinctOuiParts = macAddresses
                .Select(a => GetOuiMacAddressPart(a))
                .Distinct();

            var vendorsLookup = await _dbContext.Vendors
                .Where(v => distinctOuiParts.Contains(v.MacAddress) && v.Visibility == 1)
                .Select(v => new { Name = v.Name ?? string.Empty, v.MacAddress })
                .AsNoTracking()
                .ToDictionaryAsync(k => k.MacAddress, v => v.Name, cancellationToken);

            var addressToVendorLookup = macAddresses.ToDictionary(k => k, v => vendorsLookup[GetOuiMacAddressPart(v)]);
            return Result.Success<IDictionary<string, string>>(addressToVendorLookup);
        }

        public async Task<Result<string>> GetManufacturerNameAsync(string macAddress, CancellationToken cancellationToken = default)
        {
            var vendor = await _dbContext.Vendors
                .Where(v => v.MacAddress == GetOuiMacAddressPart(macAddress) && v.Visibility == 1)
                .Select(v => v.Name)
                .AsNoTracking()
                .SingleOrDefaultAsync(cancellationToken);

            if (vendor is null) return Result.Failure<string>(OuiServiceError.OuiNotFound);

            return Result.Success<string>(vendor);
        }

        private static int GetOuiMacAddressPart(string macAddress)
        {
            string hexadecimalOuiMacPart = macAddress
                .Replace(":", string.Empty)[..6];

            return Convert.ToInt32(hexadecimalOuiMacPart, 16);
        }
    }
}
