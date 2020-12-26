using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Twenty
{
    class Day05
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();

            int maxId = 0;
            int minId = 128*8+7;
            List<int> emptySeats = new List<int>(Enumerable.Range(0,128*8));

            foreach(string seat in input)
            {
                int id = Convert.ToInt32(seat.Replace('F', '0').Replace('B','1').Replace('L','0').Replace('R','1'), 2);
                maxId = Math.Max(id, maxId);
                minId = Math.Min(id, minId);

                emptySeats.Remove(id);
            }

            Console.WriteLine($"Part One: {maxId}");
            Console.WriteLine($"Part Two: {emptySeats.Where(s => s < maxId && s > minId).First()}");
        }
    }
}