using System.Collections.Generic;

namespace AccessPointMap.Service.Settings
{
    public class EncryptionTypeSettings
    {
        public IEnumerable<string> EncryptionStandardsAndTypes { get; set; }
        public IEnumerable<string> EncryptionStandardsForStatistics { get; set; }
        public IEnumerable<string> SafeEncryptionStandards { get; set; }
    }
}
