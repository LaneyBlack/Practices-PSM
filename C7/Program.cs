using System;
using System.IO;
using System.Net;

namespace C7
{
    internal class Program
    {
        private const string FilePath = //"C:\\PJATK\\4th\\PSM\\C7\\Data\\exp.csv";
            "C:\\PJATK\\4th\\PSM\\CwiczeniaPSM\\C7\\Data\\exp.csv";

        public static void Main(string[] args)
        {
            ClearFileCsv();
            var tempPlate = new TemperaturePlate(int.Parse(args[0]));
            SetHeaderCsv();
            tempPlate.SetEdgeTemps(new double[] { 200, 50, 150, 100 });
            ExportToCsv(tempPlate);
            SetHeaderCsv();
            tempPlate.MatrixCalcResultTemperature();
            ExportToCsv(tempPlate);
        }

        private static void ClearFileCsv()
        {
            File.WriteAllText(FilePath, "");
        }

        private static void SetHeaderCsv()
        {
            File.AppendAllText(FilePath, "\n\n\n");
        }

        private static void ExportToCsv(TemperaturePlate plate)
        {
            for (var y = 0; y < plate.T.Length; y++)
            {
                File.AppendAllText(FilePath, ",,");
                for (var x = 0; x < plate.T[y].Length; x++)
                    File.AppendAllText(FilePath, $"{Math.Round(plate.T[y][x])},");
                File.AppendAllText(FilePath, "\n");
            }
        }
    }
}