namespace C6
{
    public class Point
    {
        public double[] S { get; set; } //coordinate
        public double Vy { get; set; } //velocity
        public double Ay { get; set; } //acceleration

        public Point(double y,double x, int dimension)
        {
            S = new double[dimension];
            Vy = 0;
            Ay = 0;
            S[0] = x;
            S[1] = y;
        }
    }
}