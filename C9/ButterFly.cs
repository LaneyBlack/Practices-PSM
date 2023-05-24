using System;

namespace C9
{
    public class ButterFly
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }
        public double A { get; set; }
        public double B { get; set; }
        public double C { get; set; }

        public ButterFly(double s0, double a, double b, double c)
        {
            X = s0;
            Y = s0;
            Z = s0;
            A = a;
            B = b;
            C = c;
        }

        public void CalcNext(double dT)
        {
            double dX = (A * Y - A * X) * dT; // dX = (A*y - A*x) * dT
            double dY = (-X * Z + B * X - Y) * dT; // dY = (-x*z + B*x - y) * dT
            double dZ = (X * Y - C * Z) * dT; // dZ = (x*y - C*z) * dT
            X += dX;
            Y += dY;
            Z += dZ;
        }

        public void CalcNextMidPoint(double dT)
        {
            var halfX = X + (A * Y - A * X) * dT / 2; // in middle point
            var halfY = Y + (-X * Z + B * X - Y) * dT / 2;
            var halfZ = Z + (X * Y - C * Z) * dT / 2;

            X += (A * halfY - A * halfX) * dT;
            Y += (-halfX * halfZ + B * halfX - halfY) * dT;
            Z += (halfX * halfY - C * halfZ) * dT;
        }

        public void CalcNextRk4(double dT)
        {
            var k = new double[4][];
            k[0] = CalcDerivative(X, Y, Z); // Get the derivative to count next moves
            for (int i = 1; i < 4; i++)
            {
                var next = i == 3 // last time without dt/2 so i need an if here
                    ? CountNext(dT, k[i - 1][0], k[i - 1][1], k[i - 1][2])
                    : CountNext(dT / 2, k[i - 1][0], k[i - 1][1], k[i - 1][2]);
                k[i] = CalcDerivative(next[0], next[1], next[2]);
            }

            // Count sum's
            var kSum = new double[3];
            for (int i = 0; i < 4; i++)
            {
                int tmp = 1;
                if (i == 1 || i == 2)
                    tmp = 2;
                for (int j = 0; j < 3; j++)
                {
                    kSum[j] += k[i][j] * tmp;
                }
            }
            // Edit data
            X += kSum[0] / 6 * dT;
            Y += kSum[1] / 6 * dT;
            Z += kSum[2] / 6 * dT;
        }

        private double[] CalcDerivative(double x, double y, double z)
        {
            var derX = A * y - A * x;
            var derY = -x * z + B * x - y;
            var derZ = x * y - C * z;
            return new[] { derX, derY, derZ };
        }

        private double[] CountNext(double dT, double kx, double ky, double kz)
        {
            var nextX = X + kx * dT;
            var nextY = Y + ky * dT;
            var nextZ = Z + kz * dT;
            return new[] { nextX, nextY, nextZ };
        }
    }
}