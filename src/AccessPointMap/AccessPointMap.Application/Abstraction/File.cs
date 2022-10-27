using System.IO;

namespace AccessPointMap.Application.Abstraction
{
    public class ExportFile
    {
        private readonly byte[] _fileBuffer;
        public byte[] FileBuffer => _fileBuffer;

        protected ExportFile() { }
        protected ExportFile(byte[] fileBuffer) => 
            _fileBuffer = fileBuffer;

        public static ExportFile FromBuffer(byte[] buffer) => new(buffer);
        public static ExportFile FromMemoryStream(MemoryStream stream) => new(stream.ToArray());

        public static implicit operator byte[](ExportFile exportFile) => exportFile.FileBuffer;
    }
}
