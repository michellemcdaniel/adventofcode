using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day11
    {
        public enum Direction
        {
            Up,
            Left,
            Down,
            Right
        }

        public static void Execute()
        {
            string input = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "input", "day11.txt"));

            Dictionary<(int, int), long> panels = Paint(0,input,out int count);

            Console.WriteLine($"Part One: {count}");

            panels = Paint(1, input, out _);
            Draw(panels);
        }

        public static void Draw(Dictionary<(int, int), long> panels)
        {
            int minRow = panels.Keys.Select(k => k.Item1).Min();
            int maxRow = panels.Keys.Select(k => k.Item1).Max();
            int minCol = panels.Keys.Select(k => k.Item2).Min();
            int maxCol = panels.Keys.Select(k => k.Item2).Max();

            Console.WriteLine($"{minRow}:{maxRow},{minCol}:{maxCol}");

            for(int i = minRow; i <= maxRow; i++)
            {
                for(int j = minCol; j <= maxCol; j++)
                {
                    if(panels.TryGetValue((i,j), out long color))
                    {
                        if (color == 1)
                        {
                            Console.Write("#");
                        }
                        else
                        {
                            Console.Write(" ");
                        }
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.WriteLine();
            }
        }

        public static Dictionary<(int, int), long> Paint(long startColor, string input, out int count)
        {
            IntCode intCode = new IntCode(input.Split(",").ToList().Select(o => long.Parse(o)).ToArray(), true);

            bool paint = true;
            count = 1;

            Dictionary<(int, int), long> panels = new Dictionary<(int, int), long>();
            (int x, int y) = (0,0);
            panels.Add((x,y),startColor);

            Direction direction = Direction.Up;
            
            while (!intCode.Halted)
            {
                long output = 0;
                while(!intCode.Paused && !intCode.Halted)
                {
                    output = intCode.Compute(panels[(x,y)]);
                }
                if (paint)
                {
                    panels[(x,y)] = output;
                    paint = false;
                }
                else
                {
                    if (output == 0)
                    {
                        if (direction == Direction.Right)
                        {
                            direction = Direction.Up;
                        }
                        else
                        {
                            direction = (Direction) ((int)direction+1);
                        }
                    }
                    else if (output == 1)
                    {
                        if (direction == Direction.Up)
                        {
                            direction = Direction.Right;
                        }
                        else
                        {
                            direction = (Direction) ((int)direction-1);
                        }
                    }
                    (int dx, int dy) = GetDirection(direction);
                    x += dx;
                    y += dy;
                    if (!panels.ContainsKey((x,y)))
                    {
                        count++;
                        panels.Add((x,y),0);
                    }
                    paint = true;
                }

                intCode.Paused = false;
            }

            return panels;
        }

        public static (int dx, int dy) GetDirection(Direction direction)
        {
            switch(direction)
            {
                case Direction.Up:
                    return (-1,0);
                case Direction.Left:
                    return (0, -1);
                case Direction.Down:
                    return (1,0);
                case Direction.Right:
                    return (0,1);
                default:
                    throw new ArgumentException($"Unknown direction {direction}", "direction");
            }
        }
    }
}
