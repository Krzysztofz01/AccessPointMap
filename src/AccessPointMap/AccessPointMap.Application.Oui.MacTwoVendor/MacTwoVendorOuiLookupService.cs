using AccessPointMap.Application.Oui.Core;
using AccessPointMap.Application.Oui.MacToVendor.Database;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Oui.MacToVendor
{
    public class MacTwoVendorOuiLookupService : IOuiLookupService
    {
        private readonly MacTwoVendorDbContext _dbContext;

        public MacTwoVendorOuiLookupService(MacTwoVendorDbContext dbContext)
        {
            _dbContext = dbContext ??
                throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<string> GetManufacturerName(string macAddress)
        {
            string hexadecimalOuiMacPart = macAddress
                .Replace(":", string.Empty)
                .Substring(0, 6);

            int decimalOuiMacPart = Convert.ToInt32(hexadecimalOuiMacPart, 16);

            var vendor = await _dbContext.Vendors
                .SingleOrDefaultAsync(v => v.MacAddress == decimalOuiMacPart && v.Visibility == 1);

            if (vendor is null) return string.Empty;

            return vendor.Name;
        }
    }
}
