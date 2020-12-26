using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Nineteen
{
    class Day10
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();

            int height = input.Count();
            int width = input[0].Length;

            int[,] replacementMap = new int[height,width];

            Dictionary<(int, int), List<string>> found = new Dictionary<(int, int), List<string>>();

            // Iterate over the the entire input
            for (int i = 0; i < input.Count(); i++)
            {
                for (int j = 0; j < input[i].Length; j++)
                {
                    if (!found.ContainsKey((i,j)))
                    {
                        found[(i,j)] = new List<string>();
                    }
                    if (input[i][j] != '#')
                    {
                        continue;
                    }
                    for (int x = 0; x < input.Count(); x++)
                    {
                        for (int y = 0; y < input[x].Count(); y++)
                        {
                            if (i == x && j == y)
                            {
                                continue;
                            }

                            if (input[x][y] != '#')
                            {
                                continue;
                            }
                            string slope = ((x-i)*1.0/(y-j)).ToString();

                            if (x-i < 0)
                            {
                                slope = $"{slope}x";
                            }
                            if (y-j < 0)
                            {
                                slope = $"{slope}y";
                            }
                            
                            if (found[(i,j)].Contains(slope))
                            {
                                continue;
                            }

                            if (input[x][y] == '#')
                            {
                                found[(i,j)].Add(slope);
                            }
                        }
                    }
                }
            }

            int max = 0;
            int maxRow = 0;
            int maxColumn = 0;
            foreach(var kvp in found)
            {
                if (kvp.Value.Count() > max)
                {
                    max = kvp.Value.Count();
                    (maxRow, maxColumn) = kvp.Key;
                }
            }
            
            Console.WriteLine($"({maxRow},{maxColumn}) = {max}");

            List<Slope> firstQuadrant = new List<Slope>();
            List<Slope> secondQuadrant = new List<Slope>();
            List<Slope> thirdQuadrant = new List<Slope>();
            List<Slope> fourthQuadrant = new List<Slope>();

            for (int row = maxRow; row >= 0; row--)
            {
                Console.WriteLine(row);
                for (int column = maxColumn; column < input[row].Length; column++)
                {
                    Console.WriteLine(column);
                    Console.WriteLine($"{row} {column}");
                    if (maxRow == row && maxColumn == column)
                    {
                        Console.WriteLine("\tcontinuing");
                        continue;
                    }

                    double slope = (column-maxColumn)*1.0/(row-maxRow);
                    Console.WriteLine($"\tslope: {slope}");

                    if (!firstQuadrant.Any(s => s.slope == slope))
                    {
                        firstQuadrant.Add(new Slope(){ row = row-maxRow, column = column-maxColumn, slope = slope});
                        Console.WriteLine("\tadding");
                    }
                    else
                    {
                        Console.WriteLine("\tnot adding");
                    }
                }
            }

            Console.WriteLine("created first quadrant");

            for (int row = maxRow+1; row < input.Count(); row++)
            {
                for (int column = maxColumn; column < input[row].Length; column++)
                {
                    if (maxRow == row && maxColumn == column)
                    {
                        continue;
                    }

                    double slope = (column-maxColumn)*1.0/(row-maxRow);

                    if (!secondQuadrant.Any(s => s.slope == slope))
                        secondQuadrant.Add(new Slope(){ row = row-maxRow, column = column-maxColumn, slope = slope});
                }
            }

            Console.WriteLine("created second quadrant");

            for (int row = maxRow; row < input.Count(); row++)
            {
                for (int column = maxColumn-1; column >= 0; column--)
                {
                    if (maxRow == row && maxColumn == column)
                    {
                        continue;
                    }

                    double slope = (column-maxColumn)*1.0/(row-maxRow);

                    if (!thirdQuadrant.Any(s => s.slope == slope))
                        thirdQuadrant.Add(new Slope(){ row = row-maxRow, column = column-maxColumn, slope = slope});
                }
            }

            Console.WriteLine("created third quadrant");

            for (int row = maxRow+1; row >= 0; row--)
            {
                for (int column = maxColumn-1; column >= 0; column--)
                {
                    if (maxRow == row && maxColumn == column)
                    {
                        continue;
                    }

                    double slope = (column-maxColumn)*1.0/(row-maxRow);

                    if (!fourthQuadrant.Any(s => s.slope == slope))
                        fourthQuadrant.Add(new Slope(){ row = row-maxRow, column = column-maxColumn, slope = slope});
                }
            }

            Console.WriteLine("created fourth quadrant");

            firstQuadrant.Sort( (a, b) => -1*a.slope.CompareTo(b.slope));
            secondQuadrant.Sort( (a, b) => -1*a.slope.CompareTo(b.slope));
            thirdQuadrant.Sort( (a, b) => -1*a.slope.CompareTo(b.slope));
            fourthQuadrant.Sort( (a, b) => -1*a.slope.CompareTo(b.slope));
            
            if (firstQuadrant.Any(s => double.IsInfinity(s.slope)))
            {
                Slope infinite = firstQuadrant.First(s => double.IsInfinity(s.slope));
                firstQuadrant.Remove(infinite);
                firstQuadrant.Add(infinite);
            }

            if (secondQuadrant.Any(s => double.IsInfinity(s.slope)))
            {
                Slope infinite = secondQuadrant.First(s => double.IsInfinity(s.slope));
                secondQuadrant.Remove(infinite);
                secondQuadrant.Add(infinite);
            }

            if (thirdQuadrant.Any(s => double.IsInfinity(s.slope)))
            {
                Slope infinite = thirdQuadrant.First(s => double.IsInfinity(s.slope));
                thirdQuadrant.Remove(infinite);
                thirdQuadrant.Add(infinite);
            }

            if (fourthQuadrant.Any(s => double.IsInfinity(s.slope)))
            {
                Slope infinite = fourthQuadrant.First(s => double.IsInfinity(s.slope));
                fourthQuadrant.Remove(infinite);
                fourthQuadrant.Add(infinite);
            }

            int count = 0;

            foreach (string s in input)
            {
                Console.WriteLine(s);
            }

            Console.WriteLine();

            bool replace = true;

            int replaced200Row = 0;
            int replaced200Column = 0;

            while (replace)
            {
                replace = false;

                foreach (Slope slope in firstQuadrant)
                {
                    bool newFound = false;
                    int multiplier = 1;

                    while (!newFound)
                    {
                        int rowLocation = maxRow+multiplier*slope.row;
                        int columnLocation = maxColumn+multiplier*slope.column;
                        if (rowLocation < 0 || rowLocation >= input.Count() || columnLocation < 0 || columnLocation >= input[0].Length)
                        {
                            break;
                        }

                        if (input[rowLocation][columnLocation] == '#')
                        {
                            char[] inputRowAsCharArry = input[rowLocation].ToCharArray();
                            inputRowAsCharArry[columnLocation] = '.';

                            input[rowLocation] = new string(inputRowAsCharArry);
                            count++;
                            replace = true;
                            newFound = true;

                            replacementMap[rowLocation,columnLocation] = count;

                            if (count == 200)
                            {
                                replaced200Row = rowLocation;
                                replaced200Column = columnLocation;
                            }
                        }

                        multiplier++;
                    }
                }

                foreach (string s in input)
                {
                    Console.WriteLine(s);
                }

                Console.WriteLine();

                foreach (Slope slope in secondQuadrant)
                {

                    bool newFound = false;
                    int multiplier = 1;

                    while (!newFound)
                    {
                        int rowLocation = maxRow+multiplier*slope.row;
                        int columnLocation = maxColumn+multiplier*slope.column;
                        if (rowLocation < 0 || rowLocation >= input.Count() || columnLocation < 0 || columnLocation >= input[0].Length)
                        {
                            break;
                        }

                        if (input[rowLocation][columnLocation] == '#')
                        {
                            char[] inputRowAsCharArry = input[rowLocation].ToCharArray();
                            inputRowAsCharArry[columnLocation] = '.';

                            Console.WriteLine($"Replacing ({rowLocation},{columnLocation})");

                            input[rowLocation] = new string(inputRowAsCharArry);
                            count++;
                            replace = true;
                            newFound = true;

                            replacementMap[rowLocation,columnLocation] = count;

                            if (count == 200)
                            {
                                replaced200Row = rowLocation;
                                replaced200Column = columnLocation;
                            }
                        }

                        multiplier++;
                    }
                }

                foreach (string s in input)
                {
                    Console.WriteLine(s);
                }

                Console.WriteLine();

                foreach (Slope slope in thirdQuadrant)
                {

                    bool newFound = false;
                    int multiplier = 1;

                    while (!newFound)
                    {
                        int rowLocation = maxRow+multiplier*slope.row;
                        int columnLocation = maxColumn+multiplier*slope.column;
                        if (rowLocation < 0 || rowLocation >= input.Count() || columnLocation < 0 || columnLocation >= input[0].Length)
                        {
                            break;
                        }

                        if (input[rowLocation][columnLocation] == '#')
                        {
                            char[] inputRowAsCharArry = input[rowLocation].ToCharArray();
                            inputRowAsCharArry[columnLocation] = '.';

                            input[rowLocation] = new string(inputRowAsCharArry);
                            count++;
                            replace = true;
                            newFound = true;

                            replacementMap[rowLocation,columnLocation] = count;

                            if (count == 200)
                            {
                                replaced200Row = rowLocation;
                                replaced200Column = columnLocation;
                            }
                        }

                        multiplier++;
                    }
                }

                foreach (string s in input)
                {
                    Console.WriteLine(s);
                }

                Console.WriteLine();

                foreach (Slope slope in fourthQuadrant)
                {
                    bool newFound = false;
                    int multiplier = 1;

                    while (!newFound)
                    {
                        int rowLocation = maxRow+multiplier*slope.row;
                        int columnLocation = maxColumn+multiplier*slope.column;

                        if (rowLocation < 0 || rowLocation >= input.Count() || columnLocation < 0 || columnLocation >= input[0].Length)
                        {
                            break;
                        }

                        if (input[rowLocation][columnLocation] == '#')
                        {
                            char[] inputRowAsCharArry = input[rowLocation].ToCharArray();
                            inputRowAsCharArry[columnLocation] = '.';

                            input[rowLocation] = new string(inputRowAsCharArry);
                            count++;
                            replace = true;
                            newFound = true;

                            replacementMap[rowLocation,columnLocation] = count;

                            if (count == 200)
                            {
                                replaced200Row = rowLocation;
                                replaced200Column = columnLocation;
                            }
                        }

                        multiplier++;
                    }
                }

                foreach (string s in input)
                {
                    Console.WriteLine(s);
                }
            }

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (i == maxRow && j == maxColumn)
                    {
                        Console.Write("X ");
                    }
                    else if (replacementMap[i,j] == 0)
                    {
                        Console.Write(". ");
                    }
                    else
                        Console.Write($"{replacementMap[i,j]} ");
                }
                Console.WriteLine();
            }

            Console.WriteLine($"Count: {count}");
            Console.WriteLine($"{replaced200Row+replaced200Column*100}");

        }

        public static void ProcessQuadrant(List<Slope> quadrant, int maxRow, int maxColumn, List<string> input)
        {
            
        }
    }

    public class Slope
    {
        public int row;
        public int column;
        public double slope;
    }
}
