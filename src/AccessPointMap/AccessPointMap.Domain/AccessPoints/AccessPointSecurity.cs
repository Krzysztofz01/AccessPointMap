using AccessPointMap.Domain.Core.Extensions;
using AccessPointMap.Domain.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace AccessPointMap.Domain.AccessPoints
{
    public class AccessPointSecurity : ValueObject<AccessPointSecurity>
    {
        public string RawSecurityPayload { get; private set; }
        public string SerializedSecurityPayload { get; private set; }
        
        public bool IsSecure { get; private set; }

        private AccessPointSecurity() { }
        private AccessPointSecurity(string rawSecurityPayload)
        {
            if (rawSecurityPayload.IsEmpty())
            {
                RawSecurityPayload = string.Empty;
                SerializedSecurityPayload = "[]";
                IsSecure = false;
            }
            else
            {
                RawSecurityPayload = rawSecurityPayload.Trim().ToUpper();

                var usedEncryptionTypes = new List<EncryptionType>();

                foreach (var type in Constants.EncryptionTypes)
                {
                    if (RawSecurityPayload.Contains(type.Name))
                        usedEncryptionTypes.Add(type);
                }

                if (usedEncryptionTypes.Count > 0)
                {
                    SerializedSecurityPayload = JsonSerializer.Serialize(usedEncryptionTypes.Select(t => t.Name));

                    IsSecure = usedEncryptionTypes.OrderByDescending(t => t.Priority).First().IsSecure;
                }
                else
                {
                    SerializedSecurityPayload = "[]";
                    
                    IsSecure = false;
                }
            }
        }

        public static AccessPointSecurity FromString(string rawSecurityPayload) => new AccessPointSecurity(rawSecurityPayload);
    }
}
