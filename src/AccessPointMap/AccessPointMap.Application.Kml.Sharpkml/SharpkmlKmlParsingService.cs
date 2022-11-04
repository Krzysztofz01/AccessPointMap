using AccessPointMap.Application.Core;
using AccessPointMap.Application.Kml.Core;
using AccessPointMap.Domain.AccessPoints;
using Microsoft.Extensions.Logging;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Kml.Sharpkml
{
    internal sealed class SharpkmlKmlParsingService : IKmlParsingService
    {
        private readonly ILogger<SharpkmlKmlParsingService> _logger;

        private const string _redPinUrl = "http://maps.google.com/mapfiles/ms/icons/red.png";
        private const string _yellowPinUrl = "http://maps.google.com/mapfiles/ms/icons/yellow.png";
        private const string _greenPinUrl = "http://maps.google.com/mapfiles/ms/icons/green.png";
        private const string _bluePinUrl = "http://maps.google.com/mapfiles/ms/icons/red.png";

        private const string _pinRed = "RED";
        private const string _pinYellow = "YELLOW";
        private const string _pinGreen = "GREEN";
        private const string _pinBlue = "BLUE";

        public SharpkmlKmlParsingService(ILogger<SharpkmlKmlParsingService> logger)
        {
            _logger = logger ??
                throw new ArgumentNullException(nameof(logger));
        }

        public Task<Result<KmlResult>> GenerateKmlAsync(IEnumerable<AccessPoint> accessPoints, CancellationToken cancellationToken = default)
        {
            try
            {
                var accessPointsFolder = GenerateAccessPointPlacemarksFolder(accessPoints, cancellationToken);

                var document = new Document();

                foreach (var style in GenerateStyles()) document.AddStyle(style);

                document.AddFeature(accessPointsFolder);

                var kmlRoot = new SharpKml.Dom.Kml();
                kmlRoot.AddNamespacePrefix(KmlNamespaces.Kml22Prefix, KmlNamespaces.Kml22Namespace);
                kmlRoot.Feature = document;

                var kml = KmlFile.Create(kmlRoot, false);

                using var memoryFileStream = new MemoryStream();
                kml.Save(memoryFileStream);

                var kmlFile = KmlResult.FromBuffer(memoryFileStream.ToArray());
                return Task.FromResult(Result.Success(kmlFile));
            }
            catch (OperationCanceledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Application service: {ServiceName} failure.\n   {Exception}",
                    typeof(SharpkmlKmlParsingService).Name, ex);

                return Task.FromResult(Result.Failure<KmlResult>(KmlParserError.FatalError));
            }
        }

        private static Folder GenerateAccessPointPlacemarksFolder(IEnumerable<AccessPoint> accessPoints, CancellationToken cancellationToken = default)
        {
            var folder = new Folder
            {
                Name = "Access points",
                Description = new Description
                {
                    Text = "Access points collection exported from the AccessPointMap server."
                }
            };

            foreach (var accessPoint in accessPoints)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var accessPointPlacemark = GetPlacemarkFromAccessPoint(accessPoint);

                folder.AddFeature(accessPointPlacemark);
            }

            return folder;
        }

        private static Placemark GetPlacemarkFromAccessPoint(AccessPoint accessPoint)
        {
            var point = new Point
            {
                Coordinate = new Vector(
                    accessPoint.Positioning.HighSignalLatitude,
                    accessPoint.Positioning.HighSignalLongitude)
            };

            var placemark = new Placemark
            {
                Name = accessPoint.Ssid.Value,
                Description = GetPlacemarkDescription(accessPoint),
                StyleUrl = GetPlacemarkStyleId(accessPoint),
                Geometry = point
            };

            return placemark;
        }

        private static Description GetPlacemarkDescription(AccessPoint accessPoint)
        {
            var descriptionBuilder = new StringBuilder(string.Empty);

            descriptionBuilder.Append("AccessPointMapId: ");
            descriptionBuilder.Append(accessPoint.Id);
            descriptionBuilder.AppendLine(string.Empty);

            descriptionBuilder.Append("BSSID: ");
            descriptionBuilder.Append(accessPoint.Bssid.Value);
            descriptionBuilder.AppendLine(string.Empty);

            descriptionBuilder.Append("Capabilities: ");
            descriptionBuilder.Append(accessPoint.Security.RawSecurityPayload);
            descriptionBuilder.AppendLine(string.Empty);

            descriptionBuilder.Append("Timestamp: ");
            descriptionBuilder.Append(accessPoint.CreationTimestamp.Value);
            descriptionBuilder.AppendLine(string.Empty);

            descriptionBuilder.Append("Signal: ");
            descriptionBuilder.Append(accessPoint.Positioning.HighSignalLevel);
            descriptionBuilder.AppendLine(string.Empty);

            return new Description
            {
                Text = descriptionBuilder.ToString()
            };
        }

        private static Uri GetPlacemarkStyleId(AccessPoint accessPoint)
        {
            var securityStandards = JsonSerializer.Deserialize<List<string>>(accessPoint.Security.SecurityStandards);

            if (securityStandards.Contains("WPA3")) return new Uri($"#{_pinGreen}", UriKind.Relative);
            if (securityStandards.Contains("WPA2")) return new Uri($"#{_pinGreen}", UriKind.Relative);
            if (securityStandards.Contains("WPA")) return new Uri($"#{_pinYellow}", UriKind.Relative);
            if (securityStandards.Contains("WPS")) return new Uri($"#{_pinYellow}", UriKind.Relative);
            if (securityStandards.Contains("WEP")) return new Uri($"#{_pinRed}", UriKind.Relative);
            return new Uri($"#{_pinRed}", UriKind.Relative);
        }

        private static IEnumerable<Style> GenerateStyles()
        {
            return new List<Style>
            {
                RegisterStyle(_pinRed, _redPinUrl),
                RegisterStyle(_pinYellow, _yellowPinUrl),
                RegisterStyle(_pinGreen, _greenPinUrl),
                RegisterStyle(_pinBlue, _bluePinUrl)
            };
        }

        private static Style RegisterStyle(string styleId, string stylePath)
        {
            return new Style
            {
                Id = styleId,
                Icon = new IconStyle
                {
                    Icon = new IconStyle.IconLink(new Uri(stylePath, UriKind.Absolute))
                }
            };
        }
    }
}
