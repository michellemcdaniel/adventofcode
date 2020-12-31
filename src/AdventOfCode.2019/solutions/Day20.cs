using AdventOfCode.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode.Nineteen
{
    public class Day20
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();
            Dictionary<string,int> paths = new();
            char[,] map = new char[input.Count, input.First().Length];
            Dictionary<char, List<(int,int)>> doors = new();
            Dictionary<(int,int), (char, bool)> doorLocations = new();
            Dictionary<string, char> keyMap = new();

            Console.WriteLine(input.Count);

            char keyChar = 'a';
            
            for (int i = 0; i < input.Count; i++)
            {
                for (int j = 0; j < input[i].Length; j++)
                {
                    map[i,j] = input[i][j];
                    if (char.IsLetter(map[i,j]))
                    {
                        string doorKey = "";
                        bool right = false;
                        bool below = false;
                        if (i < input.Count()-1 && char.IsLetter(input[i+1][j]))
                        {
                            doorKey = $"{map[i,j]}{input[i+1][j]}";
                            below = true;
                        }
                        else if (j < input[i].Length-1 && char.IsLetter(input[i][j+1]))
                        {
                            doorKey = $"{map[i,j]}{input[i][j+1]}";
                            right = true;
                        }

                        if (!string.IsNullOrEmpty(doorKey))
                        {
                            char currentChar;
                            if (!keyMap.TryGetValue(doorKey, out currentChar))
                            {
                                keyMap.Add(doorKey, keyChar);
                                currentChar = keyChar;
                                doors[currentChar] = new List<(int, int)>();
                                keyChar++;
                            }
                            if (i == 0)
                            {
                                doorLocations[(i+2,j)] = (currentChar, false);
                                doors[currentChar].Add((i+2,j));
                            }
                            else if (i == input.Count-2)
                            {
                                doorLocations[(i-1,j)] = (currentChar, false);
                                doors[currentChar].Add((i-1, j));
                            }
                            else if (below && input[i+2][j] == '.')
                            {
                                doorLocations[(i+2,j)] = (currentChar, true);
                                doors[currentChar].Add((i+2, j));
                            }
                            else if (below && input[i-1][j] == '.')
                            {
                                doorLocations[(i-1,j)] = (currentChar, true);
                                doors[currentChar].Add((i-1,j));
                            }
                            else if (j == 0)
                            {
                                doorLocations[(i,j+2)] = (currentChar, false);
                                doors[currentChar].Add((i,j+2));
                            }
                            else if (j == input[i].Length-2)
                            {
                                doorLocations[(i,j-1)] = (currentChar, false);
                                doors[currentChar].Add((i,j-1));
                            }
                            else if (right && input[i][j+2] == '.')
                            {
                                doorLocations[(i,j+2)] = (currentChar, true);
                                doors[currentChar].Add((i,j+2));
                            }
                            else if (right && input[i][j-1] == '.')
                            {
                                doorLocations[(i,j-1)] = (currentChar, true);
                                doors[currentChar].Add((i,j-1));
                            }
                        }
                    }
                }
            }

            foreach(var loc in doorLocations)
            {
                Console.WriteLine($"{loc.Key} {loc.Value}");
            }
            map = RemoveDeadEnds(map);
            DumpMap(map);

            (int startX, int startY) = doors[keyMap["AA"]].First();
            (int endX, int endY) = doors[keyMap["ZZ"]].First();

            Console.WriteLine($"Part One: {BFS(map, startX, startY, endX, endY, doors, doorLocations, false)}");
            Console.WriteLine($"Part One: {BFS(map, startX, startY, endX, endY, doors, doorLocations, true)}");
        }

        public class Node
        {
            public int Row { get; set;}
            public int Column { get; set; }
            public int Distance { get; set; }
            public List<char> VisistedChar { get; set; }
            public int Level { get; set; }

            public Node(int row, int col, int distance, List<char> vc, int level)
            {
                Row = row;
                Column = col;
                Distance = distance;
                VisistedChar = vc;
                Level = level;
            }

            public override bool Equals(object obj)
            {   
                if (obj == null || GetType() != obj.GetType())
                {
                    return false;
                }
                
                Node compare = obj as Node;
                return (compare.Row == Row && compare.Column == Column && compare.Level == Level);
            }
            
            // override object.GetHashCode
            public override int GetHashCode()
            {
                unchecked
                {
                    int hash = 13;
                    hash = (hash*7) + Row.GetHashCode();
                    hash = (hash*7) + Column.GetHashCode();
                    hash = (hash*7) + Level.GetHashCode();
                    return hash;
                }
            }
        }

        public static int BFS(char[,] map, int x, int y, int endX, int endY, Dictionary<char, List<(int, int)>> doors, Dictionary<(int,int), (char, bool)> doorLocations, bool levels)
        {
            Dictionary<Node, int> state = new();
            Queue<Node> nodesToProcess = new();
            nodesToProcess.Enqueue(new Node(x, y, 0, new List<char>(), 0));
            Dictionary<string, int> allPaths = new();
            int maxLevel = 25;

            while (nodesToProcess.Any())
            {
                Node node = nodesToProcess.Dequeue();
                char value = map[node.Row, node.Column];

                if (node.Row == endX && node.Column == endY)
                {
                    if (!levels || (levels && node.Level == 0))
                    {
                        Console.WriteLine(string.Join("", node.VisistedChar));
                        return node.Distance;
                    }
                    else
                    {
                        continue; // found ZZ but not on the right level. Not a good path.
                    }
                }
                else if ((levels && node.Level > maxLevel) || (levels && node.Level < 0))
                {
                    continue;
                }
                else if (levels && node.Row == x && node.Column == y && node.Level != 0)
                {
                    continue;
                }
                else
                {
                    int currentDistance = int.MaxValue;
                    if (!state.TryGetValue(node, out currentDistance))
                    {
                        currentDistance = int.MaxValue;
                    }
                    if (node.Distance >= currentDistance)
                    {
                        // We've been here before with the same doors gone through, 
                        // but it was faster, so we should stop checking
                        continue;
                    }


                    state[node] = node.Distance;
                    node.Distance++;

                    if (doorLocations.TryGetValue((node.Row, node.Column), out var key) && !(node.Row == x && node.Column == y))
                    {
                        node.VisistedChar.Add(key.Item1);
                        // node is a door
                        (int i, int j) = doors[key.Item1].Where(d => d != (node.Row, node.Column)).First();
                        
                        int change = key.Item2 ? 1 : -1;
                        //Console.WriteLine($"At door {key.Item1} {key.Item2}. Going to {(i,j)}. Level {node.Level+change}");
                        nodesToProcess.Enqueue(new Node(i,j, node.Distance, new List<char>(node.VisistedChar), node.Level+change));
                    }

                    foreach(var neighbor in GetAvailableNeighbors(map, node.Row, node.Column))
                    {
                        if (map[neighbor.Item1,neighbor.Item2] == '.')
                        {
                            nodesToProcess.Enqueue(new Node(neighbor.Item1, neighbor.Item2, node.Distance, new List<char>(node.VisistedChar), node.Level));
                        }
                    }
                }
            }

            return int.MaxValue; // ran out of nodes without collecting everything?
        }

        public static char[,] RemoveDeadEnds(char[,] map)
        {
            Queue<(int, int, List<(int, int)>)> deadEnds = new();

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (map[i, j] == '.' || (char.IsLetter(map[i,j]) && char.IsUpper(map[i,j])))
                    {
                        List<(int, int)> neighbors = GetAvailableNeighbors(map, i, j);

                        if (neighbors.Count == 1)
                        {
                            deadEnds.Enqueue((i,j, neighbors));
                        }
                    }
                }
            }

            while (deadEnds.Any())
            {
                (int i, int j, List<(int, int)> neighbors) = deadEnds.Dequeue();
                map[i,j] = '#';

                foreach(var neighbor in neighbors)
                {
                    (int x, int y) = neighbor;
                    List<(int, int)> neighborOfNeighbors = GetAvailableNeighbors(map, x, y);
                    if (neighborOfNeighbors.Count == 1 && 
                        (map[x, y] == '.' || 
                            (char.IsLetter(map[x,y]) && char.IsUpper(map[x,y]))))
                    {
                        deadEnds.Enqueue((x, y, neighborOfNeighbors));
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
