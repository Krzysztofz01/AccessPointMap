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

        private const string baseAddress = "http://localhost/";
        private const string add = "api/action/add.php";
        private const string read = "api/action/read.php";

        public ApiHelper()
        {
            client.BaseAddress = new Uri(baseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));
        }

        //public async Task createorupdata()
        public async Task send(List<AccessPoint> accessPoints)
        {
            for(int i=0; i<accessPoints.Count; i++)
            {
                if(alreadyKnown(accessPoints[i].bssid))
                {
                    update(accessPoints[i]);   
                }
                else
                {
                    addNew(accessPoints[i]);
                }
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

        private async Task<bool> update(AccessPoint accessPoint)
        {
            return false;
        }

        public async Task getData(List<string> bssid)
        {
            var response = await client.GetAsync(read);

            if(!response.IsSuccessStatusCode)
            {
                //AccessPoint.knownBssid;
                //umieszczenie bssid obiekotw w liscie bssid
            }
        }

        private bool alreadyKnown(string bssid)
        {
            bool result = false;

            for(int i=0; i<AccessPoint.AccessPointKnown.Count; i++)
            {
                if(bssid == AccessPoint.AccessPointKnown[i].bssid)
                {
                    result = true;
                }
            }
            return result;
        }
    }
}