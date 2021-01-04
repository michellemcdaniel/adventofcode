using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Eighteen
{
    public class Day01
    {
        public static void Execute(string filename)
        {
            List<int> input = File.ReadAllLines(filename).Select(s => int.Parse(s)).ToList();

            Console.WriteLine($"Part One: {input.Sum()}");

            HashSet<int> frequencies = new();
            int val = 0;
            frequencies.Add(val);

            while (true)
            {
                foreach(int i in input)
                {
                    val += i;

                    if (!frequencies.Add(val))
                    {
                        Console.WriteLine($"Part Two: {val}");
                        return;
                    }
                }
            }
        }
    }
}
