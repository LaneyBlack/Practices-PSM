namespace C9
{
    public class PhysicalObject
    {
        public double[] S { get; set; } //0-X, 1-Y, 2-Z ...
        public double[] V { get; set; }
        public double[] A { get; set; }

        public PhysicalObject(int dimensions)
        {
            S = new double[dimensions];
        }

        public void CalcNext(double dT)
        {
            for (int i = 0; i < S.Length; i++)
            {
                S[i] += V[i] * dT; // S(t) = S(0) + V(0) * dT
                V[i] += A[i] * dT;
                A[i] = 9.81;
            }
        }

        public void CalcNextMidPoint(double dT)
        {
            for (int i = 0; i < S.Length; i++)
            {
                var halfV = A[i] * dT / 2;
                var halfA = 9.81;

                var dS = halfV * dT;
                var dV = halfA * dT;

                S[i] += dS;
                V[i] += dV;
                A[i] = 9.81;
            }
        }

        public void CalcNextRk4(double dT)
        {
            //ToDo get this done
            var k = new double[4];
            double dTn = 0, sum = 0;
            for (int i = 0; i < 4; i++)
            {
                if (i != 2)
                    dTn += (dT / 2);
                var tN = i * dT;
                k[i] = dTn * V[i];
                if (i == 1 || i == 2)
                    sum += 2 * k[i];
                sum += k[i];
            }
            sum /= 6;
            sum *= dT;
            S[0] += sum;
        }
    }
}