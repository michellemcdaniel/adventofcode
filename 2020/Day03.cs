using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventofcode
{
    class Day03
    {
        public static void Execute()
        {
            List<string> inputMap = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "input", "day03.txt")).ToList();
            List<Slope> slopes = new List<Slope>()
            {
                new Slope(1,1),
                new Slope(3,1),
                new Slope(5,1),
                new Slope(7,1),
                new Slope(1,2)
            };

            int currentRow = 0;


            foreach(string row in inputMap)
            {
                if (currentRow == 0)
                {
                    currentRow++;
                    continue;
                }

                char[] rowAsCharArray = row.ToCharArray();

                foreach (Slope slope in slopes)
                {
                    if (currentRow % slope.Down == 0)
                    {
                        slope.CurrentIndex = (slope.CurrentIndex+slope.Right)%row.Length;
                        
                        if (rowAsCharArray[slope.CurrentIndex] == '#')
                        {
                            slope.Trees++;
                        }
                    }
                }
                currentRow++;
            }

            long totalTrees = 1;

            foreach(Slope slope in slopes)
            {
                Console.WriteLine($"Trees: {slope.Trees}");
                totalTrees = totalTrees * slope.Trees;
            }

            Console.WriteLine($"Total trees: {totalTrees}");
        }
    }

    class Slope
    {
        public int Right { get; }
        public int Down { get; }
        public int Trees { get; set; }

        public int CurrentIndex { get; set; }
        public Slope(int right, int down)
        {
            Right = right;
            Down = down;
        }
    }
}
