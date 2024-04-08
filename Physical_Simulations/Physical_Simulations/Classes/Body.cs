using System;
using System.Numerics;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Physical_Simulations.Classes
{
    public class Body
    {
        #region Fields
        Vector3 position;
        Vector3 velocity;
        Vector3 force;
        double mass;
        Ellipse representation;
        System.Windows.Media.Color color;
        Polyline trajectory;
        Path forceVector;
        static float defaultInitialSize = 15;
        Vector3 acceleration;
        #endregion

        #region Getter/Setter
        public Vector3 Position { get => position; set => position = value; }
        public Vector3 Velocity { get => velocity; set => velocity = value; }
        public Vector3 Force { get => force; set => force = value; }
        public double Mass { get => mass; set => mass = value; }
        public Ellipse Representation { get => representation; set => representation = value; }
        public System.Windows.Media.Color Color { get => color; set => color = value; }
        public Polyline Trajectory { get => trajectory; set => trajectory = value; }
        public Path ForceVector { get => forceVector; private set => forceVector = value; }
        public static float DefaultInitialSize { get => defaultInitialSize; set => defaultInitialSize = value; }
        public Vector3 Acceleration { get => acceleration; set => acceleration = value; }
        #endregion


        public Body(Vector3 position, Vector3 velocity, Vector3 force, System.Windows.Media.Color color, double mass = 200)
        {
            Position = position;
            Velocity = velocity;
            Force = force;
            Mass = mass;
            Color = color;
            Acceleration = new Vector3(0, 0, 0);
            InitializeRepresentation(color);
        }

        private void InitializeRepresentation(System.Windows.Media.Color color)
        {
            Representation = new Ellipse
            {
                Width = 15,
                Height = 15,
                Fill = new SolidColorBrush(color)
            };

            Trajectory = new Polyline
            {
                Stroke = new SolidColorBrush(color),
                StrokeThickness = 1
            };

            ForceVector = new Path
            {
                Stroke = new SolidColorBrush(color),
                StrokeThickness = 2,
                Fill = new SolidColorBrush(color)
            };
        }

        public void DrawOrbitLine(System.Windows.Point projectedPosition)
        {
            Trajectory.Points.Add(new System.Windows.Point(projectedPosition.X, projectedPosition.Y));
        }

        public void DrawForceVectorArrow()
        {
            const float MaxForceLength = 100;
            const float MinForceLength = 20;
            Vector3 scaledForce = Vector3.Normalize(Force) * Math.Min(Math.Max(Force.Length(), MinForceLength), MaxForceLength);

            PathGeometry geometry = new PathGeometry();
            PathFigure figure = new PathFigure();
            figure.StartPoint = new System.Windows.Point(Position.X + Representation.Width / 2, Position.Y + Representation.Height / 2);

            Vector lineEnd = new Vector(scaledForce.X, scaledForce.Y);
            figure.Segments.Add(new LineSegment(figure.StartPoint + lineEnd, true));
            System.Windows.Point endPoint = new System.Windows.Point(figure.StartPoint.X + lineEnd.X, figure.StartPoint.Y + lineEnd.Y);

            double arrowHeadLength = 10;
            double arrowHeadWidth = 10;
            Vector arrowDirection = endPoint - figure.StartPoint;
            arrowDirection.Normalize();
            Vector arrowNorm = new Vector(-arrowDirection.Y, arrowDirection.X) * arrowHeadWidth;

            System.Windows.Point arrowBase = endPoint - arrowDirection * arrowHeadLength;
            PointCollection points = new PointCollection
            {
                endPoint,
                arrowBase + arrowNorm / 2,
                arrowBase - arrowNorm / 2,
                endPoint
            };

            PolyLineSegment arrowHead = new PolyLineSegment(points, true);
            figure.Segments.Add(arrowHead);


            geometry.Figures.Add(figure);
            ForceVector.Data = geometry;
        }

        public void UpdateVelocity(double vx, double vy, double vz)
        {
            Vector3 newVelocity = new Vector3((float)vx, (float)vy, (float)vz);
            Velocity = newVelocity;
        }

        public void UpdateForce(double fx, double fy, double fz)
        {
            Vector3 newForce = new Vector3((float)fx, (float)fy, (float)fz);
            Force = newForce;
        }

        public void UpdatePosition(double x, double y, double z)
        {
            Vector3 newPosition = new Vector3((float)x, (float)y, (float)z);
            Position = newPosition;
        }
    }
}
