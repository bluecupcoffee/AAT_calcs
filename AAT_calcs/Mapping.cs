namespace AAT_calcs;

// x = (N(∅) + h) * cos(∅) * cos(λ)
//
// y = (N(∅) + h) * cos(∅) * sin(λ)
//
// z = ((1 - e²)N(∅) + h) *sin(∅)

// a ( equatorial radius ) = 6378137.0 m
// b ( polar radius ) = 6356752.3 m
// e2 ( eccentricity ) = 1 - (b^2/a^2) => f = 1 - (b / a)

public static class Mapping
{
    private const double a = 6378137.0D;
    private const double b = 6356752.3D;
    private const double e = 0.08181921804834474711755146934483D;
    private const double eSquared = 0.00669438444204258280946884516695D;

    private static double PrimeVerticalRadius(double latRadians)
    {
        // N(∅) = a / sqrt(1 - ((e^2) / (1 + cot^2(∅)))
        double denominator = Math.Sqrt(1 - eSquared * Math.Pow(Math.Sin(latRadians), 2));
        return a / denominator;
    }

    // gps coordinates mapped to cartesian space with origin set at earth center of mass
    public static (double X, double Y, double Z) MapToXyzTuple(double pLat, double pLon, double heightMeters = 0)
    {
        // ∅ = latitude
        // λ = longitude
        double latRadians = pLat * Math.PI / 180.0;
        double lonRadians = pLon * Math.PI / 180.0;
        double pvr = PrimeVerticalRadius(latRadians);
        double x = (pvr + heightMeters)*Math.Cos(latRadians) * Math.Cos(lonRadians);
        double y = (pvr + heightMeters)*Math.Cos(latRadians) * Math.Sin(lonRadians);
        double z = (((1 - eSquared) * pvr) + heightMeters) * Math.Sin(latRadians);
        return (x, y, z);
    }

    // distance between 2 cartesian coordinates
    public static double CartesianCoordinateDistance(double ax, double ay, double az, double bx, double by, double bz)
    {
        return Math.Sqrt(Math.Pow(ax-bx, 2) + Math.Pow(ay-by, 2) + Math.Pow(az-bz, 2));
    }

    // distance between 2 Gps coordinates
    public static double GpsCoordinateDistance(double aLat, double aLon, double aH, double bLat, double bLon,
        double bH)
    {
        var aTuple = MapToXyzTuple(aLat, aLon, aH);
        var bTuple = MapToXyzTuple(bLat, bLon, bH);
        return CartesianCoordinateDistance(aTuple.X, aTuple.Y, aTuple.Z, bTuple.X, bTuple.Y, bTuple.Z);
    }
    
}