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

        private const string baseAddress = "http://accesspointmap.ct8.pl/";
        private const string add = "api/actions/add.php";
        private const string read = "api/actions/read.php";
        private const string updataExisting = "api/actions/update.php";

        public ApiHelper()
        {
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task send(List<AccessPoint> accessPoints)
        {
            for (int i=0; i<accessPoints.Count; i++)
            {   
                    System.Diagnostics.Debug.WriteLine("Send");
                    await addNew(accessPoints[i]);
            }
        }

        private async Task<bool> addNew(AccessPoint accessPoint)
        {   
            var response = await client.PostAsJsonAsync(add, accessPoint);

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
        }   
    }
}