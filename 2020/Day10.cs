using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day10
    {
        public static Dictionary<int, long> cache = new Dictionary<int, long>();
        public static void Execute(string filename)
        {
            List<int> input = File.ReadAllLines(filename).Select(i => int.Parse(i)).ToList();

            input.Sort();
            input.Add(input.Last()+3);

            int diffOfOne = 0;
            int diffOfThree = 0;

            int previous = 0;

            List<Node> nodes = new List<Node>();
            nodes.Add(new Node(0));

            List<int> previousNodes = new List<int>();
            previousNodes.Add(0);

            foreach(var i in input)
            {
                nodes.Add(new Node(i));

                List<int> remove = new List<int>();

                foreach (var n in previousNodes)
                {
                    if (i - n <=3)
                    {
                        nodes.First(node => node.Number == n).Children.Add(i);
                    }
                    else
                    {
                        remove.Add(n);
                    }
                }

                previousNodes.Except(remove);
                previousNodes.Add(i);

                if (i - previous == 1)
                {
                    diffOfOne++;
                }
                else if (i - previous == 3)
                {
                    diffOfThree++;
                }

                previous = i;
            }

            Console.WriteLine($"Part one: {diffOfOne*(diffOfThree+1)}");
            Console.WriteLine($"Part two: {WalkTree(nodes.First(), nodes)}");
        }

        public static long WalkTree(Node node, List<Node> nodes)
        {
            if (!node.Children.Any())
            {
                return 1;
            }
            else
            {
                long total = 0;
                foreach(var child in node.Children)
                {
                    if (cache.ContainsKey(child))
                    {
                        total += cache[child];
                    }
                    else
                    {
                        cache[child] = WalkTree(nodes.First(n => n.Number == child), nodes);
                        total += cache[child];
                    }
                }

                return total;
            }
        }
    }

    public class Node
    {
        public int Number { get; set; }
        public List<int> Children { get; set; }

        public  Node(int num)
        {
            Number = num;
            Children = new List<int>();
        }
    }
}