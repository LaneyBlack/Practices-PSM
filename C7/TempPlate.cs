using System;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex32;


namespace C7
{
    public class TempPlate
    {
        public double[][] T { get; set; } // temperature matrix
        public int N { get; set; }

        public TempPlate(int n)
        {
            N = n;
            T = new double[N + 2][];
            for (var i = 0; i < T.Length; i++)
                T[i] = new double[N + 2];
        }

        public void SetEdgeTemps(double[] edges)
        {
            int maxX = T[0].Length, maxY = T.Length;
            for (var y = 0; y < maxY; y++)
            {
                for (var x = 0; x < maxX; x++)
                {
                    if ((y == 0 || y == maxY - 1) && (x == 0 || x == maxX - 1))
                        continue;
                    if (y == 0)
                        T[y][x] = edges[0];
                    else if (x == maxX - 1)
                        T[y][x] = edges[1];
                    else if (y == maxY - 1)
                        T[y][x] = edges[2];
                    else if (x == 0)
                        T[y][x] = edges[3];
                    else
                        T[y][x] = 0;
                }
            }
        }

        public void StepsCalcResultTemperature()
        {
            // Not used in the result lab
            for (int i = 0; i < 200; i++)
            for (int y = 1; y < T.Length - 1; y++)
            for (int x = 1; x < T.Length - 1; x++)
                T[y][x] = (T[y - 1][x] + T[y + 1][x] + T[y][x + 1] + T[y][x - 1]) / 4;
            // T[y][x] = (T[])
        }

        public void MatrixCalcResultTemperature()
        {
            //Create a matrix and vector
            var vector = Vector<double>.Build.Dense(N*N,0);
            var matrix = Matrix<double>.Build.Dense(N*N,N*N,0);;
            double sum;
            for (int i = 0; i < N*N; i++)
            {
                matrix[i,i] = -4;
                int y = (i / N) + 1; //relative to plate
                int x = (i % N) + 1; //relative to plate
                sum = 0;
                for (int j = -1; j < 2; j += 2)
                {
                    if (T[y + j][x] != 0) // if temp is known
                    {
                        sum -= T[y + j][x]; // put after = 
                    }
                    else // if temp is unknown
                    {
                        matrix[i,(y - 1 + j) * N + (x - 1)] = 1; // place 1 in the place
                    }

                    if (T[y][x + j] != 0) // if temp is known
                    {
                        sum -= T[y][x + j]; // put after =
                    }
                    else // if temp is unknown
                    {
                        // i - row, 
                        matrix[i,(y - 1) * N + (x - 1 + j)] = 1;
                    }
                }
                vector[i] = sum;
                // Console.WriteLine(matrix);
                // Console.WriteLine("\n");
                // Console.WriteLine(vector);
                // Console.WriteLine("----------------------------------------------------------------------------------");
            }
            // Solve the matrix
            var result = matrix.Solve(vector);
            for (int y = 1; y < N+1; y++)
            {
                for (int x = 1; x < N+1; x++)
                {
                    T[y][x] = result[(y-1)*N+x-1];
                }
            }
        }
    }
}