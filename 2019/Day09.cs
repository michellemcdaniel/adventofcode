using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day09
    {
        public static void Execute()
        {
            List<string> input = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "input", "day09.txt")).ToList();

            string inputString = string.Join("", input);

            IntCode partOne = new IntCode(inputString.Split(",").ToList().Select(o => long.Parse(o)).ToArray());
            long result = partOne.Compute(1);
            Console.WriteLine($"Part One: {result}");

            IntCode intCode = new IntCode(inputString.Split(",").ToList().Select(o => long.Parse(o)).ToArray());
            result = intCode.Compute(2);

            Console.WriteLine($"Part Two: {result}");
        }
    }
}
