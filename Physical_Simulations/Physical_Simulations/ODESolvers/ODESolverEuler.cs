namespace Physical_Simulations.ODESolvers
{
    class ODESolverEuler
    {
        public delegate double Function(double[] x, double t);

        public static double[] EulerMethod(Function[] f, double[] x0, double t0, double dt)
        {
            int n = x0.Length;
            double[] x = new double[n];
            System.Array.Copy(x0, x, n);

            double[] dx = new double[n];
            for (int i = 0; i < n; i++)
            {
                dx[i] = dt * f[i](x, t0);
            }

            for (int i = 0; i < n; i++)
            {
                x[i] += dx[i];
            }

            return x;
        }
    }
}
