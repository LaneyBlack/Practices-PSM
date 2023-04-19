using System;
using System.Collections.Generic;
using System.IO;

namespace C6
{
    internal class Program
    {
        private const int Dimensions = 2;

        private const string FilePath = //"C:\\PJATK\\4th\\PSM\\C6\\Data\\exp.csv";
            "C:\\PJATK\\4th\\PSM\\CwiczeniaPSM\\C6\\Data\\exp.csv";

        public static void Main()
        {
            ClearFileCsv();
            var spring = new Spring(Math.PI/2, 10, Dimensions);
            SetHeaderCsv(spring);
            double time = 0;
            const double dT = 0.3;
            do
            {
                ExportCsv(time, spring);
                spring.CountNextMidPoint(dT);
                time += dT;
            } while (time<=6);
        }

        private static void ExportCsv(double time, Spring spring)
        {
            File.AppendAllText(FilePath, $"{time},");
            foreach (var point in spring.Points)
            {
                File.AppendAllText(FilePath, $"{Math.Round(point.S[1],3)},");
            }
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
            File.AppendAllText(FilePath, "\n");
        }
    }
}