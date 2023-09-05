using System;

namespace C6
{
    public class Spring
    {
        public Point[] Points { get; set; }

        public double Dx { get; set; }

        public Spring(double pullDistance, int numberOfPoints, int dimensions)
        {
            Dx = Math.PI / numberOfPoints;
            if (numberOfPoints % 2 == 0)
                numberOfPoints++;
            Points = new Point[numberOfPoints];
            double y = 0;
            Points[0] = new Point(0, 0, dimensions);
            for (var i = 1; i < numberOfPoints; i++)
            {
                y += Dx;
                Points[i] = new Point(pullDistance * Math.Sin(y), i, dimensions);
            }
        }

        public void CountNextEuler(double dT)
        {
            for (var i = 1; i < Points.Length - 1; i++)
                Points[i].Ay = (Points[i + 1].S[1] - 2 * Points[i].S[1] + Points[i - 1].S[1]) / Math.Pow(Dx, 2);
            //A = (Y(i+1) - 2*Y(i) + Y(i-1)) / Dy^2

            for (var i = 1; i < Points.Length - 1; i++)
            {
                Points[i].S[1] += Points[i].Vy * dT; //S=S(0) + V(0)*dT
                Points[i].Vy += Points[i].Ay * dT; //V(t)=V(0) + A(0)*dT
            }
        }

        public void CountNextMidPoint(double dT)
        {
            var aYMid = new double[Points.Length];
            var vYMid = new double[Points.Length];
            var sYMid = new double[Points.Length];
            sYMid[0] = 0;
            sYMid[Points.Length-1] = 0;
            //Acceleration in current point
            for (var i = 1; i < Points.Length - 1; i++)
            {
                var curPoint = Points[i];
                //Acceleration
                curPoint.Ay = (Points[i + 1].S[1] - 2 * curPoint.S[1] + Points[i - 1].S[1]) / Math.Pow(Dx, 2);
                //A = (Y(i+1) - 2*Y(i) + Y(i-1)) / Dy^2
                sYMid[i] = curPoint.S[1] + curPoint.Vy * dT / 2; // S(dt/2) = S(0) + V(0)*dt/2
                vYMid[i] = curPoint.Vy + curPoint.Ay * dT / 2; // V(dt/2) = V(0) + A(0)*dt/2
            }

            for (var i = 1; i < Points.Length - 1; i++)
            {
                aYMid[i] = (sYMid[i + 1] - 2 * sYMid[i] + sYMid[i - 1]) / Math.Pow(Dx, 2);
                //A = (Y(i+1) - 2*Y(i) + Y(i-1)) / Dy^2
                //Coordinate
                Points[i].S[1] += vYMid[i] * dT; //S(t) = S(0) + V(dt/2) * dT
                //Velocity
                Points[i].Vy += aYMid[i] * dT; //V(t)=V(0) + A(dt/2) * dT
            }
        }
    }
}