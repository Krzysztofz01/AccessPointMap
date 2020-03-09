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
        private const string add = "api/request/add.php";
        private int errorCount = 0;

        public ApiHelper()
        {
            client.BaseAddress = new Uri("http://accesspointmap.ct8.pl/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task send(List<AccessPoint> accessPoints)
        {
            foreach(AccessPoint element in accessPoints)
            {
                var response = await client.PostAsJsonAsync(add, element);

                if(!response.IsSuccessStatusCode)
                {
                    errorCount++;
                }
            }
        }
    }
}