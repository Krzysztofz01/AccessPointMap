using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace APM
{
    class ApiHelper
    {
        private static HttpClient client = new HttpClient();

        public ApiHelper()
        {
            client.BaseAddress = new Uri("http://localhost/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<bool> send(List<AccessPoint> accessPoints)
        {
            bool success = true;

            for(int i=0; i<accessPoints.Count; i++)
            {
                var response = await client.PostAsJsonAsync("api/action/add.php", accessPoints[i]);
                
                if(!response.IsSuccessStatusCode)
                {
                    success = false;
                }
            }
            return success;
        }

        public async void getBssid(List<string> bssid)
        {
            //Get all bssid from database to compare 
        }
    }
}