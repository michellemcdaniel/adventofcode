using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode.Helpers;
using AdventOfCode.Models;

namespace AdventOfCode.TwentyTwo
{
    public class Day15
    {
        public static void Execute(string filename)
        {
            List<(int, int)> sensors = new();
            HashSet<(long, long)> beacons = new();

            DictionaryMap<char> map = new();
            Dictionary<(int row, int col), int> manhattanDistances = new();
            long minCol = long.MaxValue;
            long maxCol = long.MinValue;

            foreach (var line in File.ReadLines(filename))
            {
                RegexHelper.Match(
                    line, 
                    @"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)", 
                    out int sensorCol, 
                    out int sensorRow, 
                    out int beaconCol, 
                    out int beaconRow);
                
                map.TryAdd((sensorRow, sensorCol), 'S');
                sensors.Add((sensorRow, sensorCol));
                map.TryAdd((beaconRow, beaconCol), 'B');
                beacons.Add((beaconRow, beaconCol));

                minCol = Math.Min(Math.Min(sensorCol, minCol), beaconCol);
                maxCol = Math.Max(Math.Max(sensorCol, maxCol), beaconCol);

                int manhattanDistance = Math.Abs(sensorRow-beaconRow)+Math.Abs(sensorCol-beaconCol);
                manhattanDistances.Add((sensorRow, sensorCol), manhattanDistance);
            }

            long searchRow = 2000000;
            HashSet<(long, long)> beaconSet = new();

            // This is super slow, but tbh, I have no idea what to do about it.
            foreach(var distance in manhattanDistances)
            {
                for (long i = minCol-distance.Value; i < maxCol+distance.Value; i++)
                {
                    if (beaconSet.Contains((searchRow,i)))
                    {
                        continue;
                    }
                    else if (beacons.Contains((searchRow, i)))
                    {
                        continue;
                    }
                    if (Math.Abs(i - distance.Key.col)+Math.Abs(searchRow - distance.Key.row) <= distance.Value)
                    {
                        beaconSet.Add((searchRow,i));
                    }
                }
            }

            Console.WriteLine($"Part one: {beaconSet.Count()}");
            
            HashSet<(long row, long col)> possible = new();
            long max = 4000000;

            // Narrow down the search space a little, but it is still super slow
            foreach (var distance in manhattanDistances)
            { 
                long row = 0;
                long col = distance.Value;

                while (row <= distance.Value && col >= 0)
                {
                    CheckNeighbors(possible, distance, row, col);

                    col--;
                    row++;
                }

                row = 0;
                col = distance.Value;

                // now check the down half
                while (row >= -distance.Value && col >= 0)
                {
                    CheckNeighbors(possible, distance, row, col);

                    col--;
                    row--;
                }
            }

            foreach (var poss in possible)
            {
                if (poss.row < 0 || poss.col < 0 || poss.row > max || poss.col > max)
                {
                    continue;
                }
                bool found = true;
                foreach(var distance in manhattanDistances)
                {
                    if (CalculateManhattanDistance(distance.Key.row, distance.Key.col, poss.row, poss.col) <= distance.Value)
                    {
                        found = false;
                        break;
                    }
                }
                if (found)
                {
                    Console.WriteLine($"Part Two: {poss.row} {poss.col} = {poss.col*4000000 + poss.row}");
                    break;
                }
            }
        }

        public static long CalculateManhattanDistance(long sensorRow, long sensorCol, long beaconRow, long beaconCol)
        {
            return Math.Abs(sensorRow - beaconRow)+Math.Abs(sensorCol - beaconCol);
        }

        public static void CheckNeighbors(HashSet<(long row, long col)> possible, KeyValuePair<(int row, int col), int> distance, long row, long col)
        {
            if (col == 0)
            {
                // gotta check up and down
                if (CalculateManhattanDistance(distance.Key.row, distance.Key.col, distance.Key.row+row+1,distance.Key.col) > distance.Value)
                {
                    possible.Add((distance.Key.row+row+1, distance.Key.col));
                }
                if (CalculateManhattanDistance(distance.Key.row, distance.Key.col, distance.Key.row+row-1,distance.Key.col) > distance.Value)
                {
                    possible.Add((distance.Key.row+row-1, distance.Key.col));
                }
            }

            // check left and right
            if (CalculateManhattanDistance(distance.Key.row, distance.Key.col, distance.Key.row+row,distance.Key.col+col+1) > distance.Value)
            {
                possible.Add((distance.Key.row+row,distance.Key.col+col+1));
            }
            if (CalculateManhattanDistance(distance.Key.row, distance.Key.col, distance.Key.row+row,distance.Key.col+col-1) > distance.Value)
            {
                possible.Add((distance.Key.row+row,distance.Key.col+col-1));
            }

            if (CalculateManhattanDistance(distance.Key.row, distance.Key.col, distance.Key.row+row,distance.Key.col-col+1) > distance.Value)
            {
                possible.Add((distance.Key.row+row,distance.Key.col-col+1));
            }
            if (CalculateManhattanDistance(distance.Key.row, distance.Key.col, distance.Key.row+row,distance.Key.col-col-1) > distance.Value)
            {
                possible.Add((distance.Key.row+row,distance.Key.col-col-1));
            }
        }
    }
}
