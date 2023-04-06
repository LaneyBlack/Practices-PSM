using System.Threading;

namespace C4
{
    public class Ball
    {
        public decimal[] S { get; set; } //S[0] = x, S[1] = y, S[2] = z (coordinates)
        public decimal V { get; set; } // Velocity
        public decimal A { get; set; } // Acceleration

        public decimal[] SRot { get; set; } // coordinates rotated by Alpha angle (hill angle) 

        // this way I can count coordinates change like it's on flat surface (with Forces)
        public decimal Omega { get; set; } // angle Velocity
        public decimal Eps { get; set; } // Epsilon (angle acceleration)
        public decimal R { get; } // Radius
        public decimal I { get; } // Inertia
        public decimal M { get; } // Mass

        public Ball(decimal y, decimal r, decimal m, int dimensions, bool isBall)
        {
            SRot = new decimal[dimensions];
            S = new decimal[dimensions];
            R = new decimal();
            R = r;
            Omega = 0;
            Eps = 0;
            V = 0;
            A = 0;
            S[0] = r;
            SRot[0] = r;
            S[1] = y;
            SRot[1] = r;
            M = m;
            I = new decimal();
            I = isBall ? 2 * M * R * R / 5 : 2 * M * R * R / 3; // 2/5 for ball and 2/3 for
        }
    }
}