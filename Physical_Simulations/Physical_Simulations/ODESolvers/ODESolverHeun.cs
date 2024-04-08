namespace Physical_Simulations.ODESolvers
{
    class ODESolverHeun
    {
        public delegate double Function(double[] x, double t);

        public static double[] HeunMethod(Function[] f, double[] x0, double t0, double dt)
        {
            int n = x0.Length;
            double[] x1 = new double[n];
            double[] x = new double[n];

            for (int i = 0; i < n; i++)
            {
                x1[i] = x0[i] + dt * f[i](x0, t0);
            }

            for (int i = 0; i < n; i++)
            {
                double correctedDerivative = (f[i](x0, t0) + f[i](x1, t0 + dt)) / 2;
                x[i] = x0[i] + dt * correctedDerivative;
            }

            return x;
        }
    }
}
