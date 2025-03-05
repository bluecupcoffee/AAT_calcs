using System.Numerics;
using System.Reflection;

namespace Calculations;
using AAT_calcs;
class Program
{
    static void Main(string[] args)
    {
        
        // X Y Z coordinates of point
        var  tup = Mapping.MapToXyzTuple(45, 45);
        Console.WriteLine($"X:{tup.X} Y:{tup.Y} Z:{tup.Z}");
        // distance from origin to point
        var dist = Mapping.CartesianCoordinateDistance(tup.X, tup.Y, tup.Z, 0, 0, 0);
        Console.WriteLine($"Dist:{dist}");
        var v = Vector3.Normalize(new Vector3((float)tup.X, (float)tup.Y, (float)tup.Z));
        Console.WriteLine(v);

        
        // point to Z-axis
        var zIntersect = Mapping.Perpendicular3DVectorSolveFor2NdZ(tup.X, tup.Y, tup.Z);
        Console.WriteLine($"Z-Intersect vector from position: {zIntersect}");
        
        Vector3 pos = new Vector3((float)tup.X, (float)tup.Y, (float)tup.Z);
        var initialHeading = Mapping.Perpendicular3DVectorSolveFor2NdZ(tup.X, tup.Y, tup.Z);
        Console.WriteLine($"Heading:{initialHeading}");
        // magnitude to Z-intersect from point
        double magnitude = Mapping.CartesianCoordinateDistance(tup.X, tup.Y, tup.Z, 0, 0, zIntersect.Z);
        Console.WriteLine($"Magnitude:{magnitude}");
        var normalizedVector = Mapping.NormalizeVector(0 - tup.X, 0 - tup.Y, zIntersect.Z - tup.Z, magnitude);
        Console.WriteLine($"NormalizedVector:{normalizedVector}");
        
        
    }
}