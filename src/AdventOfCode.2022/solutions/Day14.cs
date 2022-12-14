using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.TwentyTwo
{
    public class Day14
    {
        public static void Execute(string filename)
        {
            // Draw diagram, but we don't know the min or max values, so let's just get in all the paths and then deal with them after
            List<RockPath> paths = new List<RockPath>();
            int minCol = int.MaxValue;
            int width = int.MinValue;
            int height = int.MinValue;

            foreach(var line in File.ReadLines(filename))
            {
                string[] split = line.Split(" -> ");
                RockPath path = new RockPath();
                foreach (var point in split)
                {
                    int[] points = point.Split(",").Select(s => int.Parse(s)).ToArray();
                    path.Paths.Add((points[0], points[1]));

                    if (points[0] > width)
                    {
                        width = points[0];
                    }
                    else if (points[0] < minCol)
                    {
                        minCol = points[0];
                    }

                    if (points[1] > height)
                    {
                        height = points[1];
                    }
                }
                paths.Add(path);
            }

            height++;
            width++;

            int[,] map = new int[height,width];

            Dictionary<(int,int), int> dictionaryMap = new();

            // populate the map with air (-1)
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    map[row,col] = -1;
                    dictionaryMap.Add((row,col), -1);
                }
            }

            // Add two additional levels
            for (int col = 0; col < width; col++)
            {
                dictionaryMap.Add((height, col), -1);
                dictionaryMap.Add((height+1, col), 1);
            }

            // populate the map with rocks (1)
            foreach (var path in paths)
            {
                for (int i = 1; i < path.Paths.Count; i++)
                {
                    // Calculate all points between this point and the previous point
                    (int startCol, int startRow) = path.Paths[i-1];
                    (int endCol, int endRow) = path.Paths[i];
                    
                    for(int row = startRow; row <= endRow; row++)
                    {
                        for (int col = startCol; col <= endCol; col++)
                        {
                            map[row,col] = 1;
                            dictionaryMap[(row,col)] = 1;
                        }
                    }

                    for(int row = endRow; row <= startRow; row++)
                    {
                        for (int col = endCol; col <= startCol; col++)
                        {
                            map[row,col] = 1;
                            dictionaryMap[(row,col)] = 1;
                        }
                    }
                }
            }

            bool flowing = false;
            int time = 0;

            while (!flowing)
            {
                time++;
                flowing = DropSand(map, minCol, width, height);
            }

            // We actually want to know the time BEFORE flowing starts, so subtract 1
            Console.WriteLine($"Part one: {time-1}");

            bool stopped = false;
            time = 0;
            while (!stopped)
            {
                time++;
                (stopped, width, minCol) = DropSand(dictionaryMap, height+2, width, minCol);
            }

            Console.WriteLine($"Part two: {time}");
        }

        public static (bool stopped, int width, int minCol) DropSand(Dictionary<(int, int), int> map, int height, int width, int minCol)
        {
            (int row, int col) = (0,500);
            (int startRow, int startCol) = (0,500);

            int newWidth = width;
            int newMin = minCol;

            while (true)
            {
                // 0 is sand. 1 is rock. -1 is air.
                if (col == newMin)
                {
                    newMin--;
                    for (int i = 0; i < height; i++)
                    {
                        map.TryAdd((i,newMin), -1);
                    }
                    map[(height-1,newMin)] = 1;
                }
                else if (col == newWidth - 1)
                {
                    for (int i = 0; i < height; i++)
                    {
                        map.TryAdd((i,newWidth), -1);
                    }
                    map[(height-1,newWidth)] = 1;
                    newWidth++;
                }

                if (row + 1 == height)
                {
                    break;
                }
                else if (map[(row + 1, col)] < 0)
                {
                    row = row+1;
                }
                else if (map[(row + 1, col - 1)] < 0)
                {
                    row = row + 1;
                    col = col - 1;
                }
                else if (map[(row + 1, col + 1)] < 0)
                {
                    row = row + 1;
                    col = col + 1;
                }
                else
                {
                    map[(row,col)] = 0;
                    break;
                }
            }
            
            if (row == startRow && col == startCol)
            {
                return (true, newWidth, newMin);
            }
            else
            {
                return (false, newWidth, newMin);
            }
        }

        public static bool DropSand(int[,] map, int minCol, int width, int height)
        {
            // We drop sand from 0,500 until we hit a rock path or sand. If we hit rock, we check to the diagonal sides. Left, then right
            (int row, int col) = (0,500);
            bool flowing = false;
            while (true)
            {
                if (row+1 >= height || col >= width)
                {
                    flowing = true;
                    break;
                }
                
                if (map[row+1, col] < 0)
                {
                    row = row+1;
                }
                else if (col > minCol && map[row+1, col-1] < 0)
                {
                    row = row + 1;
                    col = col - 1;
                }
                else if (col < width && map[row+1, col+1] < 0)
                {
                    row = row + 1;
                    col = col + 1;
                }
                else if (map[row+1, col] >= 0 && map[row+1, col+1] >= 0 && map[row+1, col-1] >= 0)
                {
                    map[row,col] = 0;
                    break;
                }
                else
                {
                    flowing = true;
                    break;
                }
            }

            return flowing;
        }

        public static void DrawMap(int[,] map, int height, int width, int minCol, int minRow)
        {
            for (int row = minRow; row < height; row++)
            {
                for (int col = minCol; col < width; col++)
                {
                    if (row == 0 && col == 500)
                    {
                        Console.Write("+");
                    }
                    else if (map[row,col] == -1)
                    {
                        Console.Write(" ");
                    }
                    else if (map[row,col] == 1)
                    {
                        Console.Write("#");
                    }
                    else if (map[row,col] == 2)
                    {
                        Console.Write("+");
                    }
                    else if (map[row,col] == 0)
                    {
                        Console.Write("o");
                    }
                }
                Console.WriteLine();
            }
        }

        public static void DrawMap(Dictionary<(int, int), int> map, int height, int width, int minCol, int minRow)
        {
            for (int row = minRow; row < height; row++)
            {
                for (int col = minCol; col < width; col++)
                {
                    if (row == 0 && col == 500)
                    {
                        Console.Write("+");
                    }
                    else if (map[(row,col)] == -1)
                    {
                        Console.Write(" ");
                    }
                    else if (map[(row,col)] == 1)
                    {
                        Console.Write("#");
                    }
                    else if (map[(row,col)] == 2)
                    {
                        Console.Write("+");
                    }
                    else if (map[(row,col)] == 0)
                    {
                        Console.Write("o");
                    }
                }
                Console.WriteLine();
            }
        }
    }

    public class RockPath
    {
        public List<(int, int)> Paths;
        
        public RockPath()
        {
            Paths = new();
        }
    }
}
