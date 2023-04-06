    using System;

namespace PSM_C1
{
    internal static class Program
    {
        private static double Sin(double x, int n)
        {
            x %= (2 * Math.PI);
            // if less than Math.PI than x = x else
            if (x > Math.PI / 2 && x <= Math.PI)
            {
                x = Math.PI - x;
            }
            else if (x > Math.PI && x <= Math.PI * 3 / 2)
            {
                x -= Math.PI;
            }
            else if (x > Math.PI * 3 / 2)
            {
                x = Math.PI * 2 - x;
            }

            double sinApprox = 0;
            for (var i = 0; i < n; i++)
            {
                var sign = (int)Math.Pow(-1, i); // in taylor of sin it is 
                var val1 = Math.Pow(x, 2 * i + 1); // numerator
                var val2 = Factorial(2 * i + 1); // denumerator
                sinApprox += sign * (val1 / val2); // as in the formula sing (numerator/denumerator)
            }

            return sinApprox;
        }

        private static long Factorial(int n)
        {
            if (n == 0 || n == 1)
            {
                return 1;
            }

            return n * Factorial(n - 1);
        }

        public static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Wrong arguments!");
                Console.WriteLine("Help:\n" +
                                  "Degrees: PSM_C1.exe number deg\n" +
                                  "         PSM_C1.exe number degrees\n" +
                                  "Radians: PSM_C1.exe number rad\n" +
                                  "         PSM_C1.exe number radians\n");
                return;
            }

            var x = double.Parse(args[0]?.Trim() ?? // angle in degrees
                                 throw new InvalidOperationException()); //if null throw exception
            if (args[1] == "deg" || args[1] == "degrees")
            {
                x *= Math.PI / 180; // changing the angle from degrees into radians
            }

            const int n = 10; // number of Taylor approximates
            Console.WriteLine("Calculation Accuracy\tReal sinus\t\t\tTaylor sinus\t\t\tApproximationError");
            for (var i = 1; i <= n; i++)
            {
                var sinTaylor = Sin(x, i); // value of the Taylor sinus
                var sinExact = Math.Sin(x); // value of the real sinus
                var approximation = Math.Abs(sinTaylor - sinExact);
                Console.WriteLine("  {0} \t\t\t {1:F20} \t {2:F20} \t {3:F20}", i, sinExact, sinTaylor, approximation);
            }
        }
    }
}