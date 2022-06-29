using AccessPointMap.Domain.Core.Extensions;
using AccessPointMap.Domain.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace AccessPointMap.Domain.AccessPoints
{
    public class AccessPointSecurity : ValueObject<AccessPointSecurity>
    {
        public string RawSecurityPayload { get; private set; }
        public string SerializedSecurityPayload { get; private set; }
        
        public string SecurityStandard { get; private set; }
        public string SecurityProtocols { get; private set; }

        public bool IsSecure { get; private set; }

        private AccessPointSecurity() { }
        private AccessPointSecurity(string rawSecurityPayload)
        {
            // Format examples, for development purpose
            // Apm - [WPA-PSK-CCMP+TKIP][WPA2-PSK-CCMP+TKIP][ESS]
            // Wigle - [WPA-PSK-CCMP+TKIP][WPA2-PSK-CCMP+TKIP][WPS][ESS]
            // Aircrack - WPA WPA2

            ParseRawSecurityPayload(rawSecurityPayload);

            //if (rawSecurityPayload.IsEmpty())
            //{
            //    RawSecurityPayload = string.Empty;
            //    SerializedSecurityPayload = "[]";
            //    IsSecure = false;
            //}
            //else
            //{
            //    RawSecurityPayload = rawSecurityPayload.Trim().ToUpper();

            //    var usedEncryptionTypes = new List<EncryptionType>();

            //    foreach (var type in Constants.EncryptionTypes)
            //    {
            //        if (RawSecurityPayload.Contains(type.Name))
            //            usedEncryptionTypes.Add(type);
            //    }

            //    if (usedEncryptionTypes.Count > 0)
            //    {
            //        SerializedSecurityPayload = JsonSerializer.Serialize(usedEncryptionTypes.Select(t => t.Name));

            //        IsSecure = usedEncryptionTypes.OrderByDescending(t => t.Priority).First().IsSecure;
            //    }
            //    else
            //    {
            //        SerializedSecurityPayload = "[]";
                    
            //        IsSecure = false;
            //    }
            //}
        }

        private void ParseRawSecurityPayload(string rawSecurityPayload)
        {
            if (rawSecurityPayload.IsEmpty())
            {
                RawSecurityPayload = string.Empty;
                SerializedSecurityPayload = "[]";

                SecurityProtocols = "None";
                SecurityStandard = "[]";

                IsSecure = false;

                return;
            }

            throw new NotImplementedException();
        }

        public static AccessPointSecurity FromString(string rawSecurityPayload) => new AccessPointSecurity(rawSecurityPayload);
    }
}
