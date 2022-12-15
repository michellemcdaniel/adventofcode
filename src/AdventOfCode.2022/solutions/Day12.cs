using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.TwentyTwo
{
    public class Day12
    {
        public static void Execute(string filename)
        {
            // path finding
            string[] input = File.ReadAllLines(filename);
            List<(int row, int col)> startingPositions = new();

            // find S, and all starting locations
            (int row, int col) = (0,0);
            for (int i = 0; i < input.Length; i++)
            {
                for (int j = 0; j < input[i].Length; j++)
                {
                    if (input[i][j] == 'S')
                    {
                        row = i;
                        col = j;
                        startingPositions.Add((i,j));
                    }
                    else if(input[i][j] == 'a')
                    {
                        startingPositions.Add((i,j));
                    }
                }
            }

            int pathLength = FindPathToEnd(input, row, col);

            List<int> trailScores = new();
            foreach(var startingPosition in startingPositions)
            {
                trailScores.Add(FindPathToEnd(input, startingPosition.row, startingPosition.col));
            }

            Console.WriteLine($"Part one: {pathLength}");
            Console.WriteLine($"Part two: {trailScores.Min()}");
        }

        public static int FindPathToEnd(string[] input, int startRow, int startCol)
        {
            // Probably need to do some sort of memoization. Ok, using a HashSet.
            Queue<(int,int)> nodesToProcess = new();
            int height = input.Length;
            int width = input[0].Length;
            
            // Set up memoization. We're basically going to keep track of the shortest distance from the starting point
            int[,] distances = new int[height, width];

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    distances[i,j] = int.MaxValue;
                }
            }

            distances[startRow,startCol] = 0;

            nodesToProcess.Enqueue((startRow, startCol));
            int distance = int.MaxValue;

            while (nodesToProcess.Any())
            {
                (int row, int col) = nodesToProcess.Dequeue();

                if (input[row][col] == 'E')
                {
                    distance = distances[row, col] < distance ? distances[row, col] : distance;
                    continue;
                }

                foreach(var neighbor in GetAvailableNeighbors(input, row, col))
                {
                    if (distances[neighbor.row, neighbor.col] > distances[row, col] + 1)
                    {
                        distances[neighbor.row, neighbor.col] = distances[row, col] + 1;
                        nodesToProcess.Enqueue((neighbor.row, neighbor.col));
                    }
                }
            }

            return distance;
        }

        public static List<(int row, int col)> GetAvailableNeighbors(string[] map, int x, int y)
        {
            List<(int, int)> neighbors = new();

            foreach ((int dx, int dy) in new List<(int, int)> {(0,-1), (0,1), (-1,0), (1,0)})
            {
                if (x+dx >= 0 && x+dx < map.Length && y+dy >= 0 && y+dy < map[0].Length)
                {
                    if (map[x][y] == 'S' && map[x+dx][y+dy] == 'a')
                    {
                        neighbors.Add((x+dx, y+dy));
                    }
                    else if (map[x][y] == 'z' && map[x+dx][y+dy] == 'E')
                    {
                        neighbors.Add((x+dx, y+dy));
                    }
                    else if (char.IsLower(map[x+dx][y+dy]) && map[x+dx][y+dy] <= map[x][y] + 1)
                    {
                        neighbors.Add((x+dx, y+dy));
                    }
                }
            }

            return neighbors;
        }
    }
}
