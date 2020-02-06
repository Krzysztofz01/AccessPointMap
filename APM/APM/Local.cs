using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using Android.OS;

namespace APM
{
    class Local
    {
        public static void saveToDeviceLight(List<AccessPoint> accessPoints)
        {
            string fileName = "ApmLight" + System.DateTime.Now.ToString("HHmmss") + ".json";
            string filePath = Path.Combine(Environment.ExternalStorageDirectory.Path, fileName);

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(JsonConvert.SerializeObject(accessPoints));
            }

        }

        public static void saveToDeviceHard(AccessPoint accessPoint)
        {
            string fileName = "ApmHard.txt";
            string filePath = Path.Combine(Environment.ExternalStorageDirectory.Path, fileName);

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine(JsonConvert.SerializeObject(accessPoint));
            }

        }
    }
}