using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventofcode
{
    class Day01
    {
        public static void Execute()
        {
            List<string> inputNumbers = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "input", "day01.txt")).ToList();
            List<int> inputNumbersAsInts = inputNumbers.Select(i => Int32.Parse(i)).ToList();

            Dictionary<int, int> inputNumbersDict = new Dictionary<int, int>();

            foreach (int num in inputNumbersAsInts)
            {
                int val = 2020 - num;
                if (inputNumbersDict.ContainsKey(val))
                {
                    Console.WriteLine($"{num} * {val} = {num * val}");
                    break;
                }
                else
                {
                    inputNumbersDict[num] = val;
                }
            }

            List<int> lessThan1000 = inputNumbersAsInts.Where(i => i < 1000).ToList();

            bool found = false;

            foreach(int first in lessThan1000)
            {
                foreach (int second in lessThan1000)
                {
                    if (first != second)
                    {
                        int search = 2020 - (first + second);
                        if (inputNumbersAsInts.Contains(search))
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
