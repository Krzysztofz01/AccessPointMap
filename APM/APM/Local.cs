using System.Collections.Generic;
using System.IO;
using Xamarin.Essentials;
using Newtonsoft.Json;
using Android.OS;

namespace APM
{
    class Local
    {
        private static string fileName = "accesspoints.json";
        private static string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

        public static void saveToFile(List<AccessPoint> accessPoints)
        {
            StreamWriter writer = new StreamWriter(filePath, true);
            System.Diagnostics.Debug.WriteLine(filePath);

            using (writer)
            {
                writer.WriteLine(JsonConvert.SerializeObject(accessPoints));
            }

            using (var streamReader = new StreamReader(filePath))
            {
                string content = streamReader.ReadToEnd();
                System.Diagnostics.Debug.WriteLine(content);
            }

        }

        public static void saveToSdCard(List<AccessPoint> accessPoints)
        {
            var sdCardPath = Environment.ExternalStorageDirectory.Path;
            var sdCardFilePath = Path.Combine(sdCardPath, "Accesspoints.json");

            using (StreamWriter writer = new StreamWriter(sdCardFilePath, true))
            {
                writer.WriteLine(JsonConvert.SerializeObject(accessPoints));
            }

        }
    }
}