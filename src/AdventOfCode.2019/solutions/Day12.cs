using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Nineteen
{
    public class Day12
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();

            List<Planet> planets = new List<Planet>();

            foreach(string i in input)
            {
                RegexHelper.Match(i, @"\<x=(-?\d+), y=(-?\d+), z=(-?\d+)\>", out int x, out int y, out int z);
                planets.Add(new Planet(x, y, z));
            }

            int iterations = 0;
            long xIters = -1;
            long yIters = -1;
            long zIters = -1;

            while(xIters == -1 || yIters == -1 || zIters == -1)
            {
                iterations++;
                foreach(var planetOne in planets)
                {
                    foreach(var planetTwo in planets)
                    {
                        if (planetOne != planetTwo)
                        {
                            planetOne.VelocityX += planetTwo.X.CompareTo(planetOne.X);
                            planetOne.VelocityY += planetTwo.Y.CompareTo(planetOne.Y);
                            planetOne.VelocityZ += planetTwo.Z.CompareTo(planetOne.Z);
                        }
                    }
                }

                foreach (var planet in planets)
                {
                    planet.Move();
                }

                if (iterations == 1000)
                {
                    long totalEnergy = 0;
                    foreach (var planet in planets)
                    {
                        totalEnergy += planet.CalculateTotalEnergy();
                    }

                    Console.WriteLine($"Part One: {totalEnergy}");
                }

                if (xIters == -1 && planets.All(p => p.VelocityX == 0))
                {
                    xIters = iterations;
                }
                if (yIters == -1 && planets.All(p => p.VelocityY == 0))
                {
                    yIters = iterations;
                }
                if (zIters == -1 && planets.All(p => p.VelocityZ == 0))
                {
                    zIters = iterations;
                }
            }

            long lcm = (2*xIters*yIters*zIters)/
                (GreatestCommonDivisor(xIters, yIters)*GreatestCommonDivisor(xIters, zIters)*GreatestCommonDivisor(yIters, zIters));

            Console.WriteLine($"Part Two: {lcm}");           
        }

        public static long GreatestCommonDivisor(long x, long y)
        {
            while (x != 0 && y != 0)
            {
                if (x > y)
                    x %= y;
                else
                    y %= x;
            }
            return x | y;
        }

        public class Planet
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
            public int VelocityX { get; set; }
            public int VelocityY { get; set; }
            public int VelocityZ { get; set; }

            public Planet (int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
                VelocityX = 0;
                VelocityY = 0;
                VelocityZ = 0;
            }

            public string GetSignature()
            {
                return $"{X},{Y},{Z}={VelocityX},{VelocityY},{VelocityZ}";
            }

            public void Move()
            {
                X = X+VelocityX;
                Y = Y+VelocityY;
                Z = Z+VelocityZ;
            }

            public long CalculatePotentialEnergy()
            {
                return Math.Abs(X) + Math.Abs(Y) + Math.Abs(Z);
            }

            public long CalculateKineticEnergy()
            {
                return Math.Abs(VelocityX) + Math.Abs(VelocityY) + Math.Abs(VelocityZ);
            }

            public long CalculateTotalEnergy()
            {
                return CalculatePotentialEnergy() * CalculateKineticEnergy();
            }
        }
    }
}
