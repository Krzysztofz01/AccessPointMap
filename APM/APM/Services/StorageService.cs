using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Widget;
using Newtonsoft.Json;

namespace APM.Services
{
    class StorageService
    {
        public StorageService()
        {

        }

        public async Task SaveJson(List<AccessPoint> accessPoints)
        {
            string fileName = "ApmData" + System.DateTime.Now.ToString() + ".json";
            string filePath = Path.Combine(Environment.ExternalStorageDirectory.Path, fileName);

            using(StreamWriter writer = new StreamWriter(filePath, true))
            {
                await writer.WriteLineAsync(JsonConvert.SerializeObject(accessPoints));
            }
        }
    }
}