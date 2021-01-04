using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Eighteen
{
    public class Day02
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();

            HashSet<string> two = new();
            HashSet<string> three = new();

            foreach(var i in input)
            {
                foreach(var c in i)
                {
                    if (i.Count(x => x == c) == 2)
                    {
                        two.Add(i);
                    }
                    else if (i.Count(x => x == c) == 3)
                    {
                        three.Add(i);
                    }
                }
            }

            Console.WriteLine($"Part One: {two.Count()*three.Count()}");

            foreach(var i in input)
            {
                foreach(var j in input)
                {
                    if (i == j) continue;
                    int countDiff = 0;
                    int foundChar = 0;

                    for (int x = 0; x < i.Length; x++)
                    {
                        if (i[x] != j[x])
                        {
                            countDiff++;
                            foundChar = x;
                        }
                        if (countDiff > 1) break;
                    }

                    if (countDiff == 1)
                    {
                        Console.Write($"Part Two: {i.Remove(foundChar,1)}");
                        return;
                    }
                }
            }
        }
    }
}
