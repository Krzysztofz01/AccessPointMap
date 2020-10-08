using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace APM
{
    class ApiHelper
    {
        private static HttpClient client = new HttpClient();
        private static readonly string baseApiUrl = "http://192.168.1.200:54805/";
        private static readonly string authEndpointPath = "projects/accesspointmap/auth";
        private static readonly string postEndpointPath = "projects/accesspointmap/api/AccessPoints";

        public ApiHelper()
        {
            client.BaseAddress = new Uri(baseApiUrl);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> send(List<AccessPoint> accessPoints, string Login, string Password)
        {
            var authResponse = await client.PostAsJsonAsync(authEndpointPath, new { Login, Password });

            if(authResponse.IsSuccessStatusCode)
            {
                var authResponseObject = await authResponse.Content.ReadAsStringAsync();
                var authResponseDefinition = new { token = "" };
                var tokenObject = JsonConvert.DeserializeAnonymousType(authResponseObject, authResponseDefinition);

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenObject.token.ToString());
                var accesspointResponse = await client.PostAsJsonAsync(postEndpointPath, accessPoints);

                if(accesspointResponse.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }

            return false;
        }
    }
}