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
            List<int> emptySeats = new List<int>();

            for (int i = 0; i < 128; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    emptySeats.Add(i*8+j);
                }
            }

            foreach(string seat in input)
            {
                string seatInBinary = seat.Replace('F', '0').Replace('B','1').Replace('L','0').Replace('R','1');
                string row = new string(seatInBinary.Take(7).ToArray());
                string column = new string(seatInBinary.Skip(7).Take(3).ToArray());

                int id = Convert.ToInt32(row, 2)*8+Convert.ToInt32(column, 2);
                maxId = id > maxId ? id : maxId;
                minId = id < minId ? id : minId;

                emptySeats.Remove(id);
            }

            Console.WriteLine($"Max ID: {maxId}");

            foreach(int id in emptySeats.Where(s => s < maxId && s > minId))
            {
                if (!emptySeats.Contains(id-1) && !emptySeats.Contains(id+1))
                {
                    Console.WriteLine($"Potential Id: {id}");
                }
            }
        }
    }
}