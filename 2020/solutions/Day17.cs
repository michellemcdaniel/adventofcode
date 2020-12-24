using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day17
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();
            Cube cube = new Cube(0, new Plane(input));

            for (int count = 0; count < 6; count++)
            {
                cube.Expand();
                cube.Generate(CheckRules);
            }

            Console.WriteLine($"Part One: {cube.CountOccupied()}");

            HyperCube hyperCube = new HyperCube(0, new Cube(0, new Plane(input)));
            for (int count = 0; count < 6; count++)
            {
                hyperCube.Expand();
                hyperCube.Generate(CheckRules);
            }

            Console.WriteLine($"Part Two: {hyperCube.CountOccupied()}");
        }

        public static char CheckRules(bool isActive, int totalActiveAround)
        {
            if ((totalActiveAround < 2 || totalActiveAround > 3) && isActive)
            {
                return '.';
            }
            else if (isActive)
            {
                return '#';
            }
            else if (!isActive && totalActiveAround == 3)
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