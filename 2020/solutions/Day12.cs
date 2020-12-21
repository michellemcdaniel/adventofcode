using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day12
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();

            List<(bool, int, int)> moveWayPoints = new List<(bool, int, int)>() { (false, 1, 0), (true, 10, 1) };

            // Part One
            foreach (var waypoint in moveWayPoints)
            {
                (bool moveWayPoint, int waypointX, int waypointY) = waypoint;
                int x = 0;
                int y = 0;

                foreach(var line in input)
                {
                    char letter = line[0];
                    int number = int.Parse(line[1..]);

                    if (letter == 'F')
                    {
                        x += number*waypointX;
                        y += number*waypointY;
                    }
                    else if (letter == 'R' || letter == 'L')
                    {
                        int adjusted = letter == 'R' ? number : 360-number;
                        for(int rotation = adjusted; rotation > 0; rotation-=90)
                        {
                            int swap = waypointX;
                            waypointX = waypointY;
                            waypointY = -swap;
                        }
                    }
                    else if (moveWayPoint)
                    {
                        (waypointX, waypointY) = MoveDirection(letter, waypointX, waypointY, number);
                    }
                    else
                    {
                        (x, y) = MoveDirection(letter, x, y, number);
                    }
                }

                Console.WriteLine($"Distance moved: {Math.Abs(x)+Math.Abs(y)}");
            }
        }

        public static (int, int) MoveDirection(char direction, int x, int y, int number)
        {
            if (direction == 'W' || direction == 'S')
            {
                number = -number;
            }

            int xChange = direction == 'E' || direction == 'W' ? number : 0;
            int yChange = direction == 'S' || direction == 'N' ? number : 0;

            return (x + xChange, y + yChange);
        }
    }
}