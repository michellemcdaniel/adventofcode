using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Nineteen
{
    public class Day15
    {
        public class Location
        {
            public Status Status { get; set; }
            public int Neighbors { get; set; }
        }

        public enum Status
        {
            Wall,
            Empty,
            Oxygen
        }

        public static Stack<Stack<int>> backtrackTraces = new();
        public static Stack<int> pathToOxygen = new();

        public static void Execute(string filename)
        {
            string input = File.ReadAllText(filename);

            IntCode intCode = new IntCode(input.Split(",").ToList().Select(o => long.Parse(o)).ToArray(), true, true);
            Dictionary<(int, int), Location> floorplan = new();
            floorplan.Add((0,0), new Location() { Status = Status.Empty, Neighbors = 0 });

            (int x, int y) = CreateMap(intCode, floorplan);
            int time = Oxygenate(floorplan, x, y);

            Console.WriteLine($"Part Two: {time}");
        }

        public static List<(int, int)> GetNeighbors(int x, int y)
        {
            List<(int,int)> locations = new();
            for(int i = 1; i <= 4; i++)
            {
                (int dx, int dy) = GetDirection(i);
                locations.Add((x+dx, y+dy));
            }
            return locations;
        }

        public static int Oxygenate(Dictionary<(int, int), Location> floorplan, int startX, int startY)
        {
            int iterations = 0;
            Queue<(int,int)> currentlyOxygenated = new();
            currentlyOxygenated.Enqueue((startX,startY));

            while(floorplan.Any(x => x.Value.Status == Status.Empty))
            {
                Queue<(int,int)> newlyOxygenated = new();

                while (currentlyOxygenated.Any())
                {
                    (int x, int y) = currentlyOxygenated.Dequeue();

                    foreach(var loc in GetNeighbors(x,y))
                    {
                        if (floorplan.TryGetValue((loc.Item1, loc.Item2), out Location l))
                        {
                            if (l.Status == Status.Empty)
                            {
                                l.Status = Status.Oxygen;
                                newlyOxygenated.Enqueue((loc.Item1, loc.Item2));
                            }
                        }
                    }
                }
                currentlyOxygenated = new Queue<(int, int)>(newlyOxygenated);
                iterations++;
            }
            DrawMap(floorplan,0,0);
            return iterations;
        }

        public static (int,int) CreateMap(IntCode intCode, Dictionary<(int, int), Location> floorplan)
        {
            int x = 0;
            int y = 0;
            int oxyX = 0;
            int oxyY = 0;
            backtrackTraces.Push(new Stack<int>());
            while(true)
            {
                Stack<int> nextDirections = CheckNeighbors(x,y,intCode, floorplan);

                int nextDirection = 0;

                if (nextDirections.Count > 1)
                {
                    backtrackTraces.Push(new Stack<int>());
                }

                if (nextDirections.Any())
                {
                    nextDirection = nextDirections.Pop();
                    backtrackTraces.Peek().Push(nextDirection);
                    pathToOxygen.Push(nextDirection);

                    intCode.Resume(nextDirection);
                    long current = intCode.Compute();
                    
                    (int dx, int dy) = GetDirection(nextDirection);
                    x+=dx;
                    y+=dy;

                    if (current == 2)
                    {
                        oxyX = x;
                        oxyY = y;
                        Console.WriteLine($"Part One: {pathToOxygen.Count()}");
                    }
                }
                else
                {
                    // Backtrack to last point that wasn't fully explored.
                    if (!backtrackTraces.Any())
                    {
                        break; // if there are no more backtraces to track, we are back at our start location and have completed the map.
                    }
                    (x,y) = BackTrack(intCode, backtrackTraces.Pop(), x, y);
                }
            }

            DrawMap(floorplan, x, y);
            return (oxyX, oxyY);
        }

        public static (int x, int y) BackTrack(IntCode intCode, Stack<int> directions, int x, int y)
        {
            while(directions.Any())
            {
                int direction = SwitchDirection(directions.Pop());
                pathToOxygen.Pop();
                intCode.Resume(direction);
                intCode.Compute();

                (int dx, int dy) = GetDirection(direction);
                x += dx;
                y += dy;
            }
            return (x,y);
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

        public static int SwitchDirection(int i)
        {
            switch(i)
            {
                case 1:
                    return 2;
                case 2:
                    return 1;
                case 3:
                    return 4;
                case 4:
                    return 3;
                default:
                    // something went wrong
                    throw new ArgumentException($"Unkown direction {i}", "i");
            }
        }

        public static Stack<int> CheckNeighbors(int x, int y, IntCode intCode, Dictionary<(int, int), Location> floorplan)
        {
            Stack<int> goodNeighbors = new();
            
            for (int i = 1; i <= 4; i++)
            {
                (int dx, int dy) = GetDirection(i);

                if(floorplan.TryGetValue((x+dx, y+dy), out Location location))
                {
                    location.Neighbors++;
                    if (location.Neighbors < 4 && location.Status == Status.Empty)
                    {
                        goodNeighbors.Push(i);
                    }
                    continue; // we have checked that neighbor already
                }
                
                intCode.Resume(i);
                long output = intCode.Compute(i);

                switch(output)
                {
                    case 0:
                        floorplan[(x+dx,y+dy)] = new Location() { Status = Status.Wall, Neighbors = 1 };
                        break;
                    case 1: 
                        floorplan[(x+dx,y+dy)] = new Location() { Status = Status.Empty, Neighbors = 1 };
                        goodNeighbors.Push(i); // go one of these directions next
                        intCode.Resume(SwitchDirection(i));
                        intCode.Compute(); // backtrack to where we were.
                        break;
                    case 2:
                        floorplan[(x+dx,y+dy)] = new Location() { Status = Status.Oxygen, Neighbors = 1 };
                        goodNeighbors.Push(i); // go one of these directions next
                        intCode.Resume(SwitchDirection(i));
                        intCode.Compute(); // backtrack to where we were.
                        break;
                    default:
                        throw new ArgumentException($"Unknown status {output}", "output");
                }
            }

            floorplan[(x,y)].Neighbors = 4;
            return goodNeighbors;
        }

        public static void DrawMap(Dictionary<(int, int), Location> floorplan, int x, int y)
        {
            int minRow = floorplan.Keys.Select(key => key.Item1).Min();
            int maxRow = floorplan.Keys.Select(key => key.Item1).Max();
            int minCol = floorplan.Keys.Select(key => key.Item2).Min();
            int maxCol = floorplan.Keys.Select(key => key.Item2).Max();

            for (int i = minRow; i <= maxRow; i++)
            {
                for (int j = minCol; j <= maxCol; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        Console.Write("X");
                    }
                    else if (i == x && j == y)
                    {
                        Console.Write("D");
                    }
                    else if(floorplan.TryGetValue((i,j), out Location status))
                    {
                        switch(status.Status)
                        {
                            case Status.Empty:
                                Console.Write(".");
                                break;
                            case Status.Wall:
                                Console.Write("#");
                                break;
                            case Status.Oxygen:
                                Console.Write("O");
                                break;
                        }
                    }
                    else
                        Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
