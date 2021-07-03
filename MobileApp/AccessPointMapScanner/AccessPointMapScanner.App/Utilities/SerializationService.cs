using System.Collections.Generic;
using System.Linq;
using AccessPointMapScanner.App.Models;
using Newtonsoft.Json;

namespace AccessPointMapScanner.App.Utilities
{
    public static class SerializationService
    {
        public static string SerializeAccessPoints(IDictionary<string, AccessPoint> accessPointsDict)
        {
            var accessPointList = accessPointsDict.Select(x => x.Value);

            return JsonConvert.SerializeObject(accessPointList);
        }

        public static IEnumerable<AccessPoint> DeserializeAccessPoints(string serializedAccessPoints)
        {
            return JsonConvert.DeserializeObject<IEnumerable<AccessPoint>>(serializedAccessPoints);
        }

        public static string DeserializeBearerToken(string response)
        {
            return JsonConvert.DeserializeObject<AuthResponse>(response).JsonWebToken;
        }

        public static string SerializeAuthCredentials(string email, string password)
        {
            return JsonConvert.SerializeObject(new AuthRequestLogin { Email = email, Password = password });
        }
    }
}