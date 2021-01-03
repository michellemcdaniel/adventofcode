using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Nineteen
{
    public class Day25
    {
        public static void Execute(string filename)
        {
            string input = File.ReadAllText(filename);

            IntCode intCode = new IntCode(input.Split(",").ToList().Select(o => long.Parse(o)).ToArray(), true);

            List<string> userCommands = new List<string> {
                "north", "north", "east", "take antenna",
                "west", "south", "east", "take cake",
                "west", "south", "west", "west", "west", "take coin",
                "east", "east", "east", "east", "east", "east", "east", "take boulder",
                "north", "east"
            };

            List<int> asciiCommand = ConvertInstructions(userCommands);

            intCode.Resume(asciiCommand);
            while(!intCode.Halted)
            {
                intCode.Resume();
                Console.Write((char)intCode.Compute());
            }
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
    }
}
