using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace AccessPointMapScanner.App.Utilities
{
    public static class WebService
    {
        //Server strings
        private static readonly string urlDev = "http://192.168.1.100:5000";
        private static readonly string urlProd = "";
        public static readonly string website = "http://www.192.168.1.100:5000";


        private static HttpClient client;

        private static readonly string authEndpoint = "api/v1/auth";
        private static readonly string apEndpoint = "api/v1/accesspoint";
        private static readonly string apMasterEndpoint = "api/v1/accesspoint/master";

        public static async Task<bool> SendResults(string serializedAccessPoints, string email, string password, bool master)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri(urlDev);
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if(await Authenticate(email, password))
            {
                if(await SendData(serializedAccessPoints, master))
                {
                    return true;
                }
            }
            return false;
        }

        private static async Task<bool> Authenticate(string email, string password)
        {
            var credentials = SerializationService.SerializeAuthCredentials(email, password);
            var data = new StringContent(credentials, Encoding.UTF8, "application/json");

            var authResponse = await client.PostAsync(authEndpoint, data);

            if(authResponse.IsSuccessStatusCode)
            {
                string token = SerializationService.DeserializeBearerToken(await authResponse.Content.ReadAsStringAsync());
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                return true;
            }
            return false;
        }

        private static async Task<bool> SendData(string serializedAccessPoints, bool master)
        {
            var data = new StringContent(serializedAccessPoints, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                Content = data,
                RequestUri = (master) ? new Uri(apMasterEndpoint) : new Uri(apEndpoint)
            };

            var response = await client.SendAsync(request);

            if(response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}