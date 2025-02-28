using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using NUnit.Framework;

namespace AAT_calcs;

[TestFixture]
public class CoordinateTests
{
    [Test]
    [TestCase(0, 0, 0, 6378137.0, 0.0, 0.0)]
    [TestCase(90, 90, 0, 0.0, 0.0, 6356752.3)]
    [TestCase(-90, -0, 0, 0.0, 0.0, -6356752.3)]
    [TestCase(35.6895, 139.6917, 10, -3954850.4855, 3354942.189, 3700270.608)]
    [TestCase(45.555, 100.0909, 30, -783853.6869, 4404581.265, 4530773.044)]
    [TestCase(45.555, 100.0909, 20, -783852.4600, 4404574.371, 4530765.904)]

    public void ConvertToECEF_Test(double latitude, double longitude, double altitude, double expectedX,
        double expectedY, double expectedZ)
    {
        var (x, y, z) = Mapping.MapToXyzTuple(latitude, longitude, altitude);
        Assert.That(x, Is.EqualTo(expectedX).Within(1E-1));
        Assert.That(y, Is.EqualTo(expectedY).Within(1E-1));
        Assert.That(z, Is.EqualTo(expectedZ).Within(1E-1));
        
    }

    [Test]
    [TestCase(-783853.6869, 4404581.265, 4530773.044, -783852.4600, 4404574.371, 4530765.904, 10)]
    [TestCase(-783853.6869, 4404581.265, 4530773.044, -3954850.4855, 3354942.189, 3700270.608, 3441903.1336)]
    [TestCase(0, 0, 0, 6378137.0, 0, 0, 6378137.0)]
    [TestCase(0, 0, 0, 0, 0, 6356752.3, 6356752.3)]
    [TestCase(0, 0, 0, 0, 0, -6356752.3, 6356752.3)]
    public void CartesianDistance_Test(double ax, double ay, double az, double bx, double by, double bz, double expectedDist)
    {
        double dist = Mapping.CartesianCoordinateDistance(ax, ay, az, bx, by, bz);
        Assert.That(dist, Is.EqualTo(expectedDist).Within(1E-1));
    }

    [Test]
    [TestCase(35.6895, 139.6917, 10, 45.555, 100.0909, 30, 3441903.1336)]
    [TestCase(90, 90, 0, -90, 90, 0, 12713504.6)]
    public void GpsCoordinateDistance_Test(double aLat, double aLon, double aH, double bLat, double bLon, double bH,
        double expectedDist)
    {
        double dist = Mapping.GpsCoordinateDistance(aLat, aLon, aH, bLat, bLon, bH);
        Assert.That(dist, Is.EqualTo(expectedDist).Within(1E-1));
    }

    [Test]
    [TestCase(1, 0, 5, 10, 3, -2)]
    [TestCase(-783853.6869, 4404581.265, 4530773.044, 783853.6869, -4404581.265, 4417516.0680)]
    public void Perpendicular3DVectorZAligned_Test(double x1, double y1, double z1, double x2, double y2, double expectedZ)
    {
        var res = Mapping.Perpendicular3DVectorSolveFor2NdZ(x1, y1, z1, x2, y2);
        Assert.That(res.Z, Is.EqualTo(expectedZ).Within(1E-1));
    }
}