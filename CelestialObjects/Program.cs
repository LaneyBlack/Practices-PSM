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
        private const string
            FilePath =
                "C:\\PJATK\\4th\\PSM\\CelestialObjects\\Data\\exp.csv"; //"C:\\PJATK\\4th\\PSM\\CwiczeniaPSM\\CelestialObjects\\Data\\exp.csv"

        private static readonly double G = 6.6743e-11; //N*km^2/kg^2 gravity const 6.6743e-11 N*m^2/kg^2 
        private const double Dt = 21_600; //delta t (s)
        private const double T = 31_556_926; //simulation Time (s) one year = 2_592_000s
        private const int Dimensions = 2;

        public static void Main()
        {
            ClearFileCsv();
            //Creating dictionary of celestial objects and filling it up
            var celestialObjectsDict = new Dictionary<string, CelestialObject>();
            celestialObjectsDict.Add("Sun", new CelestialObject(0, 1.989e+30, null, G));
            celestialObjectsDict.Add("Earth", new CelestialObject(1.5e+11, 5.972e+24, celestialObjectsDict["Sun"], G));
            celestialObjectsDict.Add("Moon", new CelestialObject(3844e+5, 7.347e+22, celestialObjectsDict["Earth"], G));
            SetHeaderCsv(celestialObjectsDict);
            //count if time not expired or it's not on y=0
            ExportCsv(0, celestialObjectsDict);
            for (var time = Dt; time <= T; time += Dt)
            {
                CalcPosMidPoint(Dt, celestialObjectsDict, time);
                ExportCsv(time, celestialObjectsDict);
            }
        }


        private static void CalcPosMidPoint(double dT, IDictionary<string, CelestialObject> celestialObjectsDict,
            double time)
        {
            var celestialObjectsBefore = new Dictionary<string, CelestialObject>();
            // Make dictionary of celestial object before changes in system
            foreach (var key in celestialObjectsDict.Keys)
            {
                celestialObjectsBefore.Add(key, new CelestialObject(celestialObjectsDict[key]));
            }

            foreach (var keyValuePair in celestialObjectsDict.Where(pair => pair.Key != "Sun"))
            {
                // Initialise variables
                var currentObject = keyValuePair.Value;
                double a = 0, middleA = 0, wLen = 0, midWLen = 0;
                var midS = new double[Dimensions];
                var anotherObjectMidS = new double[Dimensions];
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
                    {
                        midS[i] = currentObject.S[i] + currentObject.V[i] * dT / 2; // S(dt/2) = S(0) + V(0)*dt/2
                        anotherObjectMidS[i] =
                            anotherObject.S[i] + anotherObject.V[i] * dT / 2; // S(dt/2) = S(0) + V(0)*dt/2
                    }

                    //Distances between objects
                    for (var i = 0; i < Dimensions; i++)
                    {
                        w[i] = anotherObject.S[i] - currentObject.S[i]; // by every coordinate W=S(anObj) - S (curObj)
                        wLen += Math.Pow(w[i], 2); // and then overall WLen = sqrt(Wx^2 + Wy^2)
                        midW[i] = anotherObjectMidS[i] - midS[i];
                        midWLen += Math.Pow(midW[i], 2);
                    }

                    wLen = Math.Sqrt(wLen); // W = Sqrt[ wx^2 + wy^2 + wz^3 ]
                    midWLen = Math.Sqrt(midWLen); // W = Sqrt[ wx^2 + wy^2 + wz^3 ]
                    a = G * anotherObject.M / Math.Pow(wLen, 2); // A = G * M / R^2
                    middleA = G * anotherObject.M / Math.Pow(midWLen, 2); // A = G * M / R^2

                    // Calculate U's and Accelrations on X and Y
                    for (var i = 0; i < Dimensions; i++)
                    {
                        u[i] = w[i] / wLen; // U(x) = W(x) / WLen(x)
                        currentObject.A[i] += a * u[i]; // Ax = (a * U[x])
                        midU[i] = midW[i] / midWLen; // U(x/2) = W(x/2) / WLen(x/2)
                        midA[i] += middleA * midU[i]; // A(x/2) = a(x/2) * U(x/2)
                    }
                }

                for (var i = 0; i < Dimensions; i++)
                {
                    midV[i] = currentObject.V[i] + currentObject.A[i] * dT / 2; // V(dt/2) = V(0) + A(0) * dt/2
                    //Coordinate (S) calculation
                    currentObject.S[i] += midV[i] * dT; // S(dt) = S(0) + V(dt/2)*dT
                    //Velocity (V) calculation
                    currentObject.V[i] += midA[i] * dT; // S(dt) = V(0) + A(dt/2)*dT
                }
            }
        }

        private static void ExportCsv(double t, Dictionary<string, CelestialObject> celestials)
        {
            File.AppendAllText(FilePath, $"{t},");
            foreach (var keyValuePair in celestials)
                File.AppendAllText(FilePath, $"{Math.Round(keyValuePair.Value.S[0], 3)},{Math.Round(keyValuePair.Value.S[1])},");
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
            foreach (var keyValuePair in celestials)
                File.AppendAllText(FilePath, $"{keyValuePair.Key}X,{keyValuePair.Key}Y,");
            File.AppendAllText(FilePath, "\n");
        }
    }
}