using Microsoft.VisualStudio.TestTools.UnitTesting;
using Physical_Simulations.Classes;
using System.Numerics;
using System.Windows.Media;

namespace Physical_Simulations_Tests
{
    [TestClass]
    public class BodyContainerTests
    {
        [TestMethod]
        public void ConstructorInitializesEmptyContainer()
        {
            BodyContainer container = new BodyContainer();

            Assert.IsNotNull(container.Bodies);
            Assert.AreEqual(0, container.PlacedBodiesCount);
            Assert.AreEqual(0, container.Count);
        }

        [TestMethod]
        public void AddBodyIncreasesPlacedBodiesCount()
        {
            BodyContainer container = new BodyContainer();
            Body bodyToAdd = new Body(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), Colors.White);

            container.AddBody(bodyToAdd);

            Assert.AreEqual(1, container.PlacedBodiesCount);
            Assert.AreEqual(1, container.Count);
            Assert.AreSame(bodyToAdd, container[0]);
        }

        [TestMethod]
        public void RemoveBodyDecreasesPlacedBodiesCountAndRemovesSpecificBody()
        {
            BodyContainer container = new BodyContainer();
            Body bodyToAdd = new Body(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), Colors.White);
            Body bodyToRemove = new Body(new Vector3(1, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, 1), Colors.Red);
            container.AddBody(bodyToAdd);
            container.AddBody(bodyToRemove);

            container.RemoveBody(bodyToRemove);

            Assert.AreEqual(1, container.PlacedBodiesCount);
            Assert.IsTrue(container[0] == bodyToAdd, "The remaining body should be bodyToAdd.");
            Assert.AreEqual(1, container.Count);
        }

        [TestMethod]
        public void ClearRemovesAllBodiesAndResetsPlacedBodiesCount()
        {
            BodyContainer container = new BodyContainer();
            Body body1 = new Body(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), Colors.White);
            Body body2 = new Body(new Vector3(1, 1, 1), new Vector3(1, 1, 1), new Vector3(1, 1, 1), Colors.Red);
            container.AddBody(body1);
            container.AddBody(body2);

            container.Clear();

            Assert.AreEqual(0, container.PlacedBodiesCount, "PlacedBodiesCount should be reset to 0.");
            Assert.AreEqual(0, container.Count, "All bodies should be removed from the container.");
        }

        [TestMethod]
        [ExpectedException(typeof(System.ArgumentOutOfRangeException))]
        public void IndexerWhenAccessedWithInvalidIndexThrowsArgumentOutOfRangeException()
        {
            BodyContainer container = new BodyContainer();
            Body body = new Body(new Vector3(0, 0, 0), new Vector3(0, 0, 0), new Vector3(0, 0, 0), Colors.White);
            container.AddBody(body);

            Body result = container[-1];
        }
    }
}
