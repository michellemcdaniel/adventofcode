using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day15
    {
        public static void Execute(string filename)
        {
            List<int> input = File.ReadAllLines(filename)[0].Split(",").Select(n => int.Parse(n)).ToList();

            Dictionary<long, long> numbers = new Dictionary<long, long>();

            long previous = 0;
            long lastTime = 0;

            void RunRule(long number, long index)
            {
                previous = number;
                numbers.TryGetValue(number, out lastTime);
                numbers[number] = index;
            }

            long inputCount = 1;

            foreach (var i in input)
            {
                RunRule(i, inputCount);
                inputCount++;
            }

            for (long i = inputCount; i <= 30000000; i++)
            {
                if (lastTime == 0)
                {
                    RunRule(0, i);
                }
                else
                {
                    RunRule(numbers[previous] - lastTime, i);
                }

                if (i == 2020)
                {
                    Console.WriteLine($"Part One: {previous}");
                }
            }

            Console.WriteLine($"Part Two: {previous}");
        }
    }
}