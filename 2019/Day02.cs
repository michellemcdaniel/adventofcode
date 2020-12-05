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

                        int currentOpcode = opcodes[0];
                        int currentLocation = 0;

                        while(currentOpcode != 99 && currentLocation < opcodes.Length)
                        {
                            int replaceLocation = opcodes[currentLocation+3];
                            int firstInputLocation = opcodes[currentLocation+1];
                            int secondInputLocation = opcodes[currentLocation+2];

                            switch (currentOpcode)
                            {
                                case 1:
                                    opcodes[replaceLocation] = opcodes[firstInputLocation] + opcodes[secondInputLocation];
                                    break;
                                case 2:
                                    opcodes[replaceLocation] = opcodes[firstInputLocation] * opcodes[secondInputLocation];
                                    break;
                                default:
                                    break;
                            }
                            currentLocation = currentLocation+4;
                            currentOpcode = opcodes[currentLocation];
                        }

                        if (opcodes[0] == 19690720)
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
