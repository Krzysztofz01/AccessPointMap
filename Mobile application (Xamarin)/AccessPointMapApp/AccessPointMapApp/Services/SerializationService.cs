using AccessPointMapApp.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AccessPointMapApp.Services
{
    class SerializationService
    {
        public string SerializeAccessPointContainer(List<Accesspoint> accessPointContainer)
        {
            return JsonConvert.SerializeObject(accessPointContainer);
        }

        public List<Accesspoint> DeserializeAccessPointContainer(string accessPointContainerString)
        {
            return JsonConvert.DeserializeObject<List<Accesspoint>>(accessPointContainerString);
        }

        public string DeserializeBearerToken(string response)
        {
            return JsonConvert.DeserializeObject<LoginResponse>(response).bearerToken;
        }

        public string SerializeAuthCredentials(string login, string password)
        {
            return JsonConvert.SerializeObject(new { Email = login, Password = password });
        }
    }
}