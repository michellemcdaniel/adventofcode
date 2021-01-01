using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Nineteen
{
    public class Day21
    {
        public static void Execute(string filename)
        {
            string input = File.ReadAllText(filename);

            IntCode intCode = new IntCode(input.Split(",").ToList().Select(o => long.Parse(o)).ToArray(), true);

            List<string> instructions = new List<string>();
            instructions.Add("NOT A J");
            instructions.Add("NOT B T");
            instructions.Add("OR T J");
            instructions.Add("NOT C T");
            instructions.Add("OR T J");
            instructions.Add("AND D J");
            instructions.Add("WALK");

            List<int> instructionValues = ConvertInstructions(instructions);
            long val = RunIntCode(instructionValues, intCode);            

            Console.WriteLine($"Part One: {val}");

            instructions = new List<string>();
            instructions.Add("NOT A J");
            instructions.Add("NOT B T");
            instructions.Add("OR T J");
            instructions.Add("NOT C T");
            instructions.Add("AND H T");
            instructions.Add("OR T J");
            instructions.Add("AND D J");
            instructions.Add("RUN");

            instructionValues = ConvertInstructions(instructions);
            val = RunIntCode(instructionValues, intCode);
            Console.WriteLine($"Part Two: {val}");
        }

        public static List<int> ConvertInstructions(List<string> instructions)
        {
            List<int> instructionValues = new();
            foreach(var inst in instructions)
            {
                foreach(var letter in inst)
                {
                    instructionValues.Add(Convert.ToInt32(letter));
                }
                instructionValues.Add(Convert.ToInt32('\n'));
            }

            return instructionValues;
        }

        public static long RunIntCode(List<int> instructions, IntCode intCode)
        {
            intCode.Restart();
            intCode.Resume(instructions);
            long val = 0;

            while (!intCode.Halted)
            {
                intCode.Resume();
                val = intCode.Compute();
                if (!intCode.Halted)
                    Console.Write((char)val);
            }

            return val;
        }
    }
}
