using System.IO;

namespace AccessPointMap.Application.Integration.Wigle.Extensions
{
    internal static class StreamReaderExtensions
    {
        public static void SkipLine(this StreamReader streamReader)
        {
            _ = streamReader.ReadLine();
        }
    }
}
