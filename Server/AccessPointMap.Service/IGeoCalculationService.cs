namespace AccessPointMap.Service
{
    public interface IGeoCalculationService
    {
        double GetDistance(double lat1, double lat2, double lon1, double lon2);
        double GetArea(double radius);
    }
}
