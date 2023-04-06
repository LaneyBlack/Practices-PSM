using System;

namespace C4
{
    public class BallEdgePoint
    {
        public decimal[] S { get; set; } //S[0] = x, S[1] = y, S[2] = z (coordinates)
        public decimal R { get; set; } // radius
        public double Beta { get; set; } // angle in radians
        public decimal Omega { get; set; } // angle velocity
        public decimal Eps { get; set; } // Epsilon (angle acceleration)

        public BallEdgePoint(double beta, decimal r, int dimensions, Ball ball)
        {
            Beta = beta;
            R = r;
            Omega = 0;
            Eps = 0;
            S = new decimal[dimensions];
            S[0] = ball.S[0] + ball.R * (decimal)Math.Cos(Math.PI / 2 - Beta);
            S[1] = ball.S[1] + ball.R * (decimal)Math.Sin(Math.PI / 2 - Beta);
        }
    }
}