using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AccessPointMapApp.Models;
using Android.OS;

namespace AccessPointMapApp.Services
{
    class StorageService
    {
        private SerializationService serializationService;

        public StorageService()
        {
            this.serializationService = new SerializationService();
        }

        public async Task SaveLocalFileToJson(List<Accesspoint> accesspoints)
        {
            string fileName = "ApmData" + System.DateTime.Now.ToString() + ".json";
            string filePath = Path.Combine(Environment.ExternalStorageDirectory.Path, fileName);

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                await writer.WriteLineAsync(serializationService.SerializeAccessPointContainer(accesspoints));
            }
        }
    }
}