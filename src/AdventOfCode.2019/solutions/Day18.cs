using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode.Nineteen
{
    public class Day18
    {
        public static Dictionary<(Node,Node), int> memoized = new();
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();
            Dictionary<string,int> paths = new();
            char[,] map = new char[input.Count, input.First().Length];
            Dictionary<char, (int, int)> keyLocations = new();
            Dictionary<char, (int, int)> doorLocations = new();

            int x = 0;
            int y = 0;

            Dictionary<(int, int), Node> nodes = new();

            for (int i = 0; i < input.Count; i++)
            {
                for (int j = 0; j < input[i].Length; j++)
                {
                    map[i,j] = input[i][j];
                    if (input[i][j] == '@')
                    {
                        x = i;
                        y = j;
                    }
                    if (input[i][j] != '#')
                    {
                        nodes.Add((i,j), new Node(i, j, input[i][j]));
                    }
                }
            }

            Console.WriteLine($"Entrance is at {x},{y}.");

            map = RemoveDeadEnds(map, nodes, x, y);
            DumpMap(map, x, y);

            keyLocations = GetPatternLocations(map, @"^[a-z]$");
            doorLocations = GetPatternLocations(map, @"^[A-Z]$");

            //Dictionary<(int, int), Stack<Stack<(int, int)>>> pathsTaken = new();

            Queue<Node> needToCheck = new Queue<Node>();
            needToCheck.Enqueue(nodes[(x,y)]);

            // Create graph with all nodes.
            while (needToCheck.Any())
            {
                Node node = needToCheck.Dequeue();
                foreach (var neighbor in GetAvailableNeighbors(map, node.Row, node.Column))
                {
                    if (!node.HasEdge(neighbor.Item1, neighbor.Item2))
                    {
                        Node neighborNode = nodes[(neighbor.Item1, neighbor.Item2)];
                        Edge edge = new Edge(node, neighborNode, 1);
                        node.FromEdges.Add(edge);
                        neighborNode.ToEdges.Add(edge);
                        needToCheck.Enqueue(neighborNode);
                    }
                }
            }

            // Reduce graph
            needToCheck.Enqueue(nodes[(x,y)]);
            int iterations = 0;

            while (needToCheck.Any())
            {
                iterations++;
                Node node = needToCheck.Dequeue();
                List<Edge> newEdgeList = new();
                bool changed = false;
                foreach (var edge in node.FromEdges)
                {
                    Node neighbor = edge.To;
                    if (neighbor.Value == '.')
                    {
                        // Collapse edges
                        foreach (var neighborEdge in neighbor.FromEdges)
                        {
                            Edge newEdge = new Edge(node, neighborEdge.To, edge.Distance+neighborEdge.Distance);
                            newEdgeList.Add(newEdge);
                            neighborEdge.To.ToEdges.Remove(neighborEdge);
                            neighborEdge.To.ToEdges.Add(newEdge);
                        }
                        nodes.Remove((neighbor.Row, neighbor.Column));
                        changed = true;
                    }
                    else
                    {
                        if (neighbor.FromEdges.Any(e => e.To.Value == '.'))
                            needToCheck.Enqueue(neighbor);

                        if (edge.To.FromEdges.Any() || RegexHelper.Match(edge.To.Value.ToString(), @"^[a-z]$"))
                            newEdgeList.Add(edge);
                        else
                            nodes.Remove((edge.To.Row, edge.To.Column));
                    }
                }
                if (changed)
                {
                    needToCheck.Enqueue(node);
                    node.FromEdges = newEdgeList;
                }
            }

            foreach(var node in nodes)
            {
                node.Value.RemoveDuplicateEdges();
                Console.WriteLine($"{node.Key}: {node.Value.Value}");
                Console.WriteLine(node.Value.ListEdges());
            }

            HashSet<char> keysCollected = new();

            foreach(var path in PathsAvailable(nodes[(x,y)], keysCollected))
            {
                foreach(var node in path)
                {
                    Console.Write($"({node.Row},{node.Column}) -> ");
                }
                Console.WriteLine();
            }
            Console.WriteLine(keysCollected.Count());
        }

        public static List<List<Node>> PathsAvailable(Node start, HashSet<char> keysCollected)
        {
            List<List<Node>> paths = new();

            foreach(var edge in start.FromEdges)
            {
                if (char.IsUpper(edge.To.Value))
                {
                    continue; // can't go there yet.
                }
                else
                {
                    if (char.IsLetter(edge.To.Value))
                    {
                        keysCollected.Add(edge.To.Value);
                    }

                    List<List<Node>> neighborPaths = PathsAvailable(edge.To, keysCollected);
                    foreach(var path in neighborPaths)
                    {
                        path.Insert(0,start);
                        paths.Add(path);
                    }
                }
            }

            if (paths.Count == 0)
            {
                paths.Add(new List<Node>() {start});
            }

            return paths;
        }

        public static int ComputeDistances(Node start, Node end)
        {
            // Need to get the min distance from the current node to the target node
            Queue<Node> nodesToProcess = new();
            Dictionary<Node, int> distances = new();
            HashSet<Edge> edgesProcessed = new();
            nodesToProcess.Enqueue(start);
            distances.Add(start, 0);

            while(nodesToProcess.Any())
            {
                Node node = nodesToProcess.Dequeue();
                foreach (var edge in node.FromEdges)
                {
                    if (edgesProcessed.Contains(edge))
                    {
                        continue;
                    }
                    else
                    {
                        if (distances.TryGetValue(edge.To, out int d))
                        {
                            if (distances[node] + edge.Distance < d)
                            {
                                distances[edge.To] = distances[node] + edge.Distance;
                            }
                        }
                        else
                        {
                            distances.Add(edge.To, distances[node] + edge.Distance);
                        }
                        if (edge.To != end)
                        {
                            nodesToProcess.Enqueue(edge.To);
                        }

                        edgesProcessed.Add(edge);
                    }
                }

                foreach (var edge in node.ToEdges)
                {
                    if (edgesProcessed.Contains(edge))
                    {
                        continue;
                    }

                    if (memoized.TryGetValue((start,edge.From), out int dist))
                    {
                        distances[edge.From] = dist;
                    }
                    else
                    {
                        if (distances.TryGetValue(edge.From, out int d))
                        {
                            if (distances[node] + edge.Distance < d)
                            {
                                distances[edge.From] = distances[node] + edge.Distance;
                            }
                        }
                        else
                        {
                            distances.Add(edge.From, distances[node] + edge.Distance);
                        }
                    }

                    if (edge.To != end)
                    {
                        nodesToProcess.Enqueue(edge.From);
                    }

                    edgesProcessed.Add(edge);
                }
            }

            foreach (var node in distances)
            {
                memoized[(start,node.Key)] = node.Value; 
            }

            return distances[end];
        }

        public static bool MapContainsKeys(HashSet<char> keysInMap, HashSet<char> keysCollected)
        {
            return keysInMap.Except(keysCollected).Count() == 0;
        }

        public static char[,] RemoveDeadEnds(char[,] map, Dictionary<(int, int), Node> nodes, int x, int y)
        {
            Queue<(int, int, List<(int, int)>)> locationsWithThreeWalls = new();

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (i == x && j == y)
                    {
                        continue;
                    }
                    if (map[i, j] == '.')
                    {
                        List<(int, int)> neighbors = GetAvailableNeighbors(map, i, j);

                        if (neighbors.Count == 1)
                        {
                            locationsWithThreeWalls.Enqueue((i,j, neighbors));
                        }
                    }
                }
            }

            while (locationsWithThreeWalls.Any())
            {
                (int i, int j, List<(int, int)> neighbors) = locationsWithThreeWalls.Dequeue();
                map[i,j] = '#';
                nodes.Remove((i,j));

                foreach(var neighbor in neighbors)
                {
                    List<(int, int)> neighborOfNeighbors = GetAvailableNeighbors(map, neighbor.Item1, neighbor.Item2);
                    if (neighborOfNeighbors.Count == 1 && map[neighbor.Item1, neighbor.Item2] == '.' && !(neighbor.Item1 == x && neighbor.Item2 == y))
                    {
                        locationsWithThreeWalls.Enqueue((neighbor.Item1, neighbor.Item2, neighborOfNeighbors));
                    }
                }
            }

            return map;
        }

        public static Dictionary<char, (int,int)> GetPatternLocations(char[,] map, string pattern)
        {
            Dictionary<char, (int, int)> locations = new();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (RegexHelper.Match(map[i,j].ToString(), pattern))
                    {
                        locations.Add(map[i,j], (i, j));
                    }
                }
            }
            return locations;
        }

        public static List<(int, int)> GetAvailableNeighbors(char[,] map, int x, int y)
        {
            List<(int, int)> neighbors = new();

            for (int i = 1; i <=4; i++)
            {
                (int dx, int dy) = GetDirection(i);
                if (x+dx >= 0 && x+dx < map.GetLength(0) && y+dy >= 0 && y+dy < map.GetLength(1))
                {
                    if (map[x+dx, y+dy] != '#')
                    {
                        neighbors.Add((x+dx, y+dy));
                    }
                }
            }

            return neighbors;
        }

        public static int SwitchDirection(int i)
        {
            switch(i)
            {
                case 1: // Up
                    return 2;
                case 2: // Down
                    return 1;
                case 3: // Right
                    return 4;
                case 4: // Left
                    return 3;
                default:
                    // something went wrong
                    throw new ArgumentException($"Unkown direction {i}", "i");
            }
        }

        public static (int dx, int dy) GetDirection(int direction)
        {
            switch(direction)
            {
                case 1:
                    return (-1,0);
                case 2:
                    return (1,0);
                case 3:
                    return (0,1);
                case 4:
                    return (0, -1);
                default:
                    throw new ArgumentException($"Unknown direction {direction}", "direction");
            }
        }

        public static void DumpMap(char[,] map, int x, int y)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (i == x && j == y && map[i,j] != '@')
                    {
                        Console.Write("+");
                    }
                    else
                        Console.Write(map[i,j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public class Node
        {
            public int Row { get; set; }
            public int Column { get; set; }
            public char Value { get; set; }
            public List<Edge> ToEdges { get; set; }
            public List<Edge> FromEdges { get; set; }

            public Node(int row, int column, char value)
            {
                Row = row;
                Column = column;
                Value = value;
                ToEdges = new List<Edge>();
                FromEdges = new List<Edge>();
            }

            public bool HasEdge(int row, int column)
            {
                return ToEdges.Any(
                        e => (e.From.Row == row && e.From.Column == column)) 
                    || FromEdges.Any(
                        e => (e.To.Row == row && e.To.Column == column));
            }

            public List<Edge> AllEdges()
            {
                return FromEdges.Concat(ToEdges).ToList();
            }

            public void RemoveDuplicateEdges()
            {
                List<Edge> newTo = new List<Edge>();
                
                foreach (var edge in ToEdges)
                {
                    Edge dup = newTo.FirstOrDefault(e => e.From == edge.From);
                    if (dup != null)
                    {
                        dup.Distance = Math.Min(dup.Distance, edge.Distance);
                    }
                    else
                    {
                        newTo.Add(edge);
                    }
                }

                ToEdges = newTo;

                List<Edge> newFrom = new List<Edge>();
                foreach (var edge in FromEdges)
                {
                    Edge dup = newFrom.FirstOrDefault(e => e.To == edge.To);
                    if (dup != null)
                    {
                        dup.Distance = Math.Min(dup.Distance, edge.Distance);
                    }
                    else
                    {
                        newFrom.Add(edge);
                    }
                }

                FromEdges = newFrom;
            }

            public string ListEdges()
            {
                StringBuilder sb = new StringBuilder();
                foreach(var edge in FromEdges)
                {
                    sb.AppendLine($"\t({edge.From.Row},{edge.From.Column}) {edge.From.Value} -> ({edge.To.Row},{edge.To.Column}) {edge.To.Value} = {edge.Distance}");
                }
                foreach(var edge in ToEdges)
                {
                    sb.AppendLine($"\t({edge.From.Row},{edge.From.Column}) {edge.From.Value} -> ({edge.To.Row},{edge.To.Column}) {edge.To.Value} = {edge.Distance}");
                }
                return sb.ToString();
            }
        }

        public class Edge
        {
            public Node From { get; set; }
            public Node To { get; set; }
            public int Distance { get; set; }

            public Edge (Node from, Node to, int distance)
            {
                From = from;
                To = to;
                Distance = distance;
            }
        }
    }
}
