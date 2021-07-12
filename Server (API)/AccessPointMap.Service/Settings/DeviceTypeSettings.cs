using System.Collections.Generic;

namespace AccessPointMap.Service.Settings
{
    public class DeviceTypeSettings
    {
        public IEnumerable<string> PrinterKeywords { get; set; }
        public IEnumerable<string> AccessPointKeywords { get; set; }
        public IEnumerable<string> TvKeywords { get; set; }
        public IEnumerable<string> CctvKeywords { get; set; }
        public IEnumerable<string> RepeaterKeywords { get; set; }
        public IEnumerable<string> IotKeywords { get; set; }
    }
}
