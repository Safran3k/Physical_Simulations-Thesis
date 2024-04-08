using System.Numerics;

namespace Physical_Simulations.Classes
{
    public static class NewtonsGravitationalLaw
    {
        public static double GravitationalConstant { get; set; } = 6.6743 * 10e-11 * 10e11;

        public static Vector3 CalculateForce(Body body, Body neighbor)
        {
            Vector3 direction = neighbor.Position - body.Position;
            float distance = Vector3.Distance(body.Position, neighbor.Position);
            float forceMagnitude = (float)(GravitationalConstant * body.Mass * neighbor.Mass / System.Math.Pow(distance, 2));
            Vector3 force = Vector3.Normalize(direction) * forceMagnitude;

            return force;
        }
    }
}
