using AccessPointMap.Domain.Core.Exceptions;
using AccessPointMap.Domain.Core.Models;
using System;

namespace AccessPointMap.Domain.AccessPoints
{
    public class AccessPointPositioning : ValueObject<AccessPointPositioning>
    {
        private const double _pi = 3.1415;

        public int LowSignalLevel { get; private set; }
        public double LowSignalLatitude { get; private set; }
        public double LowSignalLongitude { get; private set; }

        public int HighSignalLevel { get; private set; }
        public double HighSignalLatitude { get; private set; }
        public double HighSignalLongitude { get; private set; }

        public double SignalRadius { get; private set; }
        public double SignalArea { get; private set; }

        private AccessPointPositioning() { }
        private AccessPointPositioning(
            int lowSignalLevel,
            double lowSignalLatitude,
            double lowSignalLongitude,
            int highSignalLevel,
            double highSignalLatitude,
            double highSignalLongitude)
        {
            if (lowSignalLevel > highSignalLevel)
                throw new ValueObjectValidationException("Invalid signal level data.");

            LowSignalLevel = lowSignalLevel;
            HighSignalLevel = highSignalLevel;

            LowSignalLatitude = lowSignalLatitude;
            LowSignalLongitude = lowSignalLongitude;

            HighSignalLatitude = highSignalLatitude;
            HighSignalLongitude = highSignalLongitude;

            SignalRadius = CalculateSignalRadius(lowSignalLatitude, lowSignalLongitude, highSignalLatitude, highSignalLongitude);

            SignalArea = CalculateSignalArea(SignalRadius);
        }

        private static double CalculateSignalRadius(
            double lowSignalLatitude,
            double lowSignalLongitude,
            double highSignalLatitude,
            double highSignalLongitude)
        {
            double o1 = lowSignalLatitude * _pi / 180.0;
            double o2 = highSignalLatitude * _pi / 180.0;

            double so = (highSignalLatitude - lowSignalLatitude) * _pi / 180.0;
            double sl = (highSignalLongitude - lowSignalLongitude) * _pi / 180.0;

            double a = Math.Pow(Math.Sin(so / 2.0), 2.0) + Math.Cos(o1) * Math.Cos(o2) * Math.Pow(Math.Sin(sl / 2.0), 2.0);
            double c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));

            return Math.Round(6371e3 * c, 2);
        }

        private static double CalculateSignalArea(double signalRadius)
        {
            return Math.Round(_pi * Math.Pow(signalRadius, 2.0), 2);
        }

        public static AccessPointPositioning FromGpsAndRssi(
            int lowSignalLevel,
            double lowSignalLatitude,
            double lowSignalLongitude,
            int highSignalLevel,
            double highSignalLatitude,
            double highSignalLongitude) =>
            
            new AccessPointPositioning(
            lowSignalLevel,
            lowSignalLatitude,
            lowSignalLongitude,
            highSignalLevel,
            highSignalLatitude,
            highSignalLongitude);
    }
}
