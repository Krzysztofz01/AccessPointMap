using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Essentials;

namespace APM
{
    class Local
    {
        private const string fileName = "Accesspoints.json";
        private const string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

        public static void saveToFile(string json)
        {
            File.WriteAllText(filePath, json);
        }
    }
}