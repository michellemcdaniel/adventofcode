using System;
using System.Collections.Generic;
using System.Linq;

namespace adventofcode
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
            return (x > 0 && y > 0 && x < Width && y < Height);
        }

        public bool IsOccupied(int x, int y)
        {
            return Exists(x,y) && Rows[y][x] == '#';
        }

        public int ActiveAround(int x, int y, int z, int w)
        {
            int totalActive = 0;
            for (int i = -1; i < 2; i++)
            {
                for (int j = -1; j < 2; j++)
                {
                    if (i == 0 && j == 0 && z == 0 && w == 0)
                    {
                        continue;
                    }

                    if (IsOccupied(x+i, y+j))
                    {
                        totalActive++;
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

        public void Output()
        {
            foreach(var row in Rows)
            {
                Console.WriteLine(row);
            }
            Console.WriteLine();
        }
    }

    public class Cube
    {
        public Dictionary<int, Plane> Planes { get; set; }
        public int MinIndex { get; set; }

        public Cube()
        {
            Planes = new();
        }

        public Cube(int index, Plane plane)
        {
            Planes = new Dictionary<int, Plane>();
            Planes.Add(index, plane);
        }
        
        public Cube(int z, int depth, int width, int height)
        {
            Planes = new Dictionary<int, Plane>();
            for (int i = z; i < z+depth; i++)
            {
                Planes.Add(i, new Plane(width, height));
            }
        }

        public int GetMinIndex()
        {
            return Planes.Keys.Min();
        }

        public int GetMaxIndex()
        {
            return Planes.Keys.Max();
        }

        public int GetDepth()
        {
            return GetMaxIndex() - GetMinIndex() + 1;
        }

        public int GetHeight()
        {
            return Planes.First().Value.Height;
        }

        public int GetWidth()
        {
            return Planes.First().Value.Width;
        }

        public bool PlaneExists(int z)
        {
            return Planes.ContainsKey(z);
        }

        public void Expand()
        {
            foreach(var plane in Planes)
            {
                plane.Value.Expand();
            }

            Planes.Add(GetMinIndex()-1, new Plane(Planes.First().Value.Width, Planes.First().Value.Height));
            Planes.Add(GetMaxIndex()+1, new Plane(Planes.First().Value.Width, Planes.First().Value.Height));
        }

        public int ActiveAround(int z, int x, int y)
        {
            return ActiveAround(z, x, y, 0);
        }

        public int ActiveAround(int z, int x, int y, int w)
        {
            int totalActive = 0;
            for (int i = -1; i < 2; i++)
            {
                if (PlaneExists(z+i))
                {
                    totalActive += Planes[z+i].ActiveAround(x,y,i,w);
                }
            }
            return totalActive;
        }

        public int CountOccupied()
        {
            int totalActive = 0;
            foreach(var plane in Planes)
            {
                totalActive += plane.Value.CountOccupied();
            }
            return totalActive;
        }

        public Cube Create(Func<bool, int, char> CheckRules, HyperCube hyperCube, int w)
        {
            Cube newCube = new();
            foreach (var plane in Planes)
            {
                int z = plane.Key;
                newCube.Planes.Add(z, plane.Value.Create(CheckRules, hyperCube, w, z));
            }
            return newCube;
        }

        public void Generate(Func<bool, int, char> CheckRules)
        {
            Cube newCube = new();
            foreach (var plane in Planes)
            {
                int z = plane.Key;
                newCube.Planes.Add(z, plane.Value.Create(CheckRules, this, z));
            }
            Planes = newCube.Planes;
        }
    }

    public class HyperCube
    {
        public Dictionary<int, Cube> Cubes { get; set; }

        public HyperCube()
        {
            Cubes = new Dictionary<int, Cube>();
        }

        public HyperCube(int index, Cube cube)
        {
            Cubes = new Dictionary<int, Cube>();
            Cubes.Add(index, cube);
        }

        public int GetMinIndex()
        {
            return Cubes.Keys.Min();
        }

        public int GetMaxIndex()
        {
            return Cubes.Keys.Max();
        }

        public bool CubeExists(int w)
        {
            return Cubes.ContainsKey(w);
        }

        public void Expand()
        {
            foreach(var cube in Cubes)
            {
                cube.Value.Expand();
            }

            Cubes.Add(GetMinIndex()-1, new Cube(Cubes.First().Value.GetMinIndex(), Cubes.First().Value.GetDepth(), Cubes.First().Value.GetWidth(), Cubes.First().Value.GetHeight()));
            Cubes.Add(GetMaxIndex()+1, new Cube(Cubes.First().Value.GetMinIndex(), Cubes.First().Value.GetDepth(), Cubes.First().Value.GetWidth(), Cubes.First().Value.GetHeight()));
        }

        public int ActiveAround(int w, int z, int x, int y)
        {
            int totalActive = 0;
            for (int i = -1; i < 2; i++)
            {
                if (CubeExists(w+i))
                {
                    totalActive+= Cubes[w+i].ActiveAround(z, x, y, i);
                }
            }
            return totalActive;
        }

        public int CountOccupied()
        {
            int totalActive = 0;
            foreach (var cube in Cubes)
            {
                totalActive += cube.Value.CountOccupied();
            }
            return totalActive;
        }

        public void Generate(Func<bool, int, char> CheckRules)
        {
            HyperCube newHyperCube = new();

            foreach (var c in Cubes)
            {
                int w = c.Key;
                newHyperCube.Cubes.Add(w, c.Value.Create(CheckRules, this, w));
            }
            
            Cubes = newHyperCube.Cubes;
        }
    }
}