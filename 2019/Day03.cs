using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Nineteen
{
    class Day03
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();

            string[] firstWire = input.ElementAt(0).Split(",");
            string[] secondWire = input.ElementAt(1).Split(",");

            Dictionary<int, List<Location>> firstWireDict = new Dictionary<int, List<Location>>();

            Point startPoint = new Point(0,0);
            long steps = 0;

            foreach(string direction in firstWire)
            {
                int count = Int32.Parse(direction[1..]);

                switch(direction[0])
                {
                    case 'U':
                        for (int i = 1+startPoint.Row; i < count+1+startPoint.Row; i++)
                        {
                            steps++;
                            if (firstWireDict.ContainsKey(i))
                            {
                                if (!firstWireDict[i].Any(l => l.Column == startPoint.Column))
                                {
                                    firstWireDict[i].Add(new Location(startPoint.Column, steps));
                                }
                            }
                            else
                            {
                                firstWireDict[i] = new List<Location>() { new Location(startPoint.Column, steps) };
                            }
                        }
                        startPoint = new Point(startPoint.Row + count, startPoint.Column);
                        break;
                    case 'D':
                        for (int i = startPoint.Row-1; i > startPoint.Row-count-1; i--)
                        {
                            steps++;
                            if (firstWireDict.ContainsKey(i))
                            {
                                if (!firstWireDict[i].Any(l => l.Column == startPoint.Column))
                                {
                                    firstWireDict[i].Add(new Location(startPoint.Column, steps));
                                }
                            }
                            else
                            {
                                firstWireDict[i] = new List<Location>() { new Location(startPoint.Column, steps) };
                            }
                        }
                        startPoint = new Point(startPoint.Row - count, startPoint.Column);
                        break;
                    case 'R':
                        for (int i = 1+startPoint.Column; i < count+startPoint.Column+1; i++)
                        {
                            steps++;
                            if (firstWireDict.ContainsKey(startPoint.Row))
                            {
                                if (!firstWireDict[startPoint.Row].Any(l => l.Column == i))
                                {
                                    firstWireDict[startPoint.Row].Add(new Location(i, steps));
                                }
                            }
                            else
                            {
                                firstWireDict[startPoint.Row] = new List<Location>() { new Location(i, steps) };
                            }
                        }
                        startPoint = new Point(startPoint.Row, startPoint.Column+count);
                        break;
                    case 'L':
                        for (int i = startPoint.Column-1; i > startPoint.Column-count-1; i--)
                        {
                            steps++;
                            if (firstWireDict.ContainsKey(startPoint.Row))
                            {
                                if (!firstWireDict[startPoint.Row].Any(l => l.Column == i))
                                {
                                    firstWireDict[startPoint.Row].Add(new Location(i, steps));
                                }
                            }
                            else
                            {
                                firstWireDict[startPoint.Row] = new List<Location>() { new Location(i, steps) };
                            }
                        }
                        startPoint = new Point(startPoint.Row, startPoint.Column-count);
                        break;
                    default:
                        break;
                }
            }

            int minDistance = Int32.MaxValue;
            long minSteps = long.MaxValue;
            startPoint = new Point(0,0);

            steps = 0;

            foreach(string direction in secondWire)
            {
                int count = Int32.Parse(direction[1..]);

                switch(direction[0])
                {
                    case 'U':
                        for (int i = 1+startPoint.Row; i < count+1+startPoint.Row; i++)
                        {
                            steps++;
                            if (firstWireDict.ContainsKey(i) && firstWireDict[i].Any(l => l.Column == startPoint.Column))
                            {
                                minDistance = Math.Min(minDistance, Math.Abs(i)+Math.Abs(startPoint.Column));
                                minSteps = Math.Min(minSteps, steps+firstWireDict[i].First(l => l.Column == startPoint.Column).Steps);
                            }
                        }
                        startPoint = new Point(startPoint.Row + count, startPoint.Column);
                        break;
                    case 'D':
                        for (int i = startPoint.Row-1; i > startPoint.Row-count-1; i--)
                        {
                            steps++;
                            if (firstWireDict.ContainsKey(i) && firstWireDict[i].Any(l => l.Column == startPoint.Column))
                            {
                                minDistance = Math.Min(minDistance, Math.Abs(i)+Math.Abs(startPoint.Column));
                                minSteps = Math.Min(minSteps, steps+firstWireDict[i].First(l => l.Column == startPoint.Column).Steps);
                            }
                        }
                        startPoint = new Point(startPoint.Row - count, startPoint.Column);
                        break;
                    case 'R':
                        for (int i = 1+startPoint.Column; i < count+startPoint.Column+1; i++)
                        {
                            steps++;
                            if (firstWireDict.ContainsKey(startPoint.Row) && firstWireDict[startPoint.Row].Any(l => l.Column == i))
                            {
                                minDistance = Math.Min(minDistance, Math.Abs(startPoint.Row)+Math.Abs(i));
                                minSteps = Math.Min(minSteps, steps+firstWireDict[startPoint.Row].First(l => l.Column == i).Steps);
                            }
                        }
                        startPoint = new Point(startPoint.Row, startPoint.Column+count);
                        break;
                    case 'L':
                        for (int i = startPoint.Column-1; i > startPoint.Column-count-1; i--)
                        {
                            steps++;
                            if (firstWireDict.ContainsKey(startPoint.Row) && firstWireDict[startPoint.Row].Any(l => l.Column == i))
                            {
                                minDistance = Math.Min(minDistance, Math.Abs(startPoint.Row)+Math.Abs(i));
                                minSteps = Math.Min(minSteps, steps+firstWireDict[startPoint.Row].First(l => l.Column == i).Steps);
                            }
                        }
                        startPoint = new Point(startPoint.Row, startPoint.Column-count);
                        break;
                    default:
                        break;
                }
            }

            Console.WriteLine($"Min Distance: {minDistance}");
            Console.WriteLine($"Min Steps: {minSteps}");
        }
    }

    public class Point
    {
        public int Row { get; }
        public int Column { get; }

        public Point(int row, int col)
        {
            Row = row;
            Column = col;
        }
    }

    public class Location
    {
        public int Column { get; }
        public long Steps { get; }

        public Location(int col, long steps)
        {
            Column = col;
            Steps = steps;
        }
    }
}
