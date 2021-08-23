using AccessPointMap.Service.Settings;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccessPointMap.Service
{
    public class AccessPointHelperService : IAccessPointHelperService
    {
        private readonly DeviceTypeSettings _deviceTypeSettings;
        private readonly EncryptionTypeSettings _encryptionTypeSettings;

        public AccessPointHelperService(
            IOptions<DeviceTypeSettings> deviceTypeSettings,
            IOptions<EncryptionTypeSettings> encryptionTypeSettings)
        {
            _deviceTypeSettings = deviceTypeSettings.Value ??
                throw new ArgumentNullException(nameof(deviceTypeSettings));

            _encryptionTypeSettings = encryptionTypeSettings.Value ??
                throw new ArgumentNullException(nameof(encryptionTypeSettings));
        }

        public bool CheckIsSecure(string fullSecurityData)
        {
            var secureEncryptionTypes = _encryptionTypeSettings.SafeEncryptionStandards;
            string security = fullSecurityData.ToUpper();

            foreach (var type in secureEncryptionTypes)
            {
                if (security.Contains(type)) return true;
            }
            return false;
        }

        public string DetectDeviceType(string ssid)
        {
            ssid = ssid.ToLower().Trim();

            if (_deviceTypeSettings.PrinterKeywords.Any(x => ssid.Contains(x.ToLower()))) return "Printer";
            if (_deviceTypeSettings.AccessPointKeywords.Any(x => ssid.Contains(x.ToLower()))) return "Access point";
            if (_deviceTypeSettings.TvKeywords.Any(x => ssid.Contains(x.ToLower()))) return "TV";
            if (_deviceTypeSettings.CctvKeywords.Any(x => ssid.Contains(x.ToLower()))) return "CCTV";
            if (_deviceTypeSettings.RepeaterKeywords.Any(x => ssid.Contains(x.ToLower()))) return "Repeater";
            if (_deviceTypeSettings.IotKeywords.Any(x => ssid.Contains(x.ToLower()))) return "IoT";

            return null;
        }

        public IEnumerable<string> GetEncryptionsForStatistics()
        {
            return _encryptionTypeSettings.EncryptionStandardsForStatistics;
        }

        public string SerializeSecurityData(string fullSecurityData)
        {
            var encryptionTypes = _encryptionTypeSettings.EncryptionStandardsAndTypes;
            var types = new List<string>();

            foreach (var type in encryptionTypes)
            {
                if (fullSecurityData.Contains(type)) types.Add(type);
            }

            return JsonConvert.SerializeObject(types);
        }
    }
}
