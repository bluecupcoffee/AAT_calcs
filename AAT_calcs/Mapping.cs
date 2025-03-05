using System.Numerics;
using System.Runtime.CompilerServices;
using ExtendedNumerics;
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
    private const double ESquared = 0.00669438444204258280946884516695D;
    private static BigDecimal _e2 = 0.00669438444204258280946884516695;
    public static BigDecimal BDa = new BigDecimal(6378137.0);
    public static BigDecimal BDb = new BigDecimal(6356752.3);

    private static double PrimeVerticalRadius(double latRadians)
    {
        // N(∅) = a / sqrt(1 - ((e^2) / (1 + cot^2(∅)))
        double denominator = Math.Sqrt(1 - ESquared * Math.Pow(Math.Sin(latRadians), 2));
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
        double x = (pvr + heightMeters) * Math.Cos(latRadians) * Math.Cos(lonRadians);
        double y = (pvr + heightMeters) * Math.Cos(latRadians) * Math.Sin(lonRadians);
        double z = (((1 - ESquared) * pvr) + heightMeters) * Math.Sin(latRadians);
        return (x, y, z);
    }

    // distance between 2 cartesian coordinates
    public static double CartesianCoordinateDistance(double ax, double ay, double az, double bx, double by, double bz)
    {
        return Math.Sqrt(Math.Pow(ax - bx, 2) + Math.Pow(ay - by, 2) + Math.Pow(az - bz, 2));
    }

    // distance between 2 Gps coordinates
    public static double GpsCoordinateDistance(double aLat, double aLon, double aH, double bLat, double bLon,
        double bH)
    {
        var aTuple = MapToXyzTuple(aLat, aLon, aH);
        var bTuple = MapToXyzTuple(bLat, bLon, bH);
        return CartesianCoordinateDistance(aTuple.X, aTuple.Y, aTuple.Z, bTuple.X, bTuple.Y, bTuple.Z);
    }

    // perpendicular vector to prime vertical radius aligned north/south
    public static (double X, double Y, double Z) Perpendicular3DVectorSolveFor2NdZ(
        double x1, double y1, double z1, double x2 = 0, double y2 = 0
    )
    {
        // perpendicular dot product = 0
        // (x1 x x2) + (y1 x y2) + (z1 x z2) = 0
        // -(x1 x x2) - (y1 x y2) = (z1 x z2)
        if (x2 == 0) x2 = -x1;
        if (y2 == 0) y2 = -y1;
        double dotX = x1 * x2;
        double dotY = y1 * y2;
        double dotZ = -dotX - dotY;
        double z2 = dotZ / z1;
        return (-x1, -y1, z2);
    }

    public static (double X, double Y, double Z) NormalizeVector(double x, double y, double z, double magnitude)
    {
        return (x / magnitude, y / magnitude, z / magnitude);
    }
    
    

}