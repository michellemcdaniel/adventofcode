using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day02
    {
        public static void Execute()
        {
            List<string> input = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "input", "day02.txt")).ToList();

            foreach(string intcode in input)
            {
                bool found = false;
                for (int noun = 0; noun < 100; noun++)
                {
                    for (int verb = 0; verb < 100; verb++)
                    {
                        int[] opcodes = intcode.Split(",").ToList().Select(o => Int32.Parse(o)).ToArray();

                        opcodes[1] = noun;
                        opcodes[2] = verb;

                        IntCode intCode = new IntCode(opcodes);

                        if (intCode.Compute(0) == 19690720)
                        {
                            Console.WriteLine($"100 * {noun} + {verb} = {100*noun + verb}");
                            found = true;
                            break;
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
}
