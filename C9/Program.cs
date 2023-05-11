using System;
using System.IO;

namespace C9
{
    internal class Program
    {
        private const string FilePath = //"C:\\PJATK\\4th\\PSM\\C7\\Data\\exp.csv";
            "C:\\PJATK\\4th\\PSM\\CwiczeniaPSM\\C9\\Data\\exp.csv";

        public static void Main()
        {
            const double a = 10, b = 25, c = 8.0 / 3.0, s0 = 1;
            ClearFileCsv();

            SetHeaderCsv();
            int iterations = 0;
            double dT = 0.03;
            var butterFly = new ButterFly(s0,a,b,c);
            while (iterations < 1500)
            {
                ExportToCsv(iterations, butterFly);
                butterFly.CalcNext(dT);
                iterations++;
            }

            SetHeaderCsv();
            iterations = 0;
            dT = 0.03;
            butterFly = new ButterFly(s0,a,b,c);
            while (iterations < 1500)
            {
                ExportToCsv(iterations, butterFly);
                butterFly.CalcNextMidPoint(dT);
                iterations++;
            }

            SetHeaderCsv();
            iterations = 0;
            butterFly = new ButterFly(s0,a,b,c);
            while (iterations < 1500)
            {
                ExportToCsv(iterations, butterFly);
                butterFly.CalcNextRk4(dT);
                iterations ++;
            }
        }

        private static void ClearFileCsv()
        {
            File.WriteAllText(FilePath, "");
        }

        private static void SetHeaderCsv()
        {
            File.AppendAllText(FilePath, "\n\n\n");
            File.AppendAllText(FilePath, "T,X,Z,Y");
            File.AppendAllText(FilePath, "\n");
        }

        private static void ExportToCsv(double time, ButterFly butterFly)
        {
            File.AppendAllText(FilePath, $"{Math.Round(time, 3)},{Math.Round(butterFly.X,3)}," +
                                         $"{Math.Round(butterFly.Z,3)},{Math.Round(butterFly.Y,3)},");
            File.AppendAllText(FilePath, "\n");
        }
    }
}