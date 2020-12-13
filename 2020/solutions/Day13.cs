using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day13
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();
            
            int timestamp = int.Parse(input.First());
            List<string> buses = input.Last().Split(",").ToList();

            Dictionary<int, (int,long)> map = new Dictionary<int, (int,long)>();
            int index = 0;

            for (int i = 0; i < buses.Count(); i++)
            {
                if (buses[i] != "x")
                {
                    map.Add(index, (i, long.Parse(buses[i])));
                    index++;
                }
            }

            int closestBus = 0;
            int time = int.MaxValue;
            
            foreach (string busString in buses)
            {
                int multiplier = 1;

                if (busString == "x")
                {
                    continue;
                }
                int bus = int.Parse(busString);

                while (true)
                {
                    if (bus*multiplier > timestamp)
                    {
                        if (bus*multiplier - timestamp < time)
                        {
                            closestBus = bus;
                            time = bus*multiplier - timestamp;
                        }
                        break;
                    }
                    multiplier++;
                }
            }

            Console.WriteLine($"Closest bus: {closestBus}; time {time}; {closestBus*time}");

            (_, long meetMultiplier) = map.First().Value;
            int mapIndex = 1;
            bool found = false;
            long meetTime = 0;

            while (!found)
            {
                found = true;
                meetTime += meetMultiplier;
                
                foreach (var kvp in map)
                {
                    (int i, long val) = kvp.Value;
                    if (kvp.Key < mapIndex)
                    {
                        continue;
                    }
                    if ((meetTime+i)%val == 0)
                    {
                        meetMultiplier*=val;
                        mapIndex++;
                    }
                    else
                    {
                        found = false;
                        break;
                    }
                }
            }

            Console.WriteLine($"Time to meet is {meetTime}");
        }
    }
}