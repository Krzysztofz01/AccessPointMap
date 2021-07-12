using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AccessPointMapScanner.App.Models;
using Android.OS;

namespace AccessPointMapScanner.App.Utilities
{
    public static class StorageService
    {
        public static async Task SaveResultsToJsonLocal(IDictionary<string, AccessPoint> accessPointsDict)
        {
            string filename = $"APM_SCAN_{ System.DateTime.Now }.json";
            string path = Path.Combine(Environment.ExternalStorageDirectory.Path, filename);

            using (var sw = new StreamWriter(path, true))
            {
                await sw.WriteLineAsync(SerializationService.SerializeAccessPoints(accessPointsDict));
            }
        }
    }
}