using AccessPointMap.Domain.Core.Extensions;
using AccessPointMap.Domain.Core.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AccessPointMap.Domain.AccessPoints
{
    public class AccessPointSecurity : ValueObject<AccessPointSecurity>
    {
        private const string _payloadTokenPattern = @"\b[A-Z0-9]+\b";
        private const string _empty = "[]";

        public string RawSecurityPayload { get; private set; }
        
        // TODO: Breaking change. SerializedSecurityPayload splited into standards and protocols
        // public string SerializedSecurityPayload { get; private set; }

        public string SecurityStandards { get; private set; }
        public string SecurityProtocols { get; private set; }

        public bool IsSecure { get; private set; }

        private AccessPointSecurity() { }
        private AccessPointSecurity(string rawSecurityPayload) =>
            ParseRawSecurityPayload(rawSecurityPayload);

        private void ParseRawSecurityPayload(string rawSecurityPayload)
        {
            if (rawSecurityPayload.IsEmpty())
            {
                RawSecurityPayload = string.Empty;

                SecurityProtocols = _empty;
                SecurityStandards = _empty;

                IsSecure = false;

                return;
            }

            RawSecurityPayload = rawSecurityPayload;

            var tokenCollection = Regex.Matches(rawSecurityPayload.ToUpper(), _payloadTokenPattern)
                .Cast<Match>()
                .Select(m => m.Value)
                .Distinct(); 

            var securityProtocols = new List<SecurityProtocol>();
            foreach (var token in tokenCollection)
            {
                var protocol = Constants.SecurityProtocols.GetValueOrDefault(token);
                if (protocol is null) continue;

                securityProtocols.Add(protocol);
            }

            var securityFrameworks = securityProtocols
                .Where(p => p.Type == SecurityProtocolType.Framework)
                .OrderByDescending(p => p.Priority);

            if (securityFrameworks.Any())
            {
                SecurityStandards = JsonSerializer.Serialize(securityFrameworks
                    .Select(p => p.Name));

                IsSecure = securityFrameworks.First().IsSecure;
            }
            else
            {
                SecurityStandards = _empty;
                IsSecure = false;
            }

            SecurityProtocols = JsonSerializer.Serialize(securityProtocols
                .Where(p => p.Type != SecurityProtocolType.Framework)
                .Select(p => p.Name));

            var containsUnsafeProtocols = securityProtocols
                .Any(p => p.Type != SecurityProtocolType.Framework && !p.IsSecure);

            if (IsSecure && containsUnsafeProtocols) IsSecure = false;
        }

        public static AccessPointSecurity FromString(string rawSecurityPayload) => new AccessPointSecurity(rawSecurityPayload);
    }
}
