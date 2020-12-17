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
            Cube cubeCube = new Cube(0, new Plane(input));

            for (int count = 0; count < 6; count++)
            {
                cubeCube.Expand();
                Cube newCube = new();
                foreach (var plane in cubeCube.Planes)
                {
                    int z = plane.Key;
                    newCube.Planes.Add(z, plane.Value.Create(CheckRules, cubeCube, z));
                }
                cubeCube = newCube;
            }

            Console.WriteLine($"Part one: {cubeCube.CountOccupied()}");

            HyperCube hyperCube = new HyperCube(0, new Cube(0, new Plane(input)));
            for (int count = 0; count < 6; count++)
            {
                hyperCube.Expand();

                Dictionary<int, Dictionary<int, List<string>>> newHypercube = new();
                HyperCube newHyperCube = new();

                foreach (var cube in hyperCube.Cubes)
                {
                    int w = cube.Key;
                    newHyperCube.Cubes.Add(w, cube.Value.Create(CheckRules, hyperCube, w));
                }
                
                hyperCube = newHyperCube;
            }

            Console.WriteLine($"Part two: {hyperCube.CountOccupied()}");
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