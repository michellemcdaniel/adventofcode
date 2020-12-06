using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day06
    {
        public static void Execute()
        {
            List<string> input = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "input", "day06.txt")).ToList();
            input.Add("");

            HashSet<char> allAnswered = new HashSet<char>{ 'a', 'b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z' };
            HashSet<char> oneAnswered = new HashSet<char>();

            int totalAllAnswered = 0;
            int totalOneAnswered = 0;

            foreach(string line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    totalAllAnswered += allAnswered.Count;
                    totalOneAnswered += oneAnswered.Count;

                    oneAnswered = new HashSet<char>();
                    allAnswered = new HashSet<char>{ 'a', 'b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z' };
                    continue;
                }

                foreach (char c in line)
                {
                    oneAnswered.Add(c);
                }

                foreach (char c in allAnswered)
                {
                    if (!line.Contains(c))
                    {
                        allAnswered.Remove(c);
                    }
                }
            }

            Console.WriteLine($"One answered: {totalOneAnswered}");
            Console.WriteLine($"All answered: {totalAllAnswered}");
        }
    }
}