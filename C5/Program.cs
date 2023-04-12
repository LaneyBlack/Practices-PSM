using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace C5
{
    internal static class Program
    {
        // private const int Dimensions = 2;
        private const string FilePath = "C:\\PJATK\\4th\\PSM\\CwiczeniaPSM\\C5\\Data\\exp.csv";
        private static readonly double G = 6.6743e-11; // gravity const
        private const double Dt = 1800; //delta t (s)
        private const double T = 2592000; //simulation Time (s) one year = 2_592_000s
        private const int Dimensions = 2;

        public static void Main()
        {
            ClearFileCsv();
            //Creating dictionary of celestial objects and filling it up
            var celestialObjectsDict = new Dictionary<string, CelestialObject>();
            celestialObjectsDict.Add("Sun", new CelestialObject(0, 1.989e+30, G, true));
            celestialObjectsDict.Add("Earth", new CelestialObject(1.5e+8, 5.972e+24, G, false));
            celestialObjectsDict.Add("Moon", new CelestialObject(celestialObjectsDict["Earth"].S[1] + 384400, 7.347e+22, G,false));
            SetHeaderCsv(celestialObjectsDict);
            //count if time not expired or it's not on y=0
            for (double time = 0; time <= T; time += Dt)
            {
                CalcPosMidPoint(Dt, celestialObjectsDict);
                ExportCsv(time, celestialObjectsDict);
            }
        }


        private static void CalcPosMidPoint(double dT, Dictionary<string, CelestialObject> celestialObjectsDict)
        {
            var celestialObjectsBefore = new Dictionary<string, CelestialObject>(celestialObjectsDict);
            foreach (var keyValuePair in celestialObjectsDict.Where(pair => pair.Key!="Sun"))
            {
                var currentObject = keyValuePair.Value;
                double a = 0, vecMidA = 0, wLen = 0, midWLen = 0;
                var midS = new double[Dimensions];
                var midV = new double[Dimensions];
                var midA = new double[Dimensions];
                var u = new double[Dimensions];
                var w = new double[Dimensions];
                var midU = new double[Dimensions];
                var midW = new double[Dimensions];

                for (var i = 0; i < Dimensions; i++)
                {
                    currentObject.A[i] = 0;
                    midA[i] = 0;
                }

                // Count this for every other celestial object in the system
                foreach (var keyValueBeforePair in celestialObjectsBefore.Where(keyValueBeforePair =>
                             keyValuePair.Key != keyValueBeforePair.Key))
                {
                    var anotherObject = keyValueBeforePair.Value;
                    for (var i = 0; i < Dimensions; i++)
                        midS[i] = currentObject.S[i] + currentObject.V[i] * dT / 2; // S(dt/2) = S(dt) + V(0)*dt/2
                    //Distances between objects
                    for (var i = 0; i < Dimensions; i++)
                    {
                        w[i] = anotherObject.S[i] - currentObject.S[i]; // by every coordinate
                        wLen += Math.Pow(w[i], 2); // and then overall
                        // ToDo be careful here
                        midW[i] = anotherObject.S[i] - midS[i];
                        midWLen += Math.Pow(midW[i], 2);
                    }
                    wLen = Math.Sqrt(wLen); // W = Sqrt[ wx^2 + wy^2 + wz^3 ]
                    midWLen = Math.Sqrt(midWLen); // W = Sqrt[ wx^2 + wy^2 + wz^3 ]
                    // Accelerations in vectors (overall)
                    //ToDo
                    //FIXME For some reason acceleration are very big.
                    a = G * anotherObject.M / Math.Pow(wLen, 2); // A = G * M / R^2
                    vecMidA = G * anotherObject.M / Math.Pow(midWLen, 2); // A = G * M / R^2
                }
                
                // Calculate U's and Accelrations on X and Y
                for (var i = 0; i < Dimensions; i++)
                {
                    u[i] = w[i] / wLen; // U(x) = W(x) / WLen(x)
                    if (u[i]!=0)
                        currentObject.A[i] = a / u[i]; // Ax = (a / U[x])
                    midU[i] = midW[i] / midWLen; // U(x/2) = W(x/2) / WLen(x/2)
                    if (midU[i]!=0)
                        midA[i] = vecMidA / midU[i]; // Ax/2 = Sum( a(x/2) / U[x/2])
                }

                for (var i = 0; i < Dimensions; i++)
                {
                    midV[i] = currentObject.V[i] + currentObject.A[i] * dT;
                    //Coordinate (S) calculation
                    currentObject.S[i] += midV[i] * dT; // S(dt) = S(0) + V(dt/2)*dT
                    //Velocity (V) calculation
                    currentObject.V[i] += midA[i] * Dt; // S(dt) = V(0) + A(dt/2)*dT
                }
            }
        }

        private static void ExportCsv(double t, Dictionary<string, CelestialObject> celestials)
        {
            File.AppendAllText(FilePath, $"{t},");
            foreach (var keyValuePair in celestials)
                File.AppendAllText(FilePath,
                    $"{Math.Round(keyValuePair.Value.S[0], 3)},{Math.Round(keyValuePair.Value.S[1])},");
            // t SunX SunY EarthX EarthY MoonX MoonY ...
            File.AppendAllText(FilePath, "\n");
        }

        private static void ClearFileCsv()
        {
            File.WriteAllText(FilePath, "");
        }

        private static void SetHeaderCsv(Dictionary<string, CelestialObject> celestials)
        {
            File.AppendAllText(FilePath, "Time,");
            foreach (KeyValuePair<string, CelestialObject> keyValuePair in celestials)
                File.AppendAllText(FilePath, $"{keyValuePair.Key}X,{keyValuePair.Key}Y,");
            File.AppendAllText(FilePath, "\n");
        }
    }
}