using Microsoft.VisualStudio.TestTools.UnitTesting;
using Physical_Simulations.Classes;
using System.Drawing;

namespace Physical_Simulations_Tests
{
    [TestClass]
    public class CentralProjectionTests
    {
        [TestMethod]
        public void ConstructorWithNoArgumentsSetsDefaultValues()
        {
            float expectedDistance = 150;
            CentralProjection projection = new CentralProjection();
            Assert.AreEqual(expectedDistance, projection.ProjectionDistance, "The default projection distance should be set.");
        }

        [TestMethod]
        public void ConstructorWithCustomDistanceSetsDistanceCorrectly()
        {
            float customDistance = 200;
            CentralProjection projection = new CentralProjection(customDistance);
            Assert.AreEqual(customDistance, projection.ProjectionDistance, "The custom projection distance should be set correctly.");
        }

        [TestMethod]
        public void PerspectiveFromXAxisShouldProjectCorrectly()
        {
            CentralProjection projection = new CentralProjection(150);
            float x = 100, y = 50, z = 25;
            PointF expectedProjectedPoint = new PointF(projection.ProjectionDistance * y / x, projection.ProjectionDistance * z / x);

            PointF projectedPoint = projection.PerspectiveFromXAxis(x, y, z);

            Assert.AreEqual(expectedProjectedPoint.X, projectedPoint.X, 1e-6, "The projected Y coordinate is incorrect.");
            Assert.AreEqual(expectedProjectedPoint.Y, projectedPoint.Y, 1e-6, "The projected Z coordinate is incorrect.");
        }

        [TestMethod]
        public void PerspectiveFromYAxisShouldProjectCorrectly()
        {
            CentralProjection projection = new CentralProjection(150);
            float x = 50, y = 100, z = 25;
            PointF expectedProjectedPoint = new PointF(projection.ProjectionDistance * x / y, projection.ProjectionDistance * z / y);

            PointF projectedPoint = projection.PerspectiveFromYAxis(x, y, z);

            Assert.AreEqual(expectedProjectedPoint.X, projectedPoint.X, 1e-6, "The projected X coordinate is incorrect.");
            Assert.AreEqual(expectedProjectedPoint.Y, projectedPoint.Y, 1e-6, "The projected Z coordinate is incorrect.");
        }

        [TestMethod]
        public void PerspectiveFromZAxisShouldProjectCorrectly()
        {
            CentralProjection projection = new CentralProjection(150);
            float x = 100, y = 50, z = 25;
            PointF expectedProjectedPoint = new PointF(projection.ProjectionDistance * x / z, projection.ProjectionDistance * y / z);

            PointF projectedPoint = projection.PerspectiveFromZAxis(x, y, z);

            Assert.AreEqual(expectedProjectedPoint.X, projectedPoint.X, 1e-6, "The projected X coordinate is incorrect.");
            Assert.AreEqual(expectedProjectedPoint.Y, projectedPoint.Y, 1e-6, "The projected Y coordinate is incorrect.");
        }
    }
}
