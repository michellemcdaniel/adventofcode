using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Twenty
{
    public class Day08
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();
            int accumulator = 0;

            CheckProgram(input, out accumulator);
            Console.WriteLine($"Part One: {accumulator}");

            for (int i = 0; i < input.Count(); i++)
            {
                string original = input[i];
                
                if (!input[i].StartsWith("acc"))
                {
                    input[i] = input[i].StartsWith("nop") ? input[i].Replace("nop", "jmp") : input[i].Replace("jmp", "nop");
                    
                    if (CheckProgram(input, out accumulator))
                    {
                        Console.WriteLine($"Part Two: {accumulator}");
                        break;
                    }

                    input[i] = original;
                }
            }
        }

        public static bool CheckProgram(List<string> input, out int accumulator)
        {
            HashSet<int> lineHit = new HashSet<int>();
            int instructionPointer = 0;
            accumulator = 0;

            while (instructionPointer < input.Count())
            {
                if (!lineHit.Add(instructionPointer))
                {
                    return false;
                }

                string[] lineSplit = input[instructionPointer].Split(" ");
                int value = int.Parse(lineSplit[1]);

                switch (lineSplit[0])
                {
                    case "acc":
                        accumulator += value;
                        instructionPointer++;
                        break;
                    case "jmp":
                        instructionPointer += value;
                        break;
                    case "nop":
                        instructionPointer++;
                        break;
                    default:
                        break;
                }
            }

            return true;
        }
    }
}