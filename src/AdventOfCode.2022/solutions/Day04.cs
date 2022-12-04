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

                var oneIntersectTwo = elfOneRange.Intersect(elfTwoRange);
                var twoIntersectOne = elfTwoRange.Intersect(elfOneRange);

                if (oneIntersectTwo.Count() == elfTwoRange.Count() || twoIntersectOne.Count() == elfOneRange.Count())
                {
                    fullyContains++;
                }

                if (oneIntersectTwo.Any() || twoIntersectOne.Any())
                {
                    overlap++;
                }
            }

            Console.WriteLine($"Part one: {fullyContains}");
            Console.WriteLine($"Part two: {overlap}");
        }
    }
}
