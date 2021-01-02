using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Nineteen
{
    public class Day24
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();
            bool[,] map = new bool[input.Count, input.First().Length];
            bool[,] initialMap = new bool[input.Count, input.First().Length];
            HashSet<int> maps = new();

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    switch(input[i][j])
                    {
                        case '#':
                            initialMap[i,j] = true;
                            break;
                        case '.':
                            initialMap[i,j] = false;
                            break;
                        default:
                            throw new ArgumentException($"Something wrong with input");
                    }
                }
            }

            map = initialMap;

            while(maps.Add(GetSignature(map)))
            {
                bool[,] newMap = new bool[map.GetLength(0), map.GetLength(1)];
                for (int i = 0; i < map.GetLength(0); i++)
                {
                    for (int j = 0; j < map.GetLength(1); j++)
                    {
                        newMap[i,j] = BugLives(map[i,j], ActiveAround(map, i, j));
                    }
                }

                map = newMap;
            }

            Console.WriteLine($"Part One: {CalculateBiodiversity(map)}");

            Dictionary<int, bool[,]> levels = new();
            Dictionary<int, int> signatures = new();
            int minLevel = 0; 
            int maxLevel = 0;
            levels.Add(0,initialMap);
            signatures.Add(0, GetSignature(initialMap));

            for (int round = 0; round < 200; round++)
            {
                // Add new levels if the current min/max level have bugs
                if (signatures[minLevel] != 0)
                {
                    minLevel--;
                    levels.Add(minLevel, new bool[initialMap.GetLength(0), initialMap.GetLength(1)]);
                    signatures.Add(minLevel, 0);
                }

                if (signatures[maxLevel] != 0)
                {
                    maxLevel++;
                    levels.Add(maxLevel, new bool[initialMap.GetLength(0), initialMap.GetLength(1)]);
                    signatures.Add(maxLevel, 0);
                }

                Dictionary<int, bool[,]> newLevels = new();

                foreach(var level in levels)
                {
                    map = level.Value;
                    bool[,] newMap = new bool[map.GetLength(0), map.GetLength(1)];
                    for (int i = 0; i < map.GetLength(0); i++)
                    {
                        for (int j = 0; j < map.GetLength(1); j++)
                        {
                            if (i == 2 && j == 2)
                            {
                                // We're just going to ignore all center points, and call them empty
                                newMap[i,j] = false;
                                continue;
                            }

                            levels.TryGetValue(level.Key-1, out bool[,] above);
                            levels.TryGetValue(level.Key+1, out bool[,] below);
                            newMap[i,j] = BugLives(map[i,j], RecursiveActiveAround(map, below, above, i, j));
                        }
                    }
                    newLevels.Add(level.Key, newMap);
                    signatures[level.Key] = GetSignature(newMap);
                }

                levels = newLevels;
            }

            int bugs = 0;
            foreach(var level in levels)
            {
                foreach(var b in level.Value)
                {
                    if (b) bugs++;
                }
            }

            Console.WriteLine($"Part Two: {bugs}");
        }

        public static void DumpMap(bool[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    switch(map[i,j])
                    {
                        case true:
                            Console.Write('#');
                            break;
                        case false:
                            Console.Write('.');
                            break;
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static long CalculateBiodiversity(bool[,] map)
        {
            long multiplier = 1;
            long biodiversity = 0;
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i,j]) biodiversity+=multiplier;
                    multiplier*=2;
                }
            }
            return biodiversity;
        }

        public static bool BugLives(bool current, int active)
        {
            if ((current && active == 1) ||
                (!current && (active == 1 || active == 2)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static int GetSignature(bool[,] map)
        {
            string sig = "";
            for(int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    switch (map[i,j])
                    {
                        case true:
                            sig += '1';
                            break;
                        case false:
                            sig += '0';
                            break;
                    }
                }
            }

            return Convert.ToInt32(sig,2);
        }

        public static bool Exists(bool[,] map, int y, int x)
        {
            return (y >= 0 && x >= 0 && x < map.GetLength(1) && y < map.GetLength(0));
        }

        public static bool IsOccupied(bool[,] map, int y, int x)
        {
            return Exists(map, y,x) && map[y,x];
        }

        public static int RecursiveActiveAround(bool[,] map, bool[,] below, bool[,] above, int y, int x)
        {
            int totalActive = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if ((i == 0 && j == 0)
                        || (i != 0 && j != 0))
                    {
                        continue;
                    }
                    else if (y+i == 2 && x+j == 2)
                    {
                        if (below != null)
                        {
                            // need to go recursively down to the next level and check the corresponding column/row
                            if (i == -1)
                            {
                                for (int b = 0; b < below.GetLength(1); b++)
                                {
                                    if (below[below.GetLength(0)-1,b]) totalActive++;
                                }
                            }
                            else if (i == 1)
                            {
                                for (int b = 0; b < below.GetLength(1); b++)
                                {
                                    if (below[0,b]) totalActive++;
                                }
                            }
                            else if (j == -1)
                            {
                                for (int b = 0; b < below.GetLength(0); b++)
                                {
                                    if (below[b,below.GetLength(1)-1]) totalActive++;
                                }
                            }
                            else if (j == 1)
                            {
                                for (int b = 0; b < below.GetLength(0); b++)
                                {
                                    if (below[b,0]) totalActive++;
                                }
                            }
                        }
                    }
                    else if (IsOccupied(map, y+i, x+j))
                    {
                        totalActive++;
                    }
                }
            }

            if (above != null)
            {
                if (y == 0)
                {
                    if (above[1,2]) totalActive++;
                }
                else if (y == map.GetLength(0)-1)
                {
                    if (above[3,2]) totalActive++;
                }
                
                if (x == 0)
                {
                    if (above[2,1]) totalActive++;
                }
                else if (x == map.GetLength(1)-1)
                {
                    if (above[2,3]) totalActive++;
                }
            }
            
            return totalActive;
        }

        public static int ActiveAround(bool[,] map, int y, int x)
        {
            int totalActive = 0;

            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if ((i == 0 && j == 0)
                        || (i != 0 && j != 0))
                    {
                        continue;
                    }
                    if (IsOccupied(map, y+i, x+j))
                    {
                        totalActive++;
                    }
                }
            }
            
            return totalActive;
        }
    }
}
