using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Nineteen
{
    public class Day23
    {
        public static void Execute(string filename)
        {
            string input = File.ReadAllText(filename);

            IntCode[] intCodes = new IntCode[50];
            for (int i = 0; i < intCodes.Length; i++)
            {
                intCodes[i] = new IntCode(input.Split(",").ToList().Select(o => long.Parse(o)).ToArray(), true);
                intCodes[i].Resume(new List<int>{i, -1});
            }

            long x = 0;
            long y = 0;
            long comp = 0;
            bool first = true;

            long NATX = 0;
            long NATY = 0;
            HashSet<long> NATYs = new HashSet<long>();

            while(true)
            {
                for (int i = 0; i < intCodes.Length; i++)
                {
                    intCodes[i].Resume();
                    comp = intCodes[i].Compute();
                    
                    if (intCodes[i].WaitForInput)
                    {
                        continue;
                    }

                    intCodes[i].Resume();
                    x = intCodes[i].Compute();
                    intCodes[i].Resume();
                    y = intCodes[i].Compute();

                    if (comp < 50)
                    {
                        intCodes[comp].Resume(new List<long> { x, y });
                    }
                    else if (comp == 255)
                    {
                        if (first)
                        {
                            Console.WriteLine($"Part One: {y}");
                            first = false;
                        }
                        NATX = x;
                        NATY = y;
                    }
                }

                if (intCodes.All(i => i.WaitForInput == true))
                {
                    if (!NATYs.Add(NATY))
                    {
                        break;
                    }
                    intCodes[0].Resume(new List<long> { NATX, NATY });
                }
            }

            Console.WriteLine($"Part Two: {y}");
        }
    }
}
