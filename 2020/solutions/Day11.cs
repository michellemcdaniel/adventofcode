using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day11
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();

            List<string> previousLayout = new List<string>(input);
            List<string> currentLayout = new List<string>(input);

            List<string> difference = new List<string>(input);

            while(difference.Any())
            {
                previousLayout = new List<string>(currentLayout);
                for (int i = 0; i < currentLayout.Count(); i++)
                {
                    for (int j = 0; j < currentLayout[i].Length; j++)
                    {
                        if (currentLayout[i][j] == '.')
                        {
                            continue;
                        }

                        int countOccupied = SeatsOccupiedAroundCurrent(i, j, false, previousLayout);

                        if (countOccupied >= 4 && currentLayout[i][j] == '#')
                        {
                            currentLayout[i] = currentLayout[i] = GetReplacementString(currentLayout[i], j, 'L');;
                        }
                        else if (currentLayout[i][j] == 'L' && countOccupied == 0)
                        {
                            currentLayout[i] = GetReplacementString(currentLayout[i], j, '#');
                        }
                    }
                }
                
                difference = previousLayout.Except(currentLayout).ToList();
            }

            int count = 0;
            foreach(string line in currentLayout)
            {
                count += line.Where(c => c == '#').Count();
            }

            Console.WriteLine($"Part one: {count}");

            previousLayout = new List<string>(input);
            currentLayout = new List<string>(input);

            difference = new List<string>(input);

            while(difference.Any())
            {
                previousLayout = new List<string>(currentLayout);
                for (int i = 0; i < currentLayout.Count(); i++)
                {
                    for (int j = 0; j < currentLayout[i].Length; j++)
                    {
                        if (currentLayout[i][j] == '.')
                        {
                            continue;
                        }

                        int countOccupied = SeatsOccupiedAroundCurrent(i, j, true, previousLayout);

                        if (countOccupied >= 5 && currentLayout[i][j] == '#')
                        {
                            currentLayout[i] = currentLayout[i] = GetReplacementString(currentLayout[i], j, 'L');;
                        }
                        else if (currentLayout[i][j] == 'L' && countOccupied == 0)
                        {
                            currentLayout[i] = GetReplacementString(currentLayout[i], j, '#');
                        }
                    }
                }
                difference = previousLayout.Except(currentLayout).ToList();
            }

            count = 0;
            foreach(string line in currentLayout)
            {
                count += line.Where(c => c == '#').Count();
            }           
            
            Console.WriteLine($"Part two: {count}");
        }

        public static string GetReplacementString(string line, int index, char replacement)
        {
            char[] newLine = line.ToCharArray();
            newLine[index] = replacement;
            return new string(newLine);
        }

        public static int SeatOccupied(int i, int j, List<string> seats)
        {
            if (i >= seats.Count() || i < 0 || j >= seats[i].Length || j < 0 || seats[i][j] == 'L')
            {
                return -1;
            }

            else if (seats[i][j] == '#')
            {
                return 1;
            }

            return 0;
        }

        public static int SeatsOccupiedAroundCurrent(int i, int j, bool useMultiplier, List<string> seats)
        {
            int count = 0;
            for (int x = -1; x < 2; x++)
            {
                for (int y = -1; y < 2; y++)
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    int multiplier = 1;

                    while (true)
                    {
                        int xIndex = i + x * multiplier;
                        int yIndex = j + y * multiplier;
                        
                        if (xIndex >= seats.Count() || xIndex < 0 || yIndex >= seats[xIndex].Length || yIndex < 0 || seats[xIndex][yIndex] == 'L')
                        {
                            break;
                        }
                        else if (seats[xIndex][yIndex] == '#')
                        {
                            count++;
                            break;
                        }
                        
                        if (!useMultiplier)
                        {
                            break;
                        }
                        else
                        {
                            multiplier++;
                        }
                    }
                }
            }

            return count;
        }
    }
}