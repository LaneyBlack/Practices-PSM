using System;
using System.IO;

namespace C9
{
    internal class Program
    {
        private const string FilePath = //"C:\\PJATK\\4th\\PSM\\C7\\Data\\exp.csv";
            "C:\\PJATK\\4th\\PSM\\CwiczeniaPSM\\C9\\Data\\exp.csv";

        private const int Dimensions = 2;

        public static void Main()
        {
            ClearFileCsv();
            
            SetHeaderCsv();
            double time = 0;
            const double dT = 0.1;
            var physObj = new PhysicalObject(Dimensions);
            do
            {
                ExportToCsv(time, physObj);
                physObj.CalcNext(dT);
                time += dT;
            } while (time <= 6);

            SetHeaderCsv();
            time = 0;
            physObj = new PhysicalObject(Dimensions);
            do
            {
                ExportToCsv(time, physObj);
                physObj.CalcNextMidPoint(dT);
                time += dT;
            } while (time <= 6);

            SetHeaderCsv();
            time = 0;
            physObj = new PhysicalObject(Dimensions);
            do
            {
                ExportToCsv(time, physObj);
                physObj.CalcNextRk4(dT);
                time += dT;
            } while (time <= 6);
        }

        private static void ClearFileCsv()
        {
            File.WriteAllText(FilePath, "");
        }

        private static void SetHeaderCsv()
        {
            File.AppendAllText(FilePath, "\n\n\n");
            File.AppendAllText(FilePath,"T,Sx,Sy");
        }

        private static void ExportToCsv(double time, PhysicalObject physObj)
        {
            File.AppendAllText(FilePath, "\n\n");
            File.AppendAllText(FilePath, $"{Math.Round(time, 3)},");
            foreach (var s in physObj.S)
                File.AppendAllText(FilePath, $"{Math.Round(s, 3)},");

            File.AppendAllText(FilePath, "\n");
        }
    }
}