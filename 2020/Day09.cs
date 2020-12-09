using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventofcode
{
    class Day09
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();
            List<long> inputAsInts = input.Select(i => long.Parse(i)).ToList();

            long partOne = 0;
            long partTwo = 0;
            int partOneIndex = 0;

            for (int i = 25; i < inputAsInts.Count(); i++)
            {
                bool foundGood = false;

                for (int j = i-25; j < i; j++)
                {
                    for (int k = i-25; k < i; k++)
                    {
                        if (k == j)
                        {
                            continue;
                        }
                        if (inputAsInts[j]+inputAsInts[k] == inputAsInts[i])
                        {
                            foundGood = true;
                            break;
                        }
                    }
                }

                if (!foundGood)
                {
                    partOne = inputAsInts[i];
                    partOneIndex = i;
                    break;
                }
            }

            for (int bufferSize = 2; bufferSize < inputAsInts.Count(); bufferSize++)
            {
                bool found = false;
                for (int i = 0; i < inputAsInts.Count()-bufferSize; i++)
                {
                    // If the range includes part one's index, we don't need to even consider it.
                    if (partOneIndex >= i && partOneIndex < i+bufferSize)
                    {
                        i = partOneIndex;
                        continue;
                    }

                    if (inputAsInts.GetRange(i, bufferSize).Sum() == partOne)
                    {
                        partTwo = inputAsInts.GetRange(i, bufferSize).Min() + inputAsInts.GetRange(i, bufferSize).Max();
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    break;
                }
            }

            Console.WriteLine($"Part one: {partOne}");
            Console.WriteLine($"Part two: {partTwo}");
        }
    }
}