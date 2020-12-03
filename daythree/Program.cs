using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace daythree
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> inputMap = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "input", "input.txt")).ToList();
            List<Slope> slopes = new List<Slope>()
            {
                new Slope(1,1),
                new Slope(3,1),
                new Slope(5,1),
                new Slope(7,1),
                new Slope(1,2)
            };

            int totalTrees = 1;

            foreach (Slope slope in slopes)
            {
                int index = 0;
                int currentRow = 0;

                int trees = 0;
                int free = 0;

                foreach(string row in inputMap)
                {
                    // Handle slopes that are not down 1
                    if (currentRow != slope.Down)
                    {
                        currentRow++;
                        continue;
                    }

                    currentRow = 1;

                    index = (index+slope.Right)%row.Length;
                    char[] rowAsCharArray = row.ToCharArray();
                    if (rowAsCharArray[index] == '#')
                    {
                        trees++;
                    }
                    else
                    {
                        free++;
                    }
                }

                Console.WriteLine($"For Slope {slope.Right},{slope.Down} -- Trees: {trees}; Free: {free}");
                slope.Trees = trees;
                totalTrees = totalTrees*slope.Trees;
            }
            Console.WriteLine($"All multiplied: {totalTrees}");
        }
    }

    class Slope
    {
        public int Right { get; }
        public int Down { get; }
        public int Trees { get; set; }

        public Slope(int right, int down)
        {
            Right = right;
            Down = down;
        }
    }
}
