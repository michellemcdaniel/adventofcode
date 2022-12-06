using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.TwentyTwo
{
    public class Day06
    {
        public static void Execute(string filename)
        {
            Console.WriteLine($"Part one: {FindStart(File.ReadAllText(filename), 4)}");
            Console.WriteLine($"Part two: {FindStart(File.ReadAllText(filename), 14)}");

            Console.WriteLine($"Part one: {FindStartString(File.ReadAllText(filename), 4)}");
            Console.WriteLine($"Part two: {FindStartString(File.ReadAllText(filename), 14)}");
        }

        private static int FindStart(string input, int count)
        {
            Queue<char> stream = new Queue<char>();
            int position = 0;

            foreach(char letter in input)
            {
                position++;
                while (stream.Contains(letter))
                {
                    stream.Dequeue();
                }

                stream.Enqueue(letter);

                if (stream.Count == count)
                {
                    return position;
                }
            }

            return -1;
        }

        private static int FindStartString(string input, int count)
        {
            string start = "";
            int position = 0;

            while (position < input.Length)
            {
                int index = start.IndexOf(input[position]);
                if (index != -1)
                {
                    start = start.Substring(index+1);
                }
                start += input[position++];
                
                if (start.Length == count)
                {
                    return position;
                }
            }
            return -1;
        }
    }
}
