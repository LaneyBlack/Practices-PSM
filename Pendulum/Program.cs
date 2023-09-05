using System;
using System.IO;

namespace C3
{
    internal static class Program
    {
        // private const int Dimensions = 2;
        private static readonly decimal G = new decimal(-9.81);
        private const string FilePath = "C:\\PJATK\\4th\\PSM\\CwiczeniaPSM\\Pendulum\\Data\\exp.csv";
        private static readonly decimal Dt = new decimal(0.01); //delta t
        private static readonly decimal K = new decimal(0.1); //wind
        private static readonly decimal L = new decimal(1); // rope length in meters
        private static readonly decimal T = new decimal(4);

        public static void Main(string[] args)
        {
            ClearFileCsv();
            var watch = System.Diagnostics.Stopwatch.StartNew();
            //Euler-----------------------------------------------------------------------------------------------------
            SetHeaderCsv();
            var alpha = new decimal(Math.PI / 180 * 45);
            var point = new RotatePoint(new[]
            {
                L * (decimal)Math.Cos((double)(alpha - (decimal)(Math.PI * 90 / 180))), // x = L*cos(alpha-90^) 
                L * (decimal)Math.Sin((double)(alpha - (decimal)(Math.PI * 90 / 180))) // y = L*sin(alpha-90^)
            }, 0, new decimal(2), alpha, 2);
            for (decimal t = 0; t <= T; t += Dt)
            {
                CountEuler(point, Dt);
                ExportCsv(t, point);
            }

            //BetterEuler-----------------------------------------------------------------------------------------------
            SetHeaderCsv();
            alpha = new decimal(Math.PI / 180 * 45);
            point = new RotatePoint(new[]
            {
                L * (decimal)Math.Cos((double)(alpha - (decimal)(Math.PI * 90 / 180))), // x = L*cos(alpha-90^) 
                L * (decimal)Math.Sin((double)(alpha - (decimal)(Math.PI * 90 / 180))) // y = L*sin(alpha-90^)
            }, 0, new decimal(2), alpha, 2);
            for (decimal t = 0; t <= T; t += Dt)
            {
                CountBetterEuler(point, Dt);
                ExportCsv(t, point);
            }

            //RK4-------------------------------------------------------------------------------------------------------
            SetHeaderCsv();
            alpha = new decimal(Math.PI / 180 * 45);
            point = new RotatePoint(new[]
            {
                L * (decimal)Math.Cos((double)(alpha - (decimal)(Math.PI * 90 / 180))), // x = L*cos(alpha-90^) 
                L * (decimal)Math.Sin((double)(alpha - (decimal)(Math.PI * 90 / 180))) // y = L*sin(alpha-90^)
            }, new decimal(0), new decimal(2), alpha, 2);
            for (decimal t = 0; t <= T; t += Dt)
            {
                CountRK4(point, Dt);
                ExportCsv(t, point);
            }

            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);
            Console.WriteLine(watch.Elapsed);
        }

        private static void CountEuler(RotatePoint point, decimal dT)
        {
            var grav = point.Mass * G * (decimal)Math.Sin((double)point.Alpha); // Fg
            point.Epsilon = (grav - K * L * point.Omega) / (L * point.Mass); // E = ((mg * sin(A)-kwl)/lm)
            var nextAlpha = point.Alpha + point.Omega * dT; // a = a0 + w0*t 
            var nextOmega = point.Omega + point.Epsilon * dT; // w = w0 + e0*t
            point.Omega = nextOmega;
            point.Alpha = nextAlpha;
            point.S[0] =
                L * (decimal)Math.Cos((double)(nextAlpha - (decimal)(Math.PI * 90 / 180))); // x = L*cos(alpha-90^) 
            point.S[1] =
                L * (decimal)Math.Sin((double)(nextAlpha - (decimal)(Math.PI * 90 / 180))); // y = L*sin(alpha-90^)
        }

        private static void CountBetterEuler(RotatePoint point, decimal dT)
        {
            var grav = point.Mass * G * (decimal)Math.Sin((double)point.Alpha);
            point.Epsilon = (grav - K * L * point.Omega) / (L * point.Mass); // E = ((mg * sin(A)-kwl)/lm)
            var omegaHalf = point.Omega + point.Epsilon * dT; // W(to+dt/2)
            var nextAlpha = point.Alpha + omegaHalf * dT;
            var alphaHalf = point.Alpha + point.Omega * dT / 2;
            grav = point.Mass * G * (decimal)Math.Sin((double)alphaHalf); //Fg
            var epsilonHalf =
                (grav - K * L * omegaHalf) /
                (L * point.Mass); // E = ((mg * sin(a)-kwl)/lm) --- a here is in point t + dT/2
            var nextOmega = point.Omega + epsilonHalf * dT; //omega
            point.Omega = nextOmega;
            point.Alpha = nextAlpha;
            point.S[0] =
                L * (decimal)Math.Cos((double)(nextAlpha - (decimal)(Math.PI * 90 / 180))); // x = L*cos(alpha-90^) 
            point.S[1] =
                L * (decimal)Math.Sin((double)(nextAlpha - (decimal)(Math.PI * 90 / 180))); // y = L*sin(alpha-90^)
        }

        private static void CountRK4(RotatePoint point, decimal dT)
        {
            var k = new decimal[4][];
            for (int i = 0; i < 4; i++)
                k[i] = new decimal[2];
            var derivative = CalcDerivative(point.Alpha, point.Omega, point);
            Tuple<decimal, decimal> next;
            k[0][0] = derivative.Item1;
            k[0][1] = derivative.Item2;
            for (int i = 1; i < 4; i++)
            {
                next = i == 3
                    ? CalcNext(point.Alpha, k[i - 1][0], point.Omega, k[i - 1][1], dT)
                    : CalcNext(point.Alpha, k[i - 1][0], point.Omega, k[i - 1][1], dT / 2);
                derivative = CalcDerivative(next.Item1, next.Item2, point);
                k[i][0] = derivative.Item1;
                k[i][1] = derivative.Item2;
            }

            point.Alpha += (k[0][0] + 2 * k[1][0] + 2 * k[2][0] + k[3][0]) / 6 * dT;
            point.Omega += (k[0][1] + 2 * k[1][1] + 2 * k[2][1] + k[3][1]) / 6 * dT;
            var grav = point.Mass * G * (decimal)Math.Sin((double)point.Alpha);
            point.Epsilon = (grav - K * L * point.Omega) / (L * point.Mass); // E = ((mg * sin(A)-kwl)/lm)
            point.S[0] =
                L * (decimal)Math.Cos((double)(point.Alpha - (decimal)(Math.PI * 90 / 180))); // x = L*cos(alpha-90^) 
            point.S[1] =
                L * (decimal)Math.Sin((double)(point.Alpha - (decimal)(Math.PI * 90 / 180))); // y = L*sin(alpha-90^)
        }

        private static Tuple<decimal, decimal> CalcDerivative(decimal alpha, decimal omega, RotatePoint point)
        {
            var grav = point.Mass * G * (decimal)Math.Sin((double)alpha);
            var epsilon = (grav - K * L * omega) / (L * point.Mass); // E = ((mg * sin(A)-kwl)/lm)
            return new Tuple<decimal, decimal>(omega, epsilon);
        }

        private static Tuple<decimal, decimal> CalcNext(decimal alpha, decimal kAlpha, decimal omega, decimal kOmega,
            decimal dT)
        {
            var nextKAlpha = alpha + kAlpha * dT;
            var nextKOmega = omega + kOmega * dT;
            return new Tuple<decimal, decimal>(nextKAlpha, nextKOmega);
        }

        private static void ExportCsv(decimal t, RotatePoint point)
        {
            var Ep = Math.Abs(point.Mass * G * (point.S[1] + L));
            var Ek = point.Mass * (point.Omega * point.Omega * L * L) / 2;
            File.AppendAllText(FilePath,
                $"{t},{Math.Round(point.Alpha, 3)},{Math.Round(point.S[0], 3)},{Math.Round(point.S[1], 3)}," +
                $"{Math.Round(Ep, 3)},{Math.Round(Ek, 3)},{Math.Round(Ep + Ek, 3)}\n"); // t x y Ep Ek Ec
        }

        private static void ClearFileCsv()
        {
            File.WriteAllText(FilePath, "");
        }

        private static void SetHeaderCsv()
        {
            File.AppendAllText(FilePath,
                "\n\nTime,Alpha,Sx,Sy,Ep,Ek,Ec\n");
        }
    }
}