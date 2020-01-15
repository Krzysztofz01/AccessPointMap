using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Android.OS;

namespace APM
{
    class Local
    {
        private static string fileName = "Accesspoints.json";
        private static string filePath = Path.Combine(Environment.ExternalStorageDirectory.Path, fileName);

        public static void saveToSdCard(List<AccessPoint> accessPoints)
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(JsonConvert.SerializeObject(accessPoints));
            }

        }
    }
}