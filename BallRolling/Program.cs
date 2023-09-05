using System;
using System.IO;
using System.Runtime.InteropServices;

namespace C4
{
    internal static class Program
    {
        // private const int Dimensions = 2;
        private const string FilePath = "C:\\PJATK\\4th\\PSM\\CwiczeniaPSM\\BallRolling\\Data\\exp.csv";
        private static readonly decimal G = new decimal(9.81); // gravity
        private static readonly decimal Dt = new decimal(0.01); //delta t (s)
        private static readonly decimal K = new decimal(0.1); // wind 
        private static readonly decimal T = new decimal(4); //simulation Time (s)
        private const double Alpha = Math.PI / 180 * 45; //steep angle (radians)
        private static readonly decimal H = new decimal(20); //height of the hill (meters)

        public static void Main()
        {
            ClearFileCsv();
            var length = new decimal((double)H / Math.Tan(Alpha));
            //Simulate Ball
            var ball = new Ball(H + 1, 1, 1, 2, true); // isBall = true
            var ballEdgePoint = new BallEdgePoint(0, ball.R, 2, ball);
            SetHeaderCsv(H, length);
            //count if time not expired or it's not on y=0
            for (decimal time = 0; time <= T && ball.S[1] >= 0; time += Dt)
            {
                CalcBetterEuler(Dt, ball, ballEdgePoint);
                ExportCsv(time, ball, ballEdgePoint);
            }
            //Simulate Sphere
            var sphere = new Ball(H + 1, 1, 1, 2, false); // isBall = false
            var sphereEdgePoint = new BallEdgePoint(0, sphere.R, 2, sphere);
            SetHeaderCsv(H, length);
            //count if time not expired or it's not on y=0
            for (decimal time = 0; time <= T && sphere.S[1] >= 0; time += Dt)
            {
                CalcBetterEuler(Dt, sphere, sphereEdgePoint);
                ExportCsv(time, sphere, sphereEdgePoint);
            }
        }


        private static void CalcBetterEuler(decimal dT, Ball ball, BallEdgePoint edgePoint)
        {
            // Ball Simulation
            ball.A = (G * (decimal)Math.Sin(Alpha)) / (1 + ball.I / (ball.M * ball.R * ball.R))
                     - (K * ball.V / ball.M); // a(0) = g * sin(Alpha) / (1 + I / m * r^2) - k * V(0) / m
            var vHalf = ball.V + ball.A * dT / 2; // V(dt/2) = V(0) + a(0) * dT/2
            ball.SRot[0] += vHalf * dT; // S(dt) = S(0) + V(dt/2) * dt  | on X
            // on Y no changes in rotated coordinate system

            var aHalf = (G * (decimal)Math.Sin(Alpha)) / (1 + ball.I / (ball.M * ball.R * ball.R))
                        - (K * vHalf / ball.M); // a(dt/2) = g * sin(Alpha) / (1 + I / m * r^2) - k * V(dt/2) / m
            ball.V += aHalf * dT; // V(dt) = V(0) + a(dt/2) * dt
            // Translate S to original (not rotated) coordinate system
            ball.S[0] = ball.SRot[0] * (decimal)Math.Cos(-Alpha) -
                        ball.SRot[1] * (decimal)Math.Sin(-Alpha);
            ball.S[1] = ball.SRot[0] * (decimal)Math.Sin(-Alpha) +
                        ball.SRot[1] * (decimal)Math.Cos(-Alpha) + H;

            // EdgePoint Simulation
            edgePoint.Eps = ball.A / edgePoint.R; // Eps(0) = a(0) / R
            var omegaHalf = edgePoint.Omega + edgePoint.Eps * dT / 2; // Omega(dt/2) = Omega(0) + Eps * dt/2
            edgePoint.Beta += (double)(omegaHalf * dT); // Beta(dt) = Beta(0) + Omega(dt/2) * dt
            var epsHalf = aHalf / edgePoint.R; // Eps(dt/2) = a(dt/2) / R
            edgePoint.Omega += epsHalf * dT; // Omega(dt) = Omega(0) + Eps(dt/2) * dt
            // Change point coordinates using his angle and ball center
            edgePoint.S[0] = ball.S[0] + edgePoint.R *
                (decimal)Math.Cos(Math.PI / 2 - edgePoint.Beta); // x = center + r * cos(90' - Beta)
            edgePoint.S[1] = ball.S[1] + edgePoint.R *
                (decimal)Math.Sin(Math.PI / 2 - edgePoint.Beta); // y = center + r * sin(90' - Beta)
        }

        private static void ExportCsv(decimal t, Ball ball, BallEdgePoint edgePoint)
        {
            var Ep = Math.Abs(ball.M * G * ball.S[1]); // potential Energy (m * g * h)
            var Ek = ball.M * ball.V * ball.V / 2; //kinetic Energy (m * v^2 / 2)
            File.AppendAllText(FilePath,
                $"{t},{Math.Round(ball.S[0], 3)},{Math.Round(ball.S[1], 3)},{Math.Round(ball.V, 3)}," +
                $"{Math.Round(edgePoint.S[0], 3)}, {Math.Round(edgePoint.S[1], 3)},{Math.Round(edgePoint.Omega, 3)}," +
                $"{Math.Round(Ep, 3)},{Math.Round(Ek, 3)},{Math.Round(Ep + Ek, 3)}\n");
            // t Sx Sy EdgeSx EdgeSy V Ep Ek Ec
        }

        private static void ClearFileCsv()
        {
            File.WriteAllText(FilePath, "");
        }

        private static void SetHeaderCsv(decimal h, decimal l)
        {
            File.AppendAllText(FilePath, $"\nTriangle,0,{h}\n,{l},0\n");
            File.AppendAllText(FilePath, "Time,Sx,Sy,V,EdgeSx,EdgeSy,EdgeW,Ep,Ek,Ec\n");
        }
    }
}