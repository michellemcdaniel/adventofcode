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

            IntCode intCode = new IntCode(inputString.Split(",").ToList().Select(o => long.Parse(o)).ToArray());

            Queue<long> result = intCode.Compute(2);

            while(result.Any())
            {
                Console.Write($"{result.Dequeue()},");
            }
            Console.WriteLine();
        }
    }
}
