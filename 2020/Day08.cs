using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventofcode
{
    class Day08
    {
        public static void Execute()
        {
            List<string> input = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "input", "day08.txt")).ToList();

            int accumulator = 0;

            CheckProgram(input, out accumulator);

            Console.WriteLine($"Part 1: {accumulator}");

            for (int i = 0; i < input.Count(); i++)
            {
                List<string> newInput = new List<string>(input);

                if (newInput[i].Contains("nop"))
                {
                    newInput[i] = newInput[i].Replace("nop", "jmp");
                }
                else if (newInput[i].Contains("jmp"))
                {
                    newInput[i] = newInput[i].Replace("jmp", "nop");
                }
                
                if (!CheckProgram(newInput, out accumulator))
                {
                    break;
                }
            }  
            
            Console.WriteLine($"Part 2: {accumulator}");
        }

        public static bool CheckProgram(List<string> input, out int accumulator)
        {
            HashSet<int> lineHit = new HashSet<int>();
            bool infinite = false;
            int currentLine = 0;
            accumulator = 0;

            while (!infinite && currentLine < input.Count())
            {
                if (lineHit.Contains(currentLine))
                {
                    infinite = true;
                    break;
                }

                lineHit.Add(currentLine);
                string[] lineSplit = input[currentLine].Split(" ");
                int value = int.Parse(lineSplit[1]);

                switch (lineSplit[0])
                {
                    case "acc":
                        accumulator += value;
                        currentLine++;
                        break;
                    case "jmp":
                        currentLine += value;
                        break;
                    case "nop":
                        currentLine++;
                        break;
                    default:
                        break;
                }
            }

            return infinite;
        }
    }
}