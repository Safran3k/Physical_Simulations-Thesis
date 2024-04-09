using Microsoft.VisualStudio.TestTools.UnitTesting;
using Physical_Simulations.Classes;
using System.Numerics;
using System.Windows;
using System.Windows.Media;

namespace Physical_Simulations_Tests
{
    [TestClass]
    public class BodyTests
    {
        [TestMethod]
        public void Body_ConstructorInitializesPropertiesCorrectly()
        {
            Vector3 expectedPosition = new Vector3(100, 100, 0);
            Vector3 expectedVelocity = new Vector3(10, 0, 0);
            Vector3 expectedForce = new Vector3(0, -9.81f, 0);
            Color expectedColor = Colors.Blue;
            double expectedMass = 200;

            Body body = new Body(expectedPosition, expectedVelocity, expectedForce, expectedColor, expectedMass);

            Assert.AreEqual(expectedPosition, body.Position, "Position is not initialized correctly.");
            Assert.AreEqual(expectedVelocity, body.Velocity, "Velocity is not initialized correctly.");
            Assert.AreEqual(expectedForce, body.Force, "Force is not initialized correctly.");
            Assert.AreEqual(expectedColor, body.Color, "Color is not initialized correctly.");
            Assert.AreEqual(expectedMass, body.Mass, "Mass is not initialized correctly.");
        }

        [TestMethod]
        public void UpdateVelocityWithNewVelocityUpdatesVelocityCorrectly()
        {
            Vector3 initialPosition = new Vector3(0, 0, 0);
            Vector3 initialVelocity = new Vector3(0, 0, 0);
            Vector3 initialForce = new Vector3(0, 0, 0);
            Color color = Colors.Red;
            double mass = 1.0;
            Body body = new Body(initialPosition, initialVelocity, initialForce, color, mass);

            Vector3 newVelocity = new Vector3(10, 10, 10);
            body.UpdateVelocity(newVelocity.X, newVelocity.Y, newVelocity.Z);
            Assert.AreEqual(newVelocity, body.Velocity, "Velocity was not updated correctly.");
        }

        [TestMethod]
        public void UpdateForceWithNewForceUpdatesForceCorrectly()
        {
            Vector3 initialPosition = new Vector3(0, 0, 0);
            Vector3 initialVelocity = new Vector3(0, 0, 0);
            Vector3 initialForce = new Vector3(0, 0, 0);
            Color color = Colors.Red;
            double mass = 1.0;
            Body body = new Body(initialPosition, initialVelocity, initialForce, color, mass);

            Vector3 newForce = new Vector3(5, 5, 5);
            body.UpdateForce(newForce.X, newForce.Y, newForce.Z);
            Assert.AreEqual(newForce, body.Force, "Force was not updated correctly.");
        }

        [TestMethod]
        public void UpdatePositionWithNewPositionUpdatesPositionCorrectly()
        {
            Vector3 initialPosition = new Vector3(0, 0, 0);
            Vector3 initialVelocity = new Vector3(0, 0, 0);
            Vector3 initialForce = new Vector3(0, 0, 0);
            Color color = Colors.Red;
            double mass = 1.0;
            Body body = new Body(initialPosition, initialVelocity, initialForce, color, mass);

            Vector3 newPosition = new Vector3(10, 10, 10);
            body.UpdatePosition(newPosition.X, newPosition.Y, newPosition.Z);
            Assert.AreEqual(newPosition, body.Position, "Position was not updated correctly.");
        }

        [TestMethod]
        public void DrawOrbitLineAddsNewPointToTrajectory()
        {
            Body body = new Body(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), Colors.Red, 1.0);
            Point pointToAdd = new Point(1, 1);

            body.DrawOrbitLine(pointToAdd);
            Assert.IsTrue(body.Trajectory.Points.Contains(pointToAdd), "The point was not added to the trajectory.");
        }
    }
}
