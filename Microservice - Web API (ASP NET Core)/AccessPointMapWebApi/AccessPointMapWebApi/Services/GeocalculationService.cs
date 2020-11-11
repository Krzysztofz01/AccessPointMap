using System;

namespace AccessPointMapWebApi.Services
{
    public interface IGeocalculationService
    {
        public double getDistance(double lat1, double lon1, double lat2, double lon2);
        public double getArea(double radius);
    }

    public class GeocalculationService : IGeocalculationService
    {
        //Haversine Formula algorithm source: www.movable-type.co.uk/scripts/latlong.html
        private const double PI = 3.1415;
        private const double R = 6371e3;

        public double getArea(double radius)
        {
            return Math.Round(PI * Math.Pow(radius, 2.0), 2);
        }

        public double getDistance(double lat1, double lon1, double lat2, double lon2)
        {  
            double o1 = lat1 * PI / 180.0;
            double o2 = lat2 * PI / 180.0;
            double so = (lat2 - lat1) * PI / 180.0;
            double sl = (lon2 - lon1) * PI / 180.0;
            double a = Math.Pow(Math.Sin(so / 2.0), 2.0) + Math.Cos(o1) * Math.Cos(o2) * Math.Pow(Math.Sin(sl / 2.0), 2.0);
            double c = 2.0 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1.0 - a));
            return R * c;
        }
    }
}
