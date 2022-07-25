using AccessPointMap.Application.Kml.Core;
using AccessPointMap.Domain.AccessPoints;
using AccessPointMap.Infrastructure.Core.Abstraction;
using Microsoft.EntityFrameworkCore;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccessPointMap.Application.Kml.Sharpkml
{
    public class SharpkmlKmlParsingService : IKmlParsingService
    {
        private readonly IDataAccess _dataAccess;

        private const string _redPinUrl = "http://maps.google.com/mapfiles/ms/icons/red.png";
        private const string _yellowPinUrl = "http://maps.google.com/mapfiles/ms/icons/yellow.png";
        private const string _greenPinUrl = "http://maps.google.com/mapfiles/ms/icons/green.png";
        private const string _bluePinUrl = "http://maps.google.com/mapfiles/ms/icons/red.png";

        public SharpkmlKmlParsingService(IDataAccess dataAccess)
        {
            _dataAccess = dataAccess ??
                throw new ArgumentNullException(nameof(dataAccess));
        }

        public async Task<KmlResult> GenerateKml(Action<KmlGenerationOptions> options)
        {
            try
            {
                var kmlOptions = new KmlGenerationOptions();
                options(kmlOptions);

                var accessPoints = !kmlOptions.IncludeHiddenAccessPoints
                    ? await _dataAccess.AccessPoints.Where(a => !a.DeletedAt.HasValue && a.DisplayStatus.Value).ToListAsync()
                    : await _dataAccess.AccessPoints.Where(a => !a.DeletedAt.HasValue).ToListAsync();

                var styles = GenerateStyles();

                var accessPointsFolder = GenerateAccessPointPlacemarksFolder(accessPoints);

                var document = new Document();

                foreach (var style in styles) document.AddStyle(style);

                document.AddChild(accessPointsFolder);

                var kml = KmlFile.Create(document, false);

                using var memoryFileStream = new MemoryStream();
                kml.Save(memoryFileStream);

                return new KmlResult
                {
                    FileBuffer = memoryFileStream.ToArray()
                };
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static Folder GenerateAccessPointPlacemarksFolder(IEnumerable<AccessPoint> accessPoints)
        {
            var folder = new Folder
            {
                Name = "Access points"
            };

            foreach (var accessPoint in accessPoints)
                folder.AddChild(GetPlacemarkFromAccessPoint(accessPoint));

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
            var description = new Description
            {
                Text = string.Empty
            };

            throw new NotImplementedException();
        }

        // Access via a style lookup table
        private static Uri GetPlacemarkStyleId(AccessPoint accessPoint)
        {
            throw new NotImplementedException();
        }

        private static IEnumerable<Style> GenerateStyles()
        {
            throw new NotImplementedException();
        }
    }
}
