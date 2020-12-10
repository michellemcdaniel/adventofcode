using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day01
    {
        public static void Execute(string filename)
        {
            List<string> inputNumbers = File.ReadAllLines(filename).ToList();
            List<int> inputNumbersAsInts = inputNumbers.Select(i => Int32.Parse(i)).ToList();

            HashSet<int> processedNumbers = new HashSet<int>();

            foreach (int num in inputNumbersAsInts)
            {
                int val = 2020 - num;

                if (processedNumbers.Contains(val))
                {
                    Console.WriteLine($"{num} * {val} = {num * val}");
                    break;
                }
                else
                {
                    processedNumbers.Add(num);
                }
            }

            List<int> minimalInputs = inputNumbersAsInts.Where(i => i < 2020/2).ToList();

            HashSet<int> allNumbers = inputNumbersAsInts.ToHashSet();
            bool found = false;

            foreach(int first in minimalInputs)
            {
                foreach (int second in minimalInputs)
                {
                    if (first != second)
                    {
                        int search = 2020 - (first + second);
                        if (allNumbers.Contains(search))
                        {
                            Console.WriteLine($"{first} * {second} * {search} = {first * second * search}");
                            found = true;
                            break;
                        }
                    }
                }
                if (found)
                {
                    break;
                }
            }
        }
    }
}
