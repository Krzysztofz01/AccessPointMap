using AccessPointMap.Domain.AccessPoints;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccessPointMap.Application.GeoJson.Core
{
    public sealed class AccessPointGeoJsonFeatureCollection
    {
        public string Type { get; } = "FeatureCollection";
        public IEnumerable<AccessPointGeoJsonFeature> Features { get; set; }

        public static AccessPointGeoJsonFeatureCollection Create(IEnumerable<AccessPoint> accessPoints)
        {
            return new AccessPointGeoJsonFeatureCollection
            {
                Features = accessPoints.Select(ap => AccessPointGeoJsonFeature.Create(ap))
            };
        }

        private AccessPointGeoJsonFeatureCollection() =>
            Features = new List<AccessPointGeoJsonFeature>();          
    }

    public sealed class AccessPointGeoJsonFeature
    {
        public string Type { get; } = "Feature";
        public AccessPointGeoJsonFeatureGeometry Geometry { get; set; }
        public AccessPointGeoJsonFeatureProperties Properties { get; set; }

        public static AccessPointGeoJsonFeature Create(AccessPoint accessPoint)
        {
            var geometry = new AccessPointGeoJsonFeatureGeometry()
            {
                Coordinates = new List<double>()
                {
                    accessPoint.Positioning.HighSignalLongitude,
                    accessPoint.Positioning.HighSignalLatitude
                }
            };

            var properties = new AccessPointGeoJsonFeatureProperties()
            {
                Id = accessPoint.Id,
                Bssid = accessPoint.Bssid,
                Manufacturer = accessPoint.Manufacturer,
                Ssid = accessPoint.Ssid,
                DeviceType = accessPoint.DeviceType,
                CreationTimestamp = accessPoint.CreationTimestamp,
                SignalArea = accessPoint.Positioning.SignalArea,
                SignalRadius = accessPoint.Positioning.SignalRadius,
                SecurityStandards = accessPoint.Security.SecurityStandards,
                SecurityProtocols = accessPoint.Security.SecurityProtocols,
                IsSecure = accessPoint.Security.IsSecure,
                IsPresent = accessPoint.Presence,
                RunIdentifier = accessPoint.RunIdentifier
            };

            return new AccessPointGeoJsonFeature()
            {
                Geometry = geometry,
                Properties = properties
            };
        }

        private AccessPointGeoJsonFeature()
        {            
        }
    }

    public sealed class AccessPointGeoJsonFeatureGeometry
    {
        public string Type { get; } = "Point";
        public IEnumerable<double> Coordinates { get; set; }

        internal AccessPointGeoJsonFeatureGeometry() =>
            Coordinates = new List<double>();
    }

    public sealed class AccessPointGeoJsonFeatureProperties
    {
        public Guid Id { get; set; }
        public string Bssid { get; set; }
        public string Manufacturer { get; set; }
        public string Ssid { get; set; }
        public string DeviceType { get; set; }
        public DateTime CreationTimestamp { get; set; }
        public double SignalArea { get; set; }
        public double SignalRadius { get; set; }
        public string SecurityStandards { get; set; }
        public string SecurityProtocols { get; set; }
        public bool IsSecure { get; set; }
        public bool IsPresent { get; set; }
        public Guid? RunIdentifier { get; set; }

        internal AccessPointGeoJsonFeatureProperties()
        {
        }
    }
}
