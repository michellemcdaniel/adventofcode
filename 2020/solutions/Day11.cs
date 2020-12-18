using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;

namespace adventofcode
{
    class Day11
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();

            for (int i = 0; i < input.Count(); i++)
            {
                input[i] = input[i].Replace('.', ' ').Replace('L', '.');
            }

            Plane plane = new Plane(input);

            bool changed = true;
            while(changed)
            {
                changed = plane.Generate(CheckRulesFirst, 1);
            }

            Console.WriteLine($"Count: {plane.CountOccupied()}");

            plane = new Plane(input);

            changed = true;
            while(changed)
            {
                changed = plane.Generate(CheckRulesSecond, Math.Max(input.Count, input.First().Length));
            }

            Console.WriteLine($"Count: {plane.CountOccupied()}");
        }

        public static char CheckRulesFirst(bool isOccupied, int countOccupied)
        {
            if (isOccupied && countOccupied >= 4)
            {
                return '.';
            }
            else if (isOccupied)
            {
                return '#';
            }
            else if (!isOccupied && countOccupied == 0)
            {
                return '#';
            }
            else
            {
                return '.';
            }
        }

        public static char CheckRulesSecond(bool isOccupied, int countOccupied)
        {
            if (isOccupied && countOccupied >= 5)
            {
                return '.';
            }
            else if (isOccupied)
            {
                return '#';
            }
            else if (!isOccupied && countOccupied == 0)
            {
                return '#';
            }
            else
            {
                return '.';
            }
        }
    }
}