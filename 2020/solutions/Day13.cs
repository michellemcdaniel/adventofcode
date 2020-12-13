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
            Dictionary<int,long> map = new Dictionary<int,long>();

            for (int i = 0; i < buses.Count(); i++)
            {
                if (buses[i] != "x")
                {
                    map.Add(i, long.Parse(buses[i]));
                }
            }

            long closestBus = 0;
            long time = long.MaxValue;
            
            foreach (var kvp in map)
            {
                int multiplier = 1;

                while (true)
                {
                    if (kvp.Value*multiplier > timestamp)
                    {
                        if (kvp.Value*multiplier - timestamp < time)
                        {
                            closestBus = kvp.Value;
                            time = kvp.Value*multiplier - timestamp;
                        }
                        break;
                    }
                    multiplier++;
                }
            }

            Console.WriteLine($"Closest bus: {closestBus}; time difference: {time}; {closestBus*time}");

            long meetMultiplier = map.First().Value;
            int mapIndex = 1;
            long meetTime = 0;

            while (mapIndex < map.Count())
            {
                meetTime += meetMultiplier;
                
                (int i, long val) = map.ElementAt(mapIndex);

                if ((meetTime+i)%val == 0)
                {
                    meetMultiplier*=val;
                    mapIndex++;
                }
            }

            Console.WriteLine($"Time to meet is {meetTime}");
        }
    }
}