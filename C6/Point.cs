namespace C6
{
    public class Point
    {
        public double[] S { get; set; } //coordinate
        public double Vy { get; set; } //velocity
        public double Ay { get; set; } //acceleration

        public Point(double height, int dimension)
        {
            S = new double[dimension];
            Vy = -1;
            Ay = 0;
            for (var i = 0; i < dimension; i++)
            {
                S[i] = 0;
            }
            S[1] = height;
        }
    }
}