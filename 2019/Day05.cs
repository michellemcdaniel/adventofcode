using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day05
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();
            foreach(string intcode in input)
            {
                long[] opcodes = intcode.Split(",").ToList().Select(o => long.Parse(o)).ToArray();
                IntCode partOne = new IntCode(opcodes);
                Console.WriteLine($"Part One: {partOne.Compute(1)}");
                IntCode intCode = new IntCode(opcodes);
                Console.WriteLine($"Part Two: {intCode.Compute(5)}");
            }
        }
    }
}
