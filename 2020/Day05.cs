using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventofcode
{
    class Day05
    {
        public static void Execute()
        {
            List<string> input = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "input", "day05.txt")).ToList();

            int maxId = 0;
            int minId = 128*8+7;
            List<int> emptySeats = new List<int>(Enumerable.Range(0,128*8));

            foreach(string seat in input)
            {
                string seatInBinary = seat.Replace('F', '0').Replace('B','1').Replace('L','0').Replace('R','1');

                int id = Convert.ToInt32(seatInBinary, 2);
                maxId = Math.Max(id, maxId);
                minId = Math.Min(id, minId);

                emptySeats.Remove(id);
            }

            Console.WriteLine($"Max ID: {maxId}");
            Console.WriteLine($"Potential Id: {emptySeats.Where(s => s < maxId && s > minId).First()}");
        }
    }
}