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
            var response = await client.PostAsJsonAsync(updataExisting, new {
            bssid = accessPoint.bssid,
            signalLevel = accessPoint.signalLevel,
            latitude = accessPoint.latitude,
            longitude = accessPoint.longitude});
            
            if(!response.IsSuccessStatusCode)
            {
                return false;
            }

            return true;
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