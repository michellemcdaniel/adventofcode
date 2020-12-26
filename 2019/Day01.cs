using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day01
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();

            long totalFuel = 0;
            foreach(string module in input)
            {
                if(Int32.TryParse(module, out int weight))
                {
                    int fuelNeeded = weight/3-2;
                    totalFuel += fuelNeeded;

                    while (fuelNeeded > 0)
                    {
                        fuelNeeded = fuelNeeded/3-2;
                        if (fuelNeeded > 0)
                        {
                            totalFuel += fuelNeeded;
                        }
                    }
                }
            }

            Console.WriteLine($"Total fuel requirements: {totalFuel}");
        }
    }
}
