using System;
using System.IO;

namespace C2
{
    internal static class Program
    {
        const int dimensions = 2;
        private static decimal t = 0, m = new decimal(0.3), k = new decimal(0.1), g = new decimal(-10);

        private static decimal[] s = new decimal[dimensions], // s(t0) - coordinate
            v = new decimal[dimensions], // V(t0) - velocity
            vDtH = new decimal[dimensions], // V (t0 + delta t/2)
            a = new decimal[dimensions], // a(t0) - acceleration
            aDtH = new decimal[dimensions]; // a(t0 + delta t/2)
        // example s[0] = sX, s[1] = sY, s[2]=sZ

        public static void Main(string[] args)
        {
            Console.WriteLine("Please input the dT (time step) in seconds:");
            var dT = decimal.Parse(Console.ReadLine()?.Trim() ?? string.Empty);
            Console.WriteLine("Please input the start V (velocity) in every direction in m/s:");
            var vAbstract = decimal.Parse(Console.ReadLine()?.Trim() ?? string.Empty);
            ResetArguments(vAbstract);
            File.WriteAllText("C:\\PJATK\\4th\\PSM\\C2\\C2\\Data\\export.csv", "Time, Sx, Sy, Vx, Vy, Ax, Ay\n");
            do // Euler ------------------------------------------------------------------------------------------------
            {
                WriteDataLine(); // exporting line into the csv file
                t += dT;
                for (var i = 0; i < dimensions; i++)
                {
                    s[i] += v[i] * dT; //s = s(t0) + v(t0)*dt
                    v[i] += a[i] * dT; //v = v(t0) + a(t0)*dt
                }

                a[0] = -k * v[0] / m; //ax=-kv/m
                // a[0] = 0;
                a[1] = (m * g - k * v[1]) / m; //ay = (mg-kv)/m
                // a[1] = g; //ay = (mg-kv)/m
            } while (s[1] >= 0); //sy >= 0 (while object didn't hit the ground)

            File.AppendAllText("C:\\PJATK\\4th\\PSM\\C2\\C2\\Data\\export.csv",
                t + "," + s[0] + ",0,0,0,0,0\n\nTime, Sx, Sy, Vx, Vy, Ax, Ay\n");
            ResetArguments(vAbstract);
            do // Better Euler -----------------------------------------------------------------------------------------
            {
                WriteDataLine();
                t += dT;
                //First count the coordinates
                for (var i = 0; i < dimensions; i++)
                {
                    vDtH[i] = v[i] + a[i] * (dT / 2); // V(t0 + dT/2) = v(t0) + ...
                    s[i] += vDtH[i] * dT;
                }

                //then count the speed
                aDtH[0] = -k * vDtH[0] / m; //x
                // aDtH[0] = 0;
                aDtH[1] = (m * g - k * vDtH[0]) / m; //y
                // aDtH[1] = g;
                for (var i = 0; i < dimensions; i++)
                {
                    v[i] += aDtH[i] * dT;
                }
            
                //then count the acceleration
                a[0] = -k * v[0] / m; //ax=-kv/m
                // a[0] = 0;
                a[1] = (m * g - k * v[1]) / m; //ay = (mg-kv)/m
                // a[1] = g; //ay = (mg-kv)/m
            } while (s[1] >= 0); //sY >= 0 (while object didn't hit the ground)

            File.AppendAllText("C:\\PJATK\\4th\\PSM\\C2\\C2\\Data\\export.csv", t + "," + Math.Round(s[0]*100)/100 + ",0,0,0,0,0");
        }

        private static void ResetArguments(decimal vAbstract)
        {
            t = 0;
            for (var i = 0; i < dimensions; i++) //zero all the variables
            {
                s[i] = 0;
                v[i] = vAbstract;
                vDtH[i] = 0;
                aDtH[i] = 0;
            }

            a[0] = -k * v[0] / m; //ax=-kv
            // a[0] = 0; //ax=-kv
            a[1] = (m * g - k * v[1]) / m; //ay = mg-kv
            // a[1] = g; //ay = mg-kv
        }

        private static void WriteDataLine()
        {
            File.AppendAllText("C:\\PJATK\\4th\\PSM\\C2\\C2\\Data\\export.csv",
                t + "," + Math.Round(s[0] * 100) / 100 + "," + Math.Round(s[1] * 100) / 100 + "," +
                +Math.Round(v[0] * 100) / 100 + "," + Math.Round(v[1] * 100) / 100 + ","
                + Math.Round(a[0] * 100) / 100 + "," + Math.Round(a[1] * 100) / 100
                + "\n");
            // Console.WriteLine(t + ",\t" + Math.Round(s[0] * 100) / 100 + ",\t" + Math.Round(s[1] * 100) / 100);
        }
    }
}