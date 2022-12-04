using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.TwentyTwo
{
    public class Day04
    {
        public static void Execute(string filename)
        {
            int fullyContains = 0;
            int overlap = 0;
            foreach(var line in File.ReadLines(filename))
            {
                var pair = line.Split(",").Select(s => s.Split("-").Select(e => int.Parse(e)));
                var elfOneRange = Enumerable.Range(pair.First().First(), pair.First().Last() - pair.First().First() + 1);
                var elfTwoRange = Enumerable.Range(pair.Last().First(), pair.Last().Last() - pair.Last().First() + 1);

                var intersect = elfOneRange.Intersect(elfTwoRange);

                if (intersect.Count() == elfTwoRange.Count() || intersect.Count() == elfOneRange.Count())
                {
                    fullyContains++;
                }

                if (intersect.Any())
                {
                    overlap++;
                }
            }

            Console.WriteLine($"Part one: {fullyContains}");
            Console.WriteLine($"Part two: {overlap}");
        }
    }
}
