using AccessPointMap.Application.Core;

namespace AccessPointMap.Application.Kml.Core
{
    public class KmlParserError : Error
    {
        protected KmlParserError() { }
        protected KmlParserError(string message) : base(message) { }

        public static KmlParserError FatalError => new("The KML format parser encountered an unexpected error.");
    }
}
