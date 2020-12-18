using System;
using System.Collections.Generic;
using System.Linq;

namespace adventofcode
{
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