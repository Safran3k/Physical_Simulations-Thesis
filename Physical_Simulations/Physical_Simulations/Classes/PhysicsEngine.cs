using Physical_Simulations.Enums;
using Physical_Simulations.ODESolvers;
using System.Numerics;

namespace Physical_Simulations.Classes
{
    class PhysicsEngine
    {
        #region Fields
        double[] xx;
        double time;
        double dt;
        ODESolverRK4.Function[] F;
        Body body1;
        Body body2;
        Body body3;
        Vector3 forceBody1, forceBody2, forceBody3;
        float b1fx, b1fy, b1fz, b2fx, b2fy, b2fz, b3fx, b3fy, b3fz;
        SolversEnum solversEnum;
        ODESolverRK4.Function[] FRK4;
        ODESolverEuler.Function[] FE;
        ODESolverHeun.Function[] FHEUN;
        #endregion

        #region Getter/Setter
        public double Time { get => time; set => time = value; }
        public double Dt { get => dt; set => dt = value; }
        internal Body Body1 { get => body1; set => body1 = value; }
        internal Body Body2 { get => body2; set => body2 = value; }
        internal Body Body3 { get => body3; set => body3 = value; }
        public SolversEnum SolversEnum { get => solversEnum; set => solversEnum = value; }
        #endregion

        public PhysicsEngine(Body body1, Body body2, Body body3, double dt, SolversEnum solversEnum)
        {
            Body1 = body1;
            Body2 = body2;
            Body3 = body3;
            xx = new double[18]
            {
                body1.Position.X,
                body1.Position.Y,
                body1.Position.Z,
                body1.Velocity.X,
                body1.Velocity.Y,
                body1.Velocity.Z,
                body2.Position.X,
                body2.Position.Y,
                body2.Position.Z,
                body2.Velocity.X,
                body2.Velocity.Y,
                body2.Velocity.Z,
                body3.Position.X,
                body3.Position.Y,
                body3.Position.Z,
                body3.Velocity.X,
                body3.Velocity.Y,
                body3.Velocity.Z
            };
            Time = 0;
            Dt = dt;
            F = new ODESolverRK4.Function[18] { F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12, F13, F14, F15, F16, F17, F18 };
            SolversEnum = solversEnum;
            switch (SolversEnum)
            {
                case SolversEnum.Euler:
                    FE = new ODESolverEuler.Function[18] { F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12, F13, F14, F15, F16, F17, F18 };
                    break;
                case SolversEnum.Runge_Kutta_4:
                    FRK4 = new ODESolverRK4.Function[18] { F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12, F13, F14, F15, F16, F17, F18 };
                    break;
                case SolversEnum.Heun_Improved_Euler:
                    FHEUN = new ODESolverHeun.Function[18] { F1, F2, F3, F4, F5, F6, F7, F8, F9, F10, F11, F12, F13, F14, F15, F16, F17, F18 };
                    break;
                default:
                    break;
            }
        }

        private double F1(double[] xx, double t)
        {
            return xx[3];
        }

        private double F2(double[] xx, double t)
        {
            return xx[4];
        }

        private double F3(double[] xx, double t)
        {
            return xx[5];
        }

        private double F4(double[] xx, double t)
        {
            Vector3 forceFromB2 = NewtonsGravitationalLaw.CalculateForce(body1, body2);
            Vector3 forceFromB3 = NewtonsGravitationalLaw.CalculateForce(body1, body3);
            Vector3 totalForce = forceFromB2 + forceFromB3;
            b1fx = totalForce.X;
            return totalForce.X;
        }

        private double F5(double[] xx, double t)
        {
            Vector3 forceFromB2 = NewtonsGravitationalLaw.CalculateForce(body1, body2);
            Vector3 forceFromB3 = NewtonsGravitationalLaw.CalculateForce(body1, body3);
            Vector3 totalForce = forceFromB2 + forceFromB3;
            b1fy = totalForce.Y;
            return totalForce.Y;
        }

        private double F6(double[] xx, double t)
        {
            Vector3 forceFromB2 = NewtonsGravitationalLaw.CalculateForce(body1, body2);
            Vector3 forceFromB3 = NewtonsGravitationalLaw.CalculateForce(body1, body3);
            Vector3 totalForce = forceFromB2 + forceFromB3;
            b1fz = totalForce.Z;
            return totalForce.Z;
        }

        private double F7(double[] xx, double t)
        {
            return xx[9];
        }

        private double F8(double[] xx, double t)
        {
            return xx[10];
        }

        private double F9(double[] xx, double t)
        {
            return xx[11];
        }

        private double F10(double[] xx, double t)
        {
            Vector3 forceFromB1 = NewtonsGravitationalLaw.CalculateForce(body2, body1);
            Vector3 forceFromB3 = NewtonsGravitationalLaw.CalculateForce(body2, body3);
            Vector3 totalForce = forceFromB1 + forceFromB3;
            b2fx = totalForce.X;
            return totalForce.X;
        }

        private double F11(double[] xx, double t)
        {
            Vector3 forceFromB1 = NewtonsGravitationalLaw.CalculateForce(body2, body1);
            Vector3 forceFromB3 = NewtonsGravitationalLaw.CalculateForce(body2, body3);
            Vector3 totalForce = forceFromB1 + forceFromB3;
            b2fy = totalForce.Y;
            return totalForce.Y;
        }

        private double F12(double[] xx, double t)
        {
            Vector3 forceFromB1 = NewtonsGravitationalLaw.CalculateForce(body2, body1);
            Vector3 forceFromB3 = NewtonsGravitationalLaw.CalculateForce(body2, body3);
            Vector3 totalForce = forceFromB1 + forceFromB3;
            b2fz = totalForce.Z;
            return totalForce.Z;
        }

        private double F13(double[] xx, double t)
        {
            return xx[15];
        }

        private double F14(double[] xx, double t)
        {
            return xx[16];
        }

        private double F15(double[] xx, double t)
        {
            return xx[17];
        }

        private double F16(double[] xx, double t)
        {
            Vector3 forceFromB1 = NewtonsGravitationalLaw.CalculateForce(body3, body1);
            Vector3 forceFromB2 = NewtonsGravitationalLaw.CalculateForce(body3, body2);
            Vector3 totalForce = forceFromB1 + forceFromB2;
            b3fx = totalForce.X;
            return totalForce.X;
        }

        private double F17(double[] xx, double t)
        {
            Vector3 forceFromB1 = NewtonsGravitationalLaw.CalculateForce(body3, body1);
            Vector3 forceFromB2 = NewtonsGravitationalLaw.CalculateForce(body3, body2);
            Vector3 totalForce = forceFromB1 + forceFromB2;
            b3fy = totalForce.Y;
            return totalForce.Y;
        }

        private double F18(double[] xx, double t)
        {
            Vector3 forceFromB1 = NewtonsGravitationalLaw.CalculateForce(body3, body1);
            Vector3 forceFromB2 = NewtonsGravitationalLaw.CalculateForce(body3, body2);
            Vector3 totalForce = forceFromB1 + forceFromB2;
            b3fz = totalForce.Z;
            return totalForce.Z;
        }

        public double[] UpdateState()
        {
            Vector3 previousVelocityBody1 = body1.Velocity;
            Vector3 previousVelocityBody2 = body2.Velocity;
            Vector3 previousVelocityBody3 = body3.Velocity;

            double[] result = new double[18];

            switch (SolversEnum)
            {
                case SolversEnum.Euler:
                    result = ODESolverEuler.EulerMethod(FE, xx, Time, Dt);
                    break;
                case SolversEnum.Runge_Kutta_4:
                    result = ODESolverRK4.RungeKutta4(FRK4, xx, Time, Dt);
                    break;
                case SolversEnum.Heun_Improved_Euler:
                    result = ODESolverHeun.HeunMethod(FHEUN, xx, Time, Dt);
                    break;
                default:
                    break;
            }

            xx = result;
            Time += Dt;
            UpdateBodyState(result, previousVelocityBody1, previousVelocityBody2, previousVelocityBody3);
            return result;
        }

        private void UpdateBodyState(double[] result, Vector3 previousVelocityBody1, Vector3 previousVelocityBody2, Vector3 previousVelocityBody3)
        {
            body1.UpdatePosition(result[0], result[1], result[2]);
            body2.UpdatePosition(result[6], result[7], result[8]);
            body3.UpdatePosition(result[12], result[13], result[14]);

            forceBody1 = new Vector3(b1fx, b1fy, b1fz);
            forceBody2 = new Vector3(b2fx, b2fy, b2fz);
            forceBody3 = new Vector3(b3fx, b3fy, b3fz);

            body1.UpdateForce(forceBody1.X, forceBody1.Y, forceBody1.Z);
            body2.UpdateForce(forceBody2.X, forceBody2.Y, forceBody2.Z);
            body3.UpdateForce(forceBody3.X, forceBody3.Y, forceBody3.Z);

            body1.UpdateVelocity(result[3], result[4], result[5]);
            body2.UpdateVelocity(result[9], result[10], result[11]);
            body3.UpdateVelocity(result[15], result[16], result[17]);

            Vector3 accelerationBody1 = new Vector3(
                (float)((body1.Velocity.X - previousVelocityBody1.X) / Dt),
                (float)((body1.Velocity.Y - previousVelocityBody1.Y) / Dt),
                (float)((body1.Velocity.Z - previousVelocityBody1.Z) / Dt)
            );

            Vector3 accelerationBody2 = new Vector3(
                (float)((body2.Velocity.X - previousVelocityBody2.X) / Dt),
                (float)((body2.Velocity.Y - previousVelocityBody2.Y) / Dt),
                (float)((body2.Velocity.Z - previousVelocityBody2.Z) / Dt)
            );

            Vector3 accelerationBody3 = new Vector3(
                (float)((body3.Velocity.X - previousVelocityBody3.X) / Dt),
                (float)((body3.Velocity.Y - previousVelocityBody3.Y) / Dt),
                (float)((body3.Velocity.Z - previousVelocityBody3.Z) / Dt)
            );

            body1.Acceleration = accelerationBody1;
            body2.Acceleration = accelerationBody2;
            body3.Acceleration = accelerationBody3;
        }
    }
}
