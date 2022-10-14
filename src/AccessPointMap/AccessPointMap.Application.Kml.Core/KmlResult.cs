namespace AccessPointMap.Application.Kml.Core
{
    public sealed class KmlResult
    {
        public byte[] FileBuffer { get; init; }

        public static implicit operator byte[](KmlResult kmlResult) => kmlResult.FileBuffer;
    }
}
