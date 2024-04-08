using System;

namespace Physical_Simulations.Classes
{
    class ParallelProjection
    {
        private float projectionAngle;

        public ParallelProjection(float angleInDegrees)
        {
            projectionAngle = (float)(angleInDegrees * Math.PI / 180.0);
        }

        public System.Drawing.PointF Project(float x, float y, float z)
        {
            float alpha = projectionAngle;

            float newX = x + z * (float)Math.Cos(alpha);
            float newY = y + z * (float)Math.Sin(alpha);

            return new System.Drawing.PointF(newX, newY);
        }
    }
}
