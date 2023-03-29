namespace C3
{
    public class RotatePoint
    {
        public decimal[] S { get; set; }
        public decimal Omega { get; set; }
        public decimal Epsilon { get; set; }

        public decimal Alpha { get; set; }
        public decimal Mass { get; set; }

        public RotatePoint(decimal[] s, decimal omega, decimal mass, decimal alpha, int dimensions)
        {
            S = s;
            Mass = mass;
            Omega = omega;
            Epsilon = new decimal(0);
            Alpha = alpha;
        }

    }
}