using System;
using System.Collections.Generic;
using System.IO;

namespace C6
{
    internal class Program
    {
        private const int Dimensions = 2;

        private const string FilePath = //"C:\\PJATK\\4th\\PSM\\Spring\\Data\\exp.csv";
            "C:\\PJATK\\4th\\PSM\\CwiczeniaPSM\\Spring\\Data\\exp.csv";

        public static void Main()
        {
            ClearFileCsv();
            var spring = new Spring(Math.PI / 2, 10, Dimensions);
            SetHeaderCsv(spring);
            double time = 0;
            const double dT = 0.3;
            do
            {
                ExportCsv(time, spring);
                spring.CountNextMidPoint(dT);
                time += dT;
            } while (time <= 6);
        }

        private static void ExportCsv(double time, Spring spring)
        {
            File.AppendAllText(FilePath, $"{time},");
            double ep = 0, ek = 0;
            for (var i = 1; i < spring.Points.Length; i++)
            {
                ep += Math.Pow(spring.Points[i].S[1] - spring.Points[i - 1].S[1], 2);
                ek += Math.Pow(spring.Points[i].Vy, 2);
            }

            ep *= 1 / (2 * spring.Dx);
            ek *= spring.Dx / 2;
            foreach (var point in spring.Points)
            {
                File.AppendAllText(FilePath, $"{Math.Round(point.S[1], 3)},");
            }

            File.AppendAllText(FilePath, $",{Math.Round(ep, 3)},{Math.Round(ek, 3)},{Math.Round(ep + ek, 3)}");
            File.AppendAllText(FilePath, "\n");
        }

        private static void ClearFileCsv()
        {
            File.WriteAllText(FilePath, "");
        }

        private static void SetHeaderCsv(Spring spring)
        {
            File.AppendAllText(FilePath, "Time,");
            for (var i = 0; i < spring.Points.Length; i++)
                File.AppendAllText(FilePath, $"{i},");
            File.AppendAllText(FilePath, ",Ep,Ek,Ec");
            File.AppendAllText(FilePath, "\n");
        }
    }
}