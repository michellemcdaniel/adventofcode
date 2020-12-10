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
            Node zeroNode = new Node(0);
            nodes.Add(zeroNode);

            List<Node> previousNodes = new List<Node>();
            previousNodes.Add(zeroNode);

            foreach(var i in input)
            {
                Node newNode = new Node(i);
                nodes.Add(newNode);

                List<Node> remove = new List<Node>();

                foreach (var n in previousNodes)
                {
                    if (i - n.Number <=3)
                    {
                        n.Children.Add(newNode);
                    }
                    else
                    {
                        remove.Add(n);
                    }
                }

                previousNodes.RemoveAll(n => remove.Contains(n));
                previousNodes.Add(newNode);

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

            Console.WriteLine($"Part one: {diffOfOne*(diffOfThree)}");
            Console.WriteLine($"Part two, recursive with cache: {CountPathsRecursive(nodes.First(), nodes)}");
            Console.WriteLine($"Part two, non-recursive: {CountPathsNonRecursive(nodes)}");
        }

        public static long CountPathsRecursive(Node node, List<Node> nodes)
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
                    if (cache.ContainsKey(child.Number))
                    {
                        total += cache[child.Number];
                    }
                    else
                    {
                        cache[child.Number] = CountPathsRecursive(child, nodes);
                        total += cache[child.Number];
                    }
                }

                return total;
            }
        }

        public static long CountPathsNonRecursive(List<Node> nodes)
        {
            Dictionary<int, long> paths = nodes.ToDictionary(n => n.Number, value => (long) 0);
            paths[0] = 1;

            foreach(var node in nodes)
            {
                foreach (var child in node.Children)
                {
                    paths[child.Number] += paths[node.Number];
                }
            }

            return paths[nodes.Last().Number];
        }

        public class Node
        {
            public int Number { get; set; }
            public List<Node> Children { get; set; }

            public  Node(int num)
            {
                Number = num;
                Children = new List<Node>();
            }
        }
    }
}