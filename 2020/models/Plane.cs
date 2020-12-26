using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Twenty
{
    public class Plane
    {
        public List<string> Rows { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Plane(List<string> rows)
        {
            Rows = rows;
            Height = rows.Count();
            Width = rows.First().Length;
        }

        public Plane(int width, int height)
        {
            Rows = new List<string>();
            for(int i = 0; i < height; i++)
            {
                Rows.Add(new string('.', width));
            }

            Height = height;
            Width = width;
        }

        public void Expand()
        {
            List<string> expanded = new List<string>();
            expanded.Add(new string('.', Width+2));
            foreach(var row in Rows)
            {
                expanded.Add($".{row}.");
            }
            expanded.Add(new string('.', Width+2));
            Width+=2;
            Height+=2;
            Rows = expanded;
        }

        public bool Exists(int x, int y)
        {
            return (x >= 0 && y >= 0 && x < Width && y < Height);
        }

        public bool IsOccupied(int x, int y)
        {
            return Exists(x,y) && Rows[y][x] == '#';
        }
        public bool IsUnOccupied(int x, int y)
        {
            return Exists(x,y) && Rows[y][x] == '.';
        }

        public int ActiveAround(int x, int y, int z, int w)
        {
            return ActiveAround(x, y, z, w, 1);
        }

        public int ActiveAround(int x, int y, int z, int w, int multipler)
        {
            int totalActive = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    int currentMultiplier = 1;
                    if (i == 0 && j == 0 && z == 0 && w == 0)
                    {
                        continue;
                    }

                    while (currentMultiplier <= multipler)
                    {
                        if (IsOccupied(x+i*currentMultiplier, y+j*currentMultiplier))
                        {
                            totalActive++;
                            break;
                        }
                        else if (IsUnOccupied(x+i*currentMultiplier, y+j*currentMultiplier))
                        {
                            break;
                        }

                        currentMultiplier++;
                    }
                }
            }
            
            return totalActive;
        }

        public int CountOccupied()
        {
            int totalActive = 0;
            foreach(var line in Rows)
            {
                totalActive += line.Where(x => x == '#').Count();
            }
            return totalActive;
        }

        public Plane Create(Func<bool, int, char> CheckRules, Cube cube, int z)
        {
            List<string> newPlane = new List<string>();
            for (int y = 0; y < Height; y++)
            {
                string newLine = "";
                for (int x = 0; x < Width; x++)
                {
                    newLine += CheckRules(IsOccupied(x,y), cube.ActiveAround(z, x, y));
                }
                newPlane.Add(newLine);
            }

            return new Plane(newPlane);
        }

        public Plane Create(Func<bool, int, char> CheckRules, HyperCube hyperCube, int w, int z)
        {
            List<string> newPlane = new List<string>();
            for (int y = 0; y < Height; y++)
            {
                string newLine = "";
                for (int x = 0; x < Width; x++)
                {
                    newLine += CheckRules(IsOccupied(x,y), hyperCube.ActiveAround(w, z, x, y));
                }
                newPlane.Add(newLine);
            }

            return new Plane(newPlane);
        }

        public bool Generate(Func<bool,  int, char> CheckRules, int multiplier)
        {
            bool changed = false;
            List<string> newPlane = new List<string>();

            for (int y = 0; y < Height; y++)
            {
                string newLine = "";
                for (int x = 0; x < Width; x++)
                {
                    if (Rows[y][x] == ' ')
                    {
                        newLine += ' ';
                        continue;
                    }
                    
                    newLine += CheckRules(IsOccupied(x,y), ActiveAround(x,y,0,0,multiplier));
                }
                if (newLine != Rows[y])
                {
                    changed = true;
                }
                newPlane.Add(newLine);
            }

            Rows = newPlane;
            return changed;
        }

        public void Output()
        {
            foreach(var row in Rows)
            {
                Console.WriteLine(row);
            }
            Console.WriteLine();
        }
    }
}