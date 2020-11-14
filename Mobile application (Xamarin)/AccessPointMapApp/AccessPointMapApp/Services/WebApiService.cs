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
        private readonly string AuthEndpointUrl = "/projects/accesspointmap/api/auth/login";
        private readonly string MasterPostEndpointUrl = "/projects/accesspointmap/api/accesspoints/master";
        private readonly string QueuePostEndpointUrl = "/projects/accesspointmap/api/accesspoints/queue";

        public WebApiService()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(BaseUrl);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            serializationService = new SerializationService();
        }

        public async Task<bool> AuthRequest(string login, string password)
        {
            var credentialsJson = serializationService.SerializeAuthCredentials(login, password);
            var data = new StringContent(credentialsJson, Encoding.UTF8, "application/json");

            var authResponse = await Client.PostAsync(AuthEndpointUrl, data);
            System.Diagnostics.Debug.WriteLine(authResponse.StatusCode);
            if(authResponse.IsSuccessStatusCode)
            {
                string token = serializationService.DeserializeBearerToken(await authResponse.Content.ReadAsStringAsync());
                //Client.DefaultRequestHeaders.Add("Authorization ", $"Bearer {token}");
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return true;
            }
            return false;
        }

        public async Task<bool> PostQueueAccessPoints(List<Accesspoint> accesspointsContainer)
        {
            var accesspointsJson = serializationService.SerializeAccessPointContainer(accesspointsContainer);
            var data = new StringContent(accesspointsJson, Encoding.UTF8, "application/json");
            
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = data,
                RequestUri = new Uri(QueuePostEndpointUrl)
            };

            var response = await Client.SendAsync(request);
            if(response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> PostMasterAccessPoints(List<Accesspoint> accesspointsContainer)
        {
            var accesspointsJson = serializationService.SerializeAccessPointContainer(accesspointsContainer);
            var data = new StringContent(accesspointsJson, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = data,
                RequestUri = new Uri(MasterPostEndpointUrl)
            };

            var response = await Client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}