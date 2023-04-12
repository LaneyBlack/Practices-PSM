using System;

namespace C5
{
    public class CelestialObject
    {
        public double[] S { get; set; } // km
        public double[] V { get; set; } // velocity km/s
        public double[] A { get; set; } // acceleration km/s^2
        public double M { get; set; } // mass [kg]
        

        public CelestialObject(double s, double m, double g, bool isSun)
        {
            M = m;
            S = new double[2];
            S[0] = 0;
            S[1] = s;
            if (isSun)
                return;
            V = new double[2];
            A = new double[2];
            V[0] = Math.Sqrt(g * m / s); // counting speed that suits the celestial
            V[1] = 0;
            A[0] = 0;
            A[1] = 0;
        }

        public override string ToString()
        {
            return Math.Round(S[0],3) + "-" + Math.Round(S[1],3);
        }
    }
}