using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace APM.Services
{
    class ConnectionService
    {
        private static HttpClient Client;

        //Change this settings for production/developement
        private static readonly string BaseApiUrl = "http://192.168.1.100:54805/";
        private static readonly string AuthEndpointPath = "projects/accesspointmap/auth";
        private static readonly string PostEndpointPath = "projects/accesspointmap/api/AccessPoints";

        public ConnectionService()
        {
            Client = new HttpClient();
            Client.BaseAddress = new Uri(BaseApiUrl);
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> SendData(List<AccessPoint> accessPoints, string login, string password)
        {
            var token = await Auth(login, password);
            if(token != null)
            {
                Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var response = await Client.PostAsJsonAsync(PostEndpointPath, accessPoints);

                if (response.IsSuccessStatusCode) return true;
            }
            return false;
        }

        private async Task<string> Auth(string login, string password)
        {
            var authResponse = await Client.PostAsJsonAsync(AuthEndpointPath, new { login, password });
            if(authResponse.IsSuccessStatusCode)
            {
                var token = await authResponse.Content.ReadAsAsync<TokenModel>();
                return token.token;
            }
            return null;
        }
    }

    class TokenModel
    {
        public string token { get; set; }
    }
}