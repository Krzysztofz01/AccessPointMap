using System.Collections.Generic;

namespace AccessPointMap.Service
{
    public interface IAccessPointHelperService
    {
        string SerializeSecurityData(string fullSecurityData);
        string DetectDeviceType(string ssid);
        bool CheckIsSecure(string fullSecurityData);
        IEnumerable<string> GetEncryptionsForStatistics();
    }
}
