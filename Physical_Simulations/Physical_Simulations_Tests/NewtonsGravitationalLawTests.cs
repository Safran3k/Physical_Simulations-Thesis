using Microsoft.VisualStudio.TestTools.UnitTesting;
using Physical_Simulations.Classes;
using System.Numerics;
using System.Windows.Media;

namespace Physical_Simulations_Tests
{
    [TestClass]
    public class NewtonsGravitationalLawTests
    {
        [TestMethod]
        public void GravitationalConstantDefaultValueCorrect()
        {
            double expected = 6.6743 * 10e-11 * 10e11;
            double actual = NewtonsGravitationalLaw.GravitationalConstant;
            Assert.AreEqual(expected, actual, delta: 0.0001, "The default value of the gravity constant is incorrect.");
        }

        [TestMethod]
        public void GravitationalConstantSetNewValueValueUpdated()
        {
            double newValue = 6.6743 * 10e-10 * 10e11;
            NewtonsGravitationalLaw.GravitationalConstant = newValue;
            Assert.AreEqual(newValue, NewtonsGravitationalLaw.GravitationalConstant, delta: 0.0001, "The gravity constant has not been updated.");
            NewtonsGravitationalLaw.GravitationalConstant = 6.6743 * 10e-11 * 10e11;
        }

        [TestMethod]
        public void CalculateForceBetweenTwoBodiesShouldMatchExpectedValue()
        {
            Body body = new Body(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), Colors.Blue, 1000.0);
            Body neighbor = new Body(new Vector3(100, 0, 0), new Vector3(100, 0, 0), new Vector3(100, 0, 0), Colors.LightGray, 5000.0);

            float distance = Vector3.Distance(body.Position, neighbor.Position);
            float expectedForceMagnitude = (float)(NewtonsGravitationalLaw.GravitationalConstant * body.Mass * neighbor.Mass / System.Math.Pow(distance, 2));

            Vector3 force = NewtonsGravitationalLaw.CalculateForce(body, neighbor);
            float actualForceMagnitude = force.Length();

            Assert.AreEqual(expectedForceMagnitude, actualForceMagnitude, 1e-5, "The calculated gravity force is incorrect.");
        }
    }
}
