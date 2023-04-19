using System;

namespace C6
{
    public class Spring
    {
        public Point[] Points { get; set; }

        public Spring(double pullDistance, int numberOfPoints, int dimensions)
        {
            Points = new Point[numberOfPoints];
            var dx = Math.PI / numberOfPoints;
            Points[0] = new Point(0, dimensions);
            for (var i = 1; i < numberOfPoints; i++)
            {
                Points[i] = new Point(pullDistance * Math.Sin(Points[i - 1].S[0] + dx), dimensions);
            }
        }
    }
}