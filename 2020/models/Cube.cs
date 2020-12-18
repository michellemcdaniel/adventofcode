using System;
using System.Collections.Generic;
using System.Linq;

namespace adventofcode
{
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
}