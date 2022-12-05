using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.TwentyTwo
{
    public class Day05
    {
        public static void Execute(string filename)
        {
            string[] allLines = File.ReadAllLines(filename);

            List<string> stacks = new();
            List<string> stacks9001 = new();
            List<string> instructions = new();
            bool change = false;

            foreach(var line in allLines)
            {
                if (string.IsNullOrEmpty(line))
                {
                    change = true;
                    continue;
                }

                if (change)
                {
                    instructions.Add(line);
                }
                else
                {
                    stacks.Add(line);
                    stacks9001.Add(line);
                }
            }

            foreach (var instruction in instructions)
            {

                RegexHelper.Match(instruction, @"move (\d+) from (\d) to (\d)", out int count, out int from, out int to);
                from--;
                to--;

                for (int i = 0; i < count; i++)
                {
                    char letter = stacks[from].Last();
                    stacks[from] = stacks[from].Remove(stacks[from].Length - 1, 1);
                    stacks[to] += letter;
                }

                string move = stacks9001[from].Substring(stacks9001[from].Length - count, count);
                stacks9001[from] = stacks9001[from].Remove(stacks9001[from].Length - count, count);
                stacks9001[to] += move;
            }

            string partOne = "";
            string partTwo = "";
            foreach (var stack in stacks)
            {
                partOne += stack.Last();
            }
            foreach (var stack in stacks9001)
            {
                partTwo += stack.Last();
            }

            Console.WriteLine($"Part one: {partOne}");
            Console.WriteLine($"Part two: {partTwo}");
        }
    }
}
