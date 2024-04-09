using System;
using System.Drawing;

namespace Physical_Simulations.Classes
{
    public class CentralProjection
    {
        float projectionDistance = 150;

        public float ProjectionDistance { get => projectionDistance; set => projectionDistance = value; }

        public CentralProjection() : this(150)
        {

        }

        public CentralProjection(float projectionDistance)
        {
            ProjectionDistance = projectionDistance;
        }

        public PointF PerspectiveFromXAxis(float x, float y, float z)
        {
            if (Math.Abs(x) < 1e-6)
            {
                x = 1e-6f;
            }
            float Y = ProjectionDistance * y / x;
            float Z = ProjectionDistance * z / x;
            return new PointF(Y, Z);
        }

        public PointF PerspectiveFromYAxis(float x, float y, float z)
        {
            if (Math.Abs(y) < 1e-6)
            {
                y = 1e-6f;
            }
            float X = ProjectionDistance * x / y;
            float Z = ProjectionDistance * z / y;
            return new PointF(X, Z);
        }

        public PointF PerspectiveFromZAxis(float x, float y, float z)
        {
            if (Math.Abs(z) < 1e-6)
            {
                z = 1e-6f;
            }
            float X = ProjectionDistance * x / z;
            float Y = ProjectionDistance * y / z;
            return new PointF(X, Y);
        }
    }
}
