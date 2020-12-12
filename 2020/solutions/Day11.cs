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

            Dictionary<int, int> parts = new Dictionary<int, int>();
            parts.Add(1,4);
            //parts.Add(Math.Min(input.Count(), input.First().Length),5);

            foreach(var kvp in parts)
            {
                List<string> previousLayout = new List<string>(input);
                List<string> currentLayout = new List<string>(input);

                bool changed = true;
                while(changed)
                {
                    Console.Clear();
                    foreach(var line in currentLayout)
                    {
                        Console.WriteLine(line);
                    }

                    Thread.Sleep(100);
                    
                    changed = false;
                    previousLayout = new List<string>(currentLayout);
                    for (int i = 0; i < currentLayout.Count(); i++)
                    {
                        for (int j = 0; j < currentLayout[i].Length; j++)
                        {
                            if (currentLayout[i][j] == '.')
                            {
                                continue;
                            }

                            int countOccupied = SeatsOccupiedAroundCurrent(i, j, kvp.Key, previousLayout);

                            if (countOccupied >= kvp.Value && currentLayout[i][j] == '#')
                            {
                                currentLayout[i] = currentLayout[i] = GetReplacementString(currentLayout[i], j, 'L');
                                changed = true;
                            }
                            else if (currentLayout[i][j] == 'L' && countOccupied == 0)
                            {
                                currentLayout[i] = GetReplacementString(currentLayout[i], j, '#');
                                changed = true;
                            }
                        }
                    }
                }

                int count = 0;
                foreach(string line in currentLayout)
                {
                    count += line.Where(c => c == '#').Count();
                }

                Console.WriteLine($"Count: {count}");
            }
        }

        public static string GetReplacementString(string line, int index, char replacement)
        {
            char[] newLine = line.ToCharArray();
            newLine[index] = replacement;
            return new string(newLine);
        }

        public static int SeatsOccupiedAroundCurrent(int i, int j, int maxMultiplier, List<string> seats)
        {
            int count = 0;
            foreach (int x in new int[] {-1, 0, 1} )
            {
                foreach (int y in new int[] {-1, 0, 1} )
                {
                    if (x == 0 && y == 0)
                    {
                        continue;
                    }

                    int multiplier = 1;

                    while (multiplier <= maxMultiplier)
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
                        
                        multiplier++;
                    }
                }
            }

            return count;
        }
    }
}