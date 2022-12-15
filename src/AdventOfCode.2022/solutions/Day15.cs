using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            HashSet<(long, long)> beacons = new();
            Dictionary<(int row, int col), int> manhattanDistances = new();
            int searchRow = 0;
            int maxSize = 0;

            foreach (var line in File.ReadLines(filename))
            {
                if (RegexHelper.Match(line, @"searchRow=\d+ maxSize=\d+"))
                {
                    RegexHelper.Match(line, @"searchRow=(\d+) maxSize=(\d+)", out searchRow, out maxSize);
                    continue;
                }

                RegexHelper.Match(
                    line, 
                    @"Sensor at x=(-?\d+), y=(-?\d+): closest beacon is at x=(-?\d+), y=(-?\d+)", 
                    out int sensorCol, 
                    out int sensorRow, 
                    out int beaconCol, 
                    out int beaconRow);

                beacons.Add((beaconRow, beaconCol));

                int manhattanDistance = Math.Abs(sensorRow-beaconRow)+Math.Abs(sensorCol-beaconCol);
                manhattanDistances.Add((sensorRow, sensorCol), manhattanDistance);
            }

            HashSet<(long, long)> beaconSet = new();

            Stopwatch timer = new Stopwatch();
            timer.Start();
            foreach(var distance in manhattanDistances)
            {
                // Calculate "inverse" manhattan distance i.e.
                // d = abs(row1-row2) + abs(col1-col2)
                // d - (abs(row1-row2)) = abs(col1-col2)
                // (+/-)range = col1-col2
                // So everything between distance.Key.col-range and distance.Key.col+range cannot have a beacon
                long range = distance.Value - Math.Abs(searchRow-distance.Key.row);
                for (long i = distance.Key.col-range; i <= distance.Key.col+range; i++)
                {
                    if (beacons.Contains((searchRow, i)))
                    {
                        continue;
                    }
                        
                    beaconSet.Add((searchRow,i));
                }
            }

            timer.Stop();
            Console.WriteLine($"Part one: {beaconSet.Count()} (time: {timer.ElapsedMilliseconds}ms)");

            (long row, long col)? distressSignal = null;
            timer.Restart();

            foreach (var distance in manhattanDistances)
            { 
                // We want to check the range of points just beyond the diamonds created by the manhattan distance
                // So we are basically searching a circle around the current signal point.
                long row = 0;
                long col = distance.Value;
                
                // Check the top half of the diamond
                while (distressSignal == null && row <= distance.Value && col >= 0)
                {
                    distressSignal = CheckNeighbors(manhattanDistances, distance, row++, col--, maxSize);
                }

                if (distressSignal != null)
                {
                    break;
                }
            }

            timer.Stop();
            Console.WriteLine($"Part Two: {distressSignal?.col*4000000 + distressSignal?.row} (time: {timer.ElapsedMilliseconds}ms)");
        }

        public static long CalculateManhattanDistance(long sensorRow, long sensorCol, long beaconRow, long beaconCol)
        {
            return Math.Abs(sensorRow - beaconRow)+Math.Abs(sensorCol - beaconCol);
        }

        public static bool PointInBounds((long row, long col) point, (int min, int max) bounds)
        {
            return point.row >= bounds.min && 
                point.col >= bounds.min && 
                point.row <= bounds.max && 
                point.col <= bounds.max;
        }

        public static (long row, long col)? CheckNeighbors(
            Dictionary<(int row, int col), int> signals, 
            KeyValuePair<(int row, int col), int> distance, 
            long row, 
            long col, 
            int maxSize)
        {
            HashSet<(long row, long col)> points = new();
            (int, int)[] searchSpace = new (int,int)[]{(1,1), (-1,-1), (1,-1), (-1,1)};
            
            // Add all the possible points that we want to check.
            // For the top and bottom of the diamond, we want to check up and down
            // For all points, we want to check the left and right side of the points in both the left quadrant and right quadrant
            if (col == 0)
            {
                foreach ((int i, int j) in searchSpace)
                {
                    points.Add((distance.Key.row+(j*row)+i, distance.Key.col));
                }
            }

            foreach((int i, int j) in searchSpace)
            {
                points.Add((distance.Key.row+(i*row),distance.Key.col+(i*col)+j));
                points.Add((distance.Key.row-(i*row),distance.Key.col+(i*col)+j));
            }

            foreach(var point in points)
            {
                // First, check that the point is both in bounds and that the manhattan distance for the point
                // is actually outside the current signal we are checking, since we're adding left and right
                // whether or not the point is on the left and right, since top and bottom require both to be checked
                if (PointInBounds(point,(0,maxSize)) &&
                    CalculateManhattanDistance(distance.Key.row, distance.Key.col, point.row, point.col) > distance.Value)
                {
                    // Now that we know the point is one we actually want to check, compare against all the signals
                    bool found = true;
                    foreach (var signal in signals)
                    {
                        if (CalculateManhattanDistance(signal.Key.row, signal.Key.col, point.row, point.col) <= signal.Value)
                        {
                            found = false;
                            break;
                        }
                    }
                    if (found)
                    {
                        // If the point is outside the manhattan distance for all signals, it's our answer. Quit early
                        return point;
                    }
                }
            }

            return null;
        }
    }
}
