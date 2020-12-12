using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day12
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();

            int direction = 0;
            List<char> directions = new List<char>(){'E','S','W','N'};

            int x = 0;
            int y = 0;

            int waypointX = 10;
            int waypointY = 1;

            // Part One
            foreach(var line in input)
            {
                char letter = line[0];
                int number = int.Parse(line[1..]);

                if (letter == 'F')
                {
                    (x,y) = MoveDirection(direction, x, y, number);
                }
                else if (letter == 'R' || letter == 'L')
                {
                    int adjusted = letter == 'R' ? number : 360-number;
                    direction = (direction+adjusted/90)%4;
                }
                else
                {
                    (x,y) = MoveDirection(directions.IndexOf(letter), x, y, number);
                }
            }

            Console.WriteLine($"Part One: {Math.Abs(x)+Math.Abs(y)}");

            x = 0;
            y = 0;

            // Part Two
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
                    for(int rotation = adjusted/90; rotation > 0; rotation--)
                    {
                        int swap = waypointX;
                        waypointX = waypointY;
                        waypointY = -swap;
                    }
                }
                else
                {
                    (waypointX, waypointY) = MoveDirection(directions.IndexOf(letter), waypointX, waypointY, number);
                }
            }

            Console.WriteLine($"Part Two: {Math.Abs(x)+Math.Abs(y)}");
        }

        public static (int, int) MoveDirection(int direction, int x, int y, int number)
        {
            if (direction == 1 || direction == 2)
            {
                number = -number;
            }

            int xChange = direction%2 == 0 ? number : 0;
            int yChange = direction%2 != 0 ? number : 0;

            return (x + xChange, y + yChange);
        }
    }
}