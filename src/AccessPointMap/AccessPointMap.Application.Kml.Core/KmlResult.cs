using AccessPointMap.Application.Abstraction;

namespace AccessPointMap.Application.Kml.Core
{
    public sealed class KmlResult : ExportFile
    {
        private KmlResult(byte[] fileBuffer) : base(fileBuffer) { }

        public static new KmlResult FromBuffer(byte[] buffer) => new(buffer);
    }
}
