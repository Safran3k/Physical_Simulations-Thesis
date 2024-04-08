using System.Collections.Generic;

namespace Physical_Simulations.Classes
{
    public class BodyContainer
    {
        List<Body> bodies;
        public IEnumerable<Body> Bodies => bodies.AsReadOnly();
        public int PlacedBodiesCount { get; private set; } = 0;

        public int Count => bodies.Count;

        public BodyContainer()
        {
            bodies = new List<Body>();
        }

        public void AddBody(Body body)
        {
            if (body == null)
            {
                throw new System.ArgumentNullException(nameof(body), "The body can't be null.");
            }
            bodies.Add(body);
            PlacedBodiesCount++;
        }

        public bool RemoveBody(Body body)
        {
            bool wasRemoved = bodies.Remove(body);
            if (wasRemoved)
            {
                PlacedBodiesCount = System.Math.Max(0, PlacedBodiesCount - 1);
            }
            return wasRemoved;
        }

        public Body this[int index]
        {
            get { return bodies[index]; }
            set { bodies[index] = value; }
        }

        public void Clear()
        {
            bodies.Clear();
            PlacedBodiesCount = 0;
        }
    }
}
