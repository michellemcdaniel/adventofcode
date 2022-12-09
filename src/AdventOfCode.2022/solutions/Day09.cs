using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.TwentyTwo
{
    public class Day09
    {
        public static void Execute(string filename)
        {
            (int x, int y)[] twoKnots = new (int,int)[2];
            HashSet<(int, int)> pointsTwoKnots = new();

            (int x, int y)[] tenKnots = new (int,int)[10];
            HashSet<(int, int)> pointsTenKnots = new();

            foreach (var line in File.ReadLines(filename))
            {
                RegexHelper.Match(line, @"([UDRL]) (\d+)", out string direction, out int count);

                MoveAllPoints(twoKnots, direction, count, pointsTwoKnots);
                MoveAllPoints(tenKnots, direction, count, pointsTenKnots);
            }

            Console.WriteLine($"Part one: {pointsTwoKnots.Count()}");
            Console.WriteLine($"Part two: {pointsTenKnots.Count()}");
        }

        public static void MoveAllPoints((int x, int y)[] knots, string direction, int count, HashSet<(int, int)> points)
        {
            for(int i = 0; i < count; i++)
            {
                switch(direction) {
                    case "U":
                        knots[0].y++;
                        break;
                    case "D":
                        knots[0].y--;
                        break;
                    case "R":
                         knots[0].x++;
                         break;
                    case "L":
                        knots[0].x--;
                        break;
                    default:
                        break;
                }

                for(int knot = 0; knot < knots.Length-1; knot++)
                {
                    // If we are within 1 of both axes, we do nothing. Ie. We don't need to move if we are in any of these spots:
                    // 1 2 3
                    // 4 X 6
                    // 7 8 9
                    // We also will never move in the same direction more than once in one step
                    // i.e. if the previous knot is up 2 and right 2, we will move diagonally up 1 and right 1 towards it, rather
                    // than moving up 2 or right 2.
                    if (!(Math.Abs(knots[knot].x-knots[knot+1].x) <= 1 && Math.Abs(knots[knot].y-knots[knot+1].y) <= 1))
                    {
                        if (knots[knot].y > knots[knot+1].y)
                        {
                            // move up
                            knots[knot+1].y++;
                        }
                        else if (knots[knot].y < knots[knot+1].y)
                        {
                            // move down
                            knots[knot+1].y--;
                        }

                        if (knots[knot].x > knots[knot+1].x)
                        {
                            // move right
                            knots[knot+1].x++;
                        }
                        else if (knots[knot].x < knots[knot+1].x)
                        {
                            // move left
                            knots[knot+1].x--;
                        }
                    }
                }

                points.Add((knots.Last().x, knots.Last().y));
            }
        }
    }
}
