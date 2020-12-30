using AdventOfCode.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode.Nineteen
{
    public class Day18
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();
            Dictionary<string,int> paths = new();
            char[,] map = new char[input.Count, input.First().Length];
            Dictionary<char, (int, int)> keyLocations = new();
            Dictionary<char, (int, int)> doorLocations = new();
            HashSet<char> keys = new();

            int x = 0;
            int y = 0;

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
                    
                    if (RegexHelper.Match(input[i][j].ToString(), @"^[a-z]$"))
                    {
                        keys.Add(input[i][j]);
                    }
                }
            }

            map = RemoveDeadEnds(map);
            DumpMap(map);

            Console.WriteLine(BFS(map, x, y));

            // rewrite the map for part 2
            map[x+1,y+1] = '@';
            map[x+1,y-1] = '@';
            map[x-1,y+1] = '@';
            map[x-1,y-1] = '@';
            map[x,y+1] = '#';
            map[x,y-1] = '#';
            map[x+1,y] = '#';
            map[x-1,y] = '#';
            map[x,y] = '#';

            DumpMap(map);

            HashSet<char> keysInUpperLeftQuadrant = new();
            HashSet<char> keysInUpperRightQuadrant = new();
            HashSet<char> keysInLowerLeftQuadrant = new();
            HashSet<char> keysInLowerRightQuadrant = new();
            
            // Find out which keys are in which quadrant.
            // Start a robot. If it hits a door, the next robot we start will be the robot whose quadrant has the key. Repeat.
            // If we have a split in the road, enqueue all possible paths. (never more than two, tbh). We need to do that path length checking thing.
            
        }

        public class Node
        {
            public int Row { get; set;}
            public int Column { get; set; }
            public uint Visited { get; set; }
            public int Distance { get; set; }
            public HashSet<char> VisistedChar { get; set; }

            public Node(int row, int col, uint visited, int distance, HashSet<char> vc)
            {
                Row = row;
                Column = col;
                Visited = visited;
                Distance = distance;
                VisistedChar = vc;
            }

            public override bool Equals(object obj)
            {   
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }
                
                Node compare = obj as Node;
                return (compare.Row == Row && compare.Column == Column && compare.Visited == Visited);
            }
            
            // override object.GetHashCode
            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 13;
                    hash = (hash*7) + Row.GetHashCode();
                    hash = (hash*7) + Column.GetHashCode();
                    hash = (hash*7) + Visited.GetHashCode();
                    return hash;
                }
            }
        }

        public static int BFS(char[,] map, int x, int y)
        {
            const uint allKeys = (1u << 26) - 1;
            Dictionary<Node, int> state = new();

            Queue<Node> nodesToProcess = new();
            nodesToProcess.Enqueue(new Node(x, y, 0,0, new HashSet<char>()));

            Dictionary<string, int> allPaths = new();

            while (nodesToProcess.Any())
            {
                Node node = nodesToProcess.Dequeue();
                char value = map[node.Row, node.Column];
                if (char.IsLetter(value) && char.IsLower(value))
                {
                    node.Visited |= (1u << (value - 'a')); // find the right bit to mark as visited
                    node.VisistedChar.Add(value); // also add the value to the char set for future printing.

                    if (node.Visited == allKeys)
                    {
                        // We found all the keys! Celebrate!
                        string path = string.Join("",node.VisistedChar);
                        
                        if (!allPaths.TryGetValue(path, out int distance))
                        {
                            Console.WriteLine($"Found solution: {path} {node.Distance}");
                            allPaths[path] = node.Distance;
                        }
                    }
                }
                else if (char.IsLetter(value) && char.IsUpper(value)
                    && (node.Visited & (1u << char.ToLower(value) - 'a')) == 0)
                {
                    // We found a door we don't have a key for
                    continue;
                }

                int currentDistance = int.MaxValue;
                if (!state.TryGetValue(node, out currentDistance))
                {
                    currentDistance = int.MaxValue;
                }
                if (node.Distance >= currentDistance)
                {
                    // We've been here before with the same keys collected, 
                    // but it was faster, so we should stop checking
                    continue;
                }

                state[node] = node.Distance;
                node.Distance++;

                foreach(var neighbor in GetAvailableNeighbors(map, node.Row, node.Column))
                {
                    nodesToProcess.Enqueue(new Node(neighbor.Item1, neighbor.Item2, node.Visited, node.Distance, new HashSet<char>(node.VisistedChar)));
                }
            }

            return allPaths.Values.Min(); // ran out of nodes without collecting everything?
        }

        public static char[,] RemoveDeadEnds(char[,] map)
        {
            Queue<(int, int, List<(int, int)>)> locationsWithThreeWalls = new();

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == '.' || (char.IsLetter(map[i,j]) && char.IsUpper(map[i,j])))
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

                foreach(var neighbor in neighbors)
                {
                    List<(int, int)> neighborOfNeighbors = GetAvailableNeighbors(map, neighbor.Item1, neighbor.Item2);
                    if (neighborOfNeighbors.Count == 1 && 
                        (map[neighbor.Item1, neighbor.Item2] == '.' || 
                            (char.IsLetter(map[neighbor.Item1,neighbor.Item2]) && char.IsUpper(map[neighbor.Item1,neighbor.Item2]))))
                    {
                        locationsWithThreeWalls.Enqueue((neighbor.Item1, neighbor.Item2, neighborOfNeighbors));
                    }
                }
            }

            return map;
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

        public static void DumpMap(char[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    Console.Write(map[i,j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
