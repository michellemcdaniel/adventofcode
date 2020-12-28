using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Nineteen
{
    public class Day17
    {
        public static void Execute(string filename)
        {
            string input = File.ReadAllText(filename);

            IntCode intCode = new IntCode(input.Split(",").ToList().Select(o => long.Parse(o)).ToArray(), true);
            List<List<char>> map = new();
            map.Add(new List<char>());
            
            while(!intCode.Halted)
            {
                intCode.Resume();
                char c = (char)(intCode.Compute());

                if (c == '\n')
                {
                    if (map.Last().Count > 0)
                        map.Add(new List<char>());
                }
                else
                {
                    map.Last().Add(c);
                }
            }

            map.RemoveAt(map.Count - 1);

            int count = 0;

            char[,] floorplan = new char[map.Count(),map.First().Count()];
            for (int i = 0; i < floorplan.GetLength(0); i++)
            {
                for(int j = 0; j < floorplan.GetLength(1); j++)
                {
                    floorplan[i,j] = map[i][j];
                    if (map[i][j] == '#') count++;
                }
            }

            DrawMap(floorplan);
            Console.WriteLine($"Part One: {SumAlignmentParameters(floorplan)}");
            Console.WriteLine(count);

            intCode = new IntCode(input.Split(",").ToList().Select(o => long.Parse(o)).ToArray(), true);
            intCode.Opcodes[0] = 2;

            // A, B,A,B,C,C,B,A,C,A\n
            // L,10,R,8,R,6,R,10\n
            // L,12,R,8,L,12\n
            // L,10,R,8,R,8\n
            // n\n

            List<int> A = new List<int>() { 76, 44, 49, 48, 44,  82, 44, 56, 44, 82, 44, 54, 44, 82, 44, 49, 48, 10 };
            List<int> B = new List<int>() { 76, 44, 49, 50, 44, 82, 44, 56, 44, 76, 44, 49, 50, 10 };
            List<int> C = new List<int>() { 76, 44, 49, 48, 44, 82, 44, 56, 44, 82, 44, 56, 10 };
            List<int> routine = new List<int>() { 65, 44, 66, 44, 65, 44, 66, 44, 67, 44, 67, 44, 66, 44, 65, 44, 67, 44, 65, 10 };

            intCode.Resume(routine);
            intCode.Resume(A);
            intCode.Resume(B);
            intCode.Resume(C);
            intCode.Resume(new List<int>() {110, 10} );

            int iteration = 0;
            long output = 0;

            while (!intCode.Halted)
            {
                iteration++;
                output = intCode.Compute();

                if (intCode.Paused)
                {
                    Console.Write($"{(char)output}");
                    //break;
                }
                intCode.Resume();
            }
            Console.WriteLine();

            Console.WriteLine($"Part Two: {output}");
        }

        public static int SumAlignmentParameters(char[,] floorplan)
        {
            int alignmentParameters = 0;

            for (int i = 0; i < floorplan.GetLength(0); i++)
            {
                for (int j = 0; j < floorplan.GetLength(1); j++)
                {
                    if (floorplan[i,j] == '#')
                    {
                        int count = 0;
                        foreach(var neighbor in GetNeighbors(i,j))
                        {
                            if (neighbor.Item1 >= 0 && neighbor.Item1 < floorplan.GetLength(0)
                                && neighbor.Item2 >= 0 && neighbor.Item2 < floorplan.GetLength(1)
                                && floorplan[neighbor.Item1, neighbor.Item2] == '#')
                            {
                                count++;
                            }
                            else break;
                        }

                        if (count == 4)
                        {
                            alignmentParameters += i*j;
                        }
                    }
                }
            }

            return alignmentParameters;
        }

        public static List<(int, int)> GetNeighbors(int x, int y)
        {
            List<(int,int)> locations = new();
            for(int i = 1; i <= 4; i++)
            {
                (int dx, int dy) = GetDirection(i);
                locations.Add((x+dx, y+dy));
            }
            return locations;
        }

        public static (int dx, int dy) GetDirection(int direction)
        {
            switch(direction)
            {
                case 1:
                    return (-1,0);
                case 2:
                    return (1,0);
                case 3:
                    return (0,1);
                case 4:
                    return (0, -1);
                default:
                    throw new ArgumentException($"Unknown direction {direction}", "direction");
            }
        }

        public static void DrawMap(char[,] floorplan)
        {
            for (int i = 0; i < floorplan.GetLength(0); i++)
            {
                for (int j = 0; j < floorplan.GetLength(1); j++)
                {
                    Console.Write(floorplan[i,j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
