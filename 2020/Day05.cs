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
                char[] rowArray = seat.Take(7).ToArray();
                char[] columnArray = seat.Skip(7).Take(3).ToArray();

                int rowMax = 127;
                int rowMin = 0;

                foreach(char rowChar in rowArray)
                {
                    switch (rowChar)
                    {
                        case 'F':
                            rowMax = (rowMax+rowMin+1)/2-1;
                            break;
                        case 'B':
                            rowMin = (rowMax+rowMin+1)/2;
                            break;
                        default:
                            break;
                    }
                }

                int columnMax = 7;
                int columnMin = 0;
                
                foreach(char columnChar in columnArray)
                {
                    switch (columnChar)
                    {
                        case 'L':
                            columnMax = (columnMax+columnMin+1)/2 - 1;
                            break;
                        case 'R':
                            columnMin = (columnMax+columnMin+1)/2;
                            break;
                        default:
                            break;
                    }
                }

                int id = rowMax*8+columnMax;
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