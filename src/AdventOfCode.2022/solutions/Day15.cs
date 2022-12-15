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
            long minRow = long.MaxValue;
            long maxRow = long.MinValue;
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

                minRow = Math.Min(Math.Min(sensorRow, minRow), beaconRow);
                maxRow = Math.Max(Math.Max(sensorRow, maxRow), beaconRow);

                minCol = Math.Min(Math.Min(sensorCol, minCol), beaconCol);
                maxCol = Math.Max(Math.Max(sensorCol, maxCol), beaconCol);

                int manhattanDistance = Math.Abs(sensorRow-beaconRow)+Math.Abs(sensorCol-beaconCol);
                manhattanDistances.Add((sensorRow, sensorCol), manhattanDistance);
            }

            long searchRow = 2000000;
            //long row = 10;

            Dictionary<(long, long), int> noBeacons = new();
            HashSet<(long, long)> beaconSet = new();

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
            HashSet<(long row, long col)> possibleLocations = new();

            //long max = 20;
            long max = 4000000;
            for (long row = 0; row < max; row++)
            {
                for(long col = 0; col < max; col++)
                {
                    possibleLocations.Add((row,col));
                    foreach (var distance in manhattanDistances)
                    {
                        if (CalculateManhattanDistance(distance.Key.row, distance.Key.col, row, col) <= distance.Value)
                        {
                            possibleLocations.Remove((row,col));
                            break;
                        }
                    }
                    
                }
            }

            Console.WriteLine(possibleLocations.First());
            Console.WriteLine($"Part two: {possibleLocations.First().col*4000000 + possibleLocations.First().row}");
        }

        public static long CalculateManhattanDistance(long sensorRow, long sensorCol, long beaconRow, long beaconCol)
        {
            return Math.Abs(sensorRow - beaconRow)+Math.Abs(sensorCol - beaconCol);
        }
    }
}
