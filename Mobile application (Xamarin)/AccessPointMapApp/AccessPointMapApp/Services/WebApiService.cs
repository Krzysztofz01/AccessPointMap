using AccessPointMapApp.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AccessPointMapApp.Services
{
    class WebApiService
    {
        //Connection settings change this for developement/production
        //===========================================================
        private readonly string BaseUrl = "http://192.168.1.100:3201";
        //===========================================================

        private static HttpClient Client;
        private SerializationService serializationService;
        private readonly string AuthEndpointUrl = "projects/accesspointmap/api/auth";
        private readonly string MasterPostEndpointUrl = "projects/accesspointmap/api/master";
        private readonly string QueuePostEndpointUrl = "projects/accesspointmap/api/queue";

        public WebApiService()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(BaseUrl);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            serializationService = new SerializationService();
        }

        public async Task<string> AuthRequest(string login, string password)
        {
            var stringContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("Login", login),
                new KeyValuePair<string, string>("Password", password)
            });

            var authResponse = await Client.PostAsync(AuthEndpointUrl, stringContent);
            if(authResponse.IsSuccessStatusCode)
            {
                return serializationService.DeserializeBearerToken(await authResponse.Content.ReadAsStringAsync());
            }
            return null;
        }

        public async Task<bool> PostQueueAccessPoints(List<Accesspoint> accesspointsContainer, string bearerToken)
        {
            var accesspointsJson = serializationService.SerializeAccessPointContainer(accesspointsContainer);
            var data = new StringContent(accesspointsJson, Encoding.UTF8, "application/json");
            
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = data,
                RequestUri = new Uri(QueuePostEndpointUrl)
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer " + bearerToken);

            var response = await Client.SendAsync(request);
            if(response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> PostMasterAccessPoints(List<Accesspoint> accesspointsContainer, string bearerToken)
        {
            var accesspointsJson = serializationService.SerializeAccessPointContainer(accesspointsContainer);
            var data = new StringContent(accesspointsJson, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = data,
                RequestUri = new Uri(MasterPostEndpointUrl)
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer " + bearerToken);

            var response = await Client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}