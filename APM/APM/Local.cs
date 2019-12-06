using System.Collections.Generic;
using System.IO;
using Xamarin.Essentials;
using Newtonsoft.Json;

namespace APM
{
    class Local
    {
        private static string fileName = "accesspoints.json";
        private static string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

        public static void saveToFile(List<AccessPoint> accessPoints)
        {
            StreamWriter writer = new StreamWriter(filePath);

            using(writer)
            {
                foreach(var ap in accessPoints)
                {
                    writer.WriteLine(JsonConvert.SerializeObject(ap));
                }
            }
        }
    }
}