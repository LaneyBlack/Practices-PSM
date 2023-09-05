namespace C3
{
    public class Point
    {
        public decimal[] S { get; set; }
        public decimal[] V { get; set; }
        public decimal[] A { get; set; }

        public decimal M { get; set; }

        public Point(decimal s, decimal v, decimal[] a, decimal m, int dimensions)
        {
            S = new decimal[dimensions];
            V = new decimal[dimensions];
            A = new decimal[dimensions];
            M = m;
            for (int i = 0; i < dimensions; i++)
            {
                S[i] = s;
                V[i] = v;
                A[i] = a[i];
            }
        }
    }
}