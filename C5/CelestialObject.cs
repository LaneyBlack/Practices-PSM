using System;

namespace C5
{
    public class CelestialObject
    {
        public string Name { get; set; }
        public double[] S { get; set; } // km
        public double[] V { get; set; } // velocity km/s
        public double[] A { get; set; } // acceleration km/s^2
        public double M { get; set; } // mass [kg]


        public CelestialObject(double sFromParent, double m, CelestialObject parent, double g)
        {
            M = m;
            S = new double[2];
            S[0] = 0;
            V = new double[2];
            A = new double[2];
            V[1] = 0;
            A[0] = 0;
            A[1] = 0;
            if (parent != null)
            {
                V[0] = Math.Sqrt(g * parent.M / sFromParent); // counting speed for celestial to stay on orbit
                Console.WriteLine(V[0]);
                V[0] += parent.V[0];
                S[1] = parent.S[1] + sFromParent;
            }
            else
            {
                S[1] = 0;
                V[0] = 0;
            }
        }

        public CelestialObject(CelestialObject anotherObject)
        {
            // Make a true copy
            M = anotherObject.M;
            S = new double[2];
            V = new double[2];
            A = new double[2];
            for (var i = 0; i < 2; i++)
            {
                S[i] = anotherObject.S[i];
                V[i] = anotherObject.V[i];
                A[i] = anotherObject.A[i];
            }
        }

        public override string ToString()
        {
            return Math.Round(S[0], 3) + "-" + Math.Round(S[1], 3);
        }
    }
}