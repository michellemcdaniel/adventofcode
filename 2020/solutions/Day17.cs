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

            //List<Cube> cubes = new();

            Dictionary<int, List<string>> cubes = new();
            cubes.Add(0, input);

            for (int count = 0; count < 6; count++)
            {
                cubes = Expand(cubes);
                Dictionary<int, List<string>> newCubes = new();
                foreach(var layer in cubes)
                {
                    int z = layer.Key;
                    List<string> newLayer = new List<string>();
                    for (int x = 0; x < layer.Value.Count(); x++)
                    {
                        string newX = "";
                        for(int y = 0; y < layer.Value[x].Length; y++)
                        {
                            bool cubeActive = layer.Value[x][y] == '#';
                            int totalActiveAround = 0;
                            for (int i = -1; i < 2; i++)
                            {
                                for (int j = -1; j < 2; j++)
                                {
                                    for (int k = -1; k < 2; k++)
                                    {
                                        if (i == 0 && j == 0 && k == 0)
                                        {
                                            continue;
                                        }

                                        int newXCoord = x+i;
                                        int newYCoord = y+j;
                                        int newZCoord = z+k;

                                        if (cubes.ContainsKey(newZCoord))
                                        {
                                            if (newXCoord < 0)
                                            {
                                                continue;
                                            }
                                            else if (newXCoord >= cubes[newZCoord].Count())
                                            {
                                                continue;
                                            }
                                            else if (newYCoord < 0)
                                            {
                                                continue;
                                            }
                                            else if (newYCoord >= cubes[newZCoord][newXCoord].Length)
                                            {
                                                continue;
                                            }
                                            else if (cubes[newZCoord][newXCoord][newYCoord] == '#')
                                            {
                                                totalActiveAround++;
                                            }
                                        }
                                    }
                                }
                            }

                            if ((totalActiveAround < 2 || totalActiveAround > 3) && cubeActive)
                            {
                                newX += '.';
                            }
                            else if (cubeActive)
                            {
                                newX += '#';
                            }
                            else if (!cubeActive && totalActiveAround == 3)
                            {
                                newX += '#';
                            }
                            else
                            {
                                newX += '.';
                            }
                        }
                        
                        newLayer.Add(newX);
                    }
                    newCubes.Add(z, newLayer);
                }

                cubes = newCubes;
            }

            int totalActive = 0;
            foreach (var kvp in cubes)
            {
                foreach(var line in kvp.Value)
                {
                    totalActive += line.Where(x => x == '#').Count();
                }
            }

            Console.WriteLine($"Part one: {totalActive}");

            Dictionary<int, Dictionary<int, List<string>>> hypercube = new();
            Dictionary<int, List<string>> first = new();
            first.Add(0, input);
            hypercube.Add(0, first);

            for (int count = 0; count < 6; count++)
            {
                hypercube = ExpandHyperCube(hypercube);

                Dictionary<int, Dictionary<int, List<string>>> newHyperCube = new();
                
                foreach(var cube in hypercube)
                {
                    int w = cube.Key;
                    Dictionary<int, List<string>> newCubes = new();
                    foreach(var layer in cube.Value)
                    {
                        int z = layer.Key;
                        List<string> newLayer = new List<string>();
                        for (int x = 0; x < layer.Value.Count(); x++)
                        {
                            string newX = "";
                            for(int y = 0; y < layer.Value[x].Length; y++)
                            {
                                bool cubeActive = layer.Value[x][y] == '#';
                                int totalActiveAround = 0;
                                for (int i = -1; i < 2; i++)
                                {
                                    for (int j = -1; j < 2; j++)
                                    {
                                        for (int k = -1; k < 2; k++)
                                        {
                                            for (int l = -1; l < 2; l++)
                                            {
                                                if (i == 0 && j == 0 && k == 0 && l == 0)
                                                {
                                                    continue;
                                                }

                                                int newXCoord = x+i;
                                                int newYCoord = y+j;
                                                int newZCoord = z+k;
                                                int newWCoord = w+l;

                                                if (hypercube.ContainsKey(newWCoord))
                                                {
                                                    Dictionary<int, List<string>> curCube = hypercube[newWCoord];
                                                    if (curCube.ContainsKey(newZCoord))
                                                    {
                                                        if (newXCoord < 0)
                                                        {
                                                            continue;
                                                        }
                                                        else if (newXCoord >= curCube[newZCoord].Count())
                                                        {
                                                            continue;
                                                        }
                                                        else if (newYCoord < 0)
                                                        {
                                                            continue;
                                                        }
                                                        else if (newYCoord >= curCube[newZCoord][newXCoord].Length)
                                                        {
                                                            continue;
                                                        }
                                                        else if (curCube[newZCoord][newXCoord][newYCoord] == '#')
                                                        {
                                                            totalActiveAround++;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                if ((totalActiveAround < 2 || totalActiveAround > 3) && cubeActive)
                                {
                                    newX += '.';
                                }
                                else if (cubeActive)
                                {
                                    newX += '#';
                                }
                                else if (!cubeActive && totalActiveAround == 3)
                                {
                                    newX += '#';
                                }
                                else
                                {
                                    newX += '.';
                                }
                            }
                            
                            newLayer.Add(newX);
                        }
                        newCubes.Add(z, newLayer);
                    }
                    newHyperCube.Add(w, newCubes);
                }

                hypercube = newHyperCube;
            }

            totalActive = 0;
            foreach (var cube in hypercube)
            {
                foreach(var layer in cube.Value)
                {
                    foreach (var line in layer.Value)
                        totalActive += line.Where(x => x == '#').Count();
                }
            }

            Console.WriteLine($"Part two: {totalActive}");
        }

        public static Dictionary<int, List<string>> Expand(Dictionary<int, List<string>> original)
        {
            Dictionary<int, List<string>> newMap = new Dictionary<int, List<string>>();

            int minRow = original.Keys.Min()-1;
            int maxRow = original.Keys.Max()+1;
            int rowLength = original.First().Value[0].Length+2;
            int rowCount = original.First().Value.Count+2;

            newMap.Add(minRow, new List<string>());
            newMap.Add(maxRow, new List<string>());

            for(int i = 0; i < rowCount; i++)
            {
                newMap[minRow].Add(new String('.',rowLength));
                newMap[maxRow].Add(new String('.',rowLength));
            }

            foreach (var layer in original)
            {
                int z = layer.Key;
                List<string> newLayer = new List<string>();
                newLayer.Add(new String('.',rowLength));
                foreach (string line in layer.Value)
                {
                    newLayer.Add($".{line}.");
                }
                newLayer.Add(new String('.',rowLength));
                newMap.Add(z, newLayer);
            }

            return newMap;
        }

        public static Dictionary<int, Dictionary<int, List<string>>> ExpandHyperCube(Dictionary<int, Dictionary<int, List<string>>> original)
        {
            Dictionary<int, Dictionary<int, List<string>>> newMap = new();

            // gets w-dimension
            int minW = original.Keys.Min()-1;
            int maxW = original.Keys.Max()+1;
            int newZMin = 0;
            int newZMax = 0;
            int rowLength = 0;
            int rowCount = 0;

            foreach (var cube in original)
            {
                int w = cube.Key;

                Dictionary<int, List<string>> newCube = new();
                newZMin = cube.Value.Keys.Min()-1;
                newZMax = cube.Value.Keys.Max()+1;

                rowLength = cube.Value.First().Value[0].Length+2;
                rowCount = cube.Value.First().Value.Count+2;

                newCube.Add(newZMin, new List<string>());
                newCube.Add(newZMax, new List<string>());

                for(int i = 0; i < rowCount; i++)
                {
                    newCube[newZMin].Add(new String('.',rowLength));
                    newCube[newZMax].Add(new String('.',rowLength));
                }

                foreach (var layer in cube.Value)
                {
                    int z = layer.Key;
                    List<string> newLayer = new List<string>();
                    newLayer.Add(new String('.',rowLength));
                    foreach (string line in layer.Value)
                    {
                        newLayer.Add($".{line}.");
                    }
                    newLayer.Add(new String('.',rowLength));
                    newCube.Add(z, newLayer);
                }

                newMap.Add(w, newCube);
            }

            Dictionary<int, List<string>> newMinW = new();
            Dictionary<int, List<string>> newMaxW = new();

            for (int i = newZMin; i <= newZMax; i++)
            {
                newMinW.Add(i, new List<string>());
                newMaxW.Add(i, new List<string>());

                for(int j = 0; j < rowCount; j++)
                {
                    newMinW[i].Add(new String('.',rowLength));
                    newMaxW[i].Add(new String('.',rowLength));
                }
            }
            newMap.Add(minW, newMinW);
            newMap.Add(maxW, newMaxW);

            return newMap;
        }
    }
}