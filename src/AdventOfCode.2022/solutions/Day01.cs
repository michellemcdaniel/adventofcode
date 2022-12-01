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

            foreach (string line in File.ReadLines(filename))
            {
                if (string.IsNullOrEmpty(line))
                {
                    elfCalories.Add(currentCalories);
                    currentCalories = 0;
                }
                else
                {
                    currentCalories += long.Parse(line);
                }
            }

            List<long> sortedCalories = elfCalories.OrderByDescending(c => c).ToList();
            long topThree = sortedCalories[0] + sortedCalories[1] + sortedCalories[2];

            Console.WriteLine($"Part one: {sortedCalories[0]}");
            Console.WriteLine($"Part two: {topThree}");
        }
    }
}
