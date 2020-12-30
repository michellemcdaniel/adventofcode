using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Nineteen
{
    public class Day19
    {
        public static void Execute(string filename)
        {
            string input = File.ReadAllText(filename);

            IntCode intCode = new IntCode(input.Split(",").ToList().Select(o => long.Parse(o)).ToArray(), true);
            char[,] tractorBeam = new char[50,50];
            int count = 0;

            int lastRow = 0;
            int previousCountPound = 1;
            int previousPoundStart = 0;

            for (int i = 0; i < tractorBeam.GetLength(0); i++)
            {
                int countPound = 0;
                bool foundPound = false;
                int iterations = i <= 5 ? 10 : previousCountPound*4;
                
                previousCountPound = 0;
                for (int j = previousPoundStart; j < previousPoundStart+iterations && j < tractorBeam.GetLength(1); j++)
                {
                    if (j > 0 && tractorBeam[i,j-1] == '.' && foundPound)
                    {
                        break;
                    }
                    intCode.Restart();
                    intCode.Resume(new List<int>(){j, i});
                    long x = intCode.Compute();
                    
                    switch(x)
                    {
                        case 0:
                            tractorBeam[i,j] = '.';
                            break;
                        case 1:
                            if (!foundPound) previousPoundStart = j;
                            previousCountPound++;
                            tractorBeam[i,j] = '#';
                            countPound++;
                            foundPound = true;
                            count++;
                            break;
                        default:
                            throw new ArgumentException($"Unknown coordinate {i},{j}");
                    }
                }

                if (countPound == 0 && i > 5)
                {
                    lastRow = i;
                    break;
                }
            }

            DrawMap(tractorBeam, lastRow);
            Console.WriteLine($"Part One: {count}");
            
            previousPoundStart = 1000;
            int row = -1;
            int col = -1;
            for (int i = 731; i < 10_000; i++)
            {
                for (int j = previousPoundStart; j < previousPoundStart+100; j++)
                {
                    intCode.Restart();
                    intCode.Resume(new List<int>(){j, i});

                    if (intCode.Compute() == 1)
                    {
                        intCode.Restart();
                        previousPoundStart = j;
                        intCode.Resume(new List<int>(){j+99, i-99}); // 100 rows up, 100 rows to the right
                        if (intCode.Compute() == 1)
                        {
                            row = i-99;
                            col = j;
                        }
                        break;
                    }
                }

                if (row != -1 && col != -1)
                {
                    break;
                }
            }

            Console.WriteLine($"Part Two: {col}*10000+{row} = {col*10_000+row}");
        }

        public static void DrawMap(char[,] map, int last)
        {
            for (int i = 0; i < last; i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i,j] == 0)
                    {
                        Console.Write(' ');
                    }
                    Console.Write(map[i,j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
