using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.TwentyTwo
{
    public class Day01
    {
        public static void Execute(string filename)
        {
            List<long> elfCalories = new();
            long currentCalories = 0;
            long minimum = 0;

            foreach (string line in File.ReadLines(filename))
            {
                if (string.IsNullOrEmpty(line))
                {
                    if (elfCalories.Count < 3)
                    {
                        elfCalories.Add(currentCalories);
                        minimum = elfCalories.Min();
                    }
                    else
                    {
                        if (currentCalories > minimum)
                        {
                            elfCalories.Remove(minimum);
                            elfCalories.Add(currentCalories);
                            minimum = elfCalories.Min();
                        }
                    }
                    currentCalories = 0;
                }
                else
                {
                    currentCalories += long.Parse(line);
                }
            }

            Console.WriteLine($"Part one: {elfCalories.Max()}");
            Console.WriteLine($"Part two: {elfCalories.Sum()}");
        }
    }
}
