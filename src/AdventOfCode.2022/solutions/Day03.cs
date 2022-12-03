using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.TwentyTwo
{
    public class Day03
    {
        public static void Execute(string filename)
        {
            List<string> allLines = File.ReadAllLines(filename).ToList();

            int totalPriority = 0;
            
            foreach (var line in allLines)
            {
                int middle = line.Length/2;
                string firstHalf = line.Substring(0,middle);
                string secondHalf = line.Substring(middle, middle);

                foreach (char letter in firstHalf)
                {
                    if (secondHalf.Contains(letter))
                    {
                        totalPriority += GetPriority(letter);
                        break;
                    }
                }
            }

            int badgePriority = 0;
            int read = 0;

            while(read*3 < allLines.Count)
            {
                string[] lines = allLines.GetRange(read*3,3).ToArray();
                foreach (var letter in lines[0])
                {
                    if (lines[1].Contains(letter) && lines[2].Contains(letter))
                    {
                        badgePriority += GetPriority(letter);
                        break;
                    }
                }
                read++;
            }

            Console.WriteLine($"Part one: {totalPriority}");
            Console.WriteLine($"Part two: {badgePriority}");
        }
        private static int GetPriority(char letter)
        {
            if (letter < 'a')
            {
                return letter - 'A' + 27;
            }
            else
            {
                return letter - 'a' + 1;
            }
        }
    }
}
