using System;
using System.IO;

namespace C3
{
    internal class Program
    {
        // private const int Dimensions = 2;
        private static readonly decimal G = new decimal(-9.81);
        private static readonly decimal Dt = new decimal(0.01); //delta t
        private static readonly decimal K = new decimal(0.01); //wind
        private static readonly decimal L = new decimal(1); // rope length in meters

        public static void Main(string[] args)
        {
            ClearFileCsv();
            //Euler-----------------------------------------------------------------------------------------------------
            SetHeaderCsv();
            var alpha = new decimal(Math.PI / 180 * 45);
            var point = new RotatePoint( new []
            {
                L * (decimal)Math.Cos((double)(alpha - (decimal)(Math.PI*90/180))), // x = L*cos(alpha-90^) 
                L * (decimal)Math.Sin((double)(alpha - (decimal)(Math.PI*90/180)))  // y = L*sin(alpha-90^)
            }, 0, new decimal(2), alpha, 2);
            for (decimal t = 0; t < 2; t += Dt)
            {
                ExportCsv(t, point);
                CountEuler(point, Dt);
            }
            ExportCsv(2, point);
            //BetterEuler-----------------------------------------------------------------------------------------------------
            SetHeaderCsv();
        }

        private static void CountEuler(RotatePoint point, decimal dT)
        {
            point.Epsilon = (point.Mass * G - K * point.Omega * L) / L *
                            (decimal)Math.Sin((double)point.Alpha); // E = ((mg-kwl)/l) * sin(A)
            var nextAlpha = point.Alpha + point.Omega * dT;
            var nextOmega = point.Omega + point.Epsilon * dT;
            point.Omega = nextOmega;
            point.Alpha = nextAlpha;
            point.S[0] = L * (decimal)Math.Cos((double)(nextAlpha - (decimal)(Math.PI*90/180))); // x = L*cos(alpha-90^) 
            point.S[1] = L * (decimal)Math.Sin((double)(nextAlpha - (decimal)(Math.PI*90/180))); // y = L*sin(alpha-90^)
        }

        public static Tuple<decimal, decimal> CountDerivative(decimal alpha, decimal omega, int k)
        {
            var ret = new Tuple<decimal, decimal>(0, 0);
            return ret;
        }

        public static void CountBetterEuler(RotatePoint point, decimal dT)
        {
            
        }

        private static void ClearFileCsv()
        {
            File.WriteAllText("C:\\PJATK\\4th\\PSM\\CwiczeniaPSM\\C3\\Data\\exp.csv", "");
        }

        private static void SetHeaderCsv()
        {
            File.AppendAllText("C:\\PJATK\\4th\\PSM\\CwiczeniaPSM\\C3\\Data\\exp.csv", "\n\nTime,Aplha,Sx,Sy,Ep,Ek,Ec\n");
        }

        private static void ExportCsv(decimal t, RotatePoint point)
        {
            var Ep = Math.Abs(point.Mass * G * (point.S[1] + L));
            var Ek = point.Mass * (decimal)Math.Pow((double)(point.Omega * L), 2) / 2;
            File.AppendAllText("C:\\PJATK\\4th\\PSM\\CwiczeniaPSM\\C3\\Data\\exp.csv", t + ","
                + Math.Round(point.Alpha * 1000) / 1000 + ","
                + Math.Round(point.S[0] * 1000) / 1000 + "," + Math.Round(point.S[1] * 1000) / 1000 +
                "," + Math.Round(Ep * 1000) / 1000 + "," + Math.Round(Ek * 1000) / 1000 + "," +
                Math.Round((Ep + Ek) * 1000) / 1000 + "\n"); // t x y Ep Ex Ec
        }
    }
}