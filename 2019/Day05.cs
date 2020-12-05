using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day05
    {
        public static void Execute()
        {
            List<string> input = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "input", "day05.txt")).ToList();
            foreach(string intcode in input)
            {
                int[] opcodes = intcode.Split(",").ToList().Select(o => Int32.Parse(o)).ToArray();
                IntCode intCode = new IntCode(opcodes);
                Console.WriteLine(intCode.Compute(5));
            }
        }
    }
}
