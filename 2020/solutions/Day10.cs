using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day10
    {
        public static Hashtable cache = new Hashtable();
        public static void Execute(string filename)
        {
            List<int> input = File.ReadAllLines(filename).Select(i => int.Parse(i)).ToList();

            input.Add(0);
            input.Sort();
            input.Add(input.Last()+3);

            int[] differences = new int[4];
            int previous = 0;

            HashSet<Node> nodes = new HashSet<Node>();
            List<Node> previousNodes = new List<Node>();

            // Build the graph
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

                differences[i-previous]++;
                previous = i;
            }

            Console.WriteLine($"Part one: {differences[1]*differences[3]}");
            Console.WriteLine($"Part two, recursive with cache: {CountPathsRecursive(nodes.First())}");
            Console.WriteLine($"Part two, non-recursive: {CountPathsNonRecursive(nodes)}");
        }

        public static long CountPathsRecursive(Node node)
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
                        total += (long)cache[child];
                    }
                    else
                    {
                        cache.Add(child, CountPathsRecursive(child));
                        total += (long)cache[child];
                    }
                }

                return total;
            }
        }

        public static long CountPathsNonRecursive(HashSet<Node> nodes)
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
            public HashSet<Node> Children { get; set; }

            public  Node(int num)
            {
                Number = num;
                Children = new HashSet<Node>();
            }
        }
    }
}