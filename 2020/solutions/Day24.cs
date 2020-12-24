using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace adventofcode
{
    class Day24
    {
        public static List<(int, double)> Neighbors = new List<(int, double)>()
        {
            (0,-1), (0,1), (-1,-0.5), (-1, 0.5), (1,-0.5), (1, 0.5)
        };
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();

            Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();
            tiles.Add("0+0", new Tile() {Row = 0, Column = 0});

            foreach(var line in input)
            {
                int row = 0;
                double col = 0;
                for (int i = 0; i < line.Length; i++)
                {
                    if (line[i] == 'e') col++;
                    else if (line[i] == 'w') col--;
                    else
                    {
                        if (line[i] == 'n') row--;
                        else row++;

                        i++;
                        if (line[i] == 'e') col+=0.5;
                        else col-=0.5;
                    }
                }

                if (tiles.TryGetValue($"{row}+{col}", out Tile tile))
                {
                    tile.Color = !tile.Color;
                }
                else
                {
                    tiles.Add($"{row}+{col}", new Tile() { Color = true, Row = row, Column = col});
                }
            }

            Console.WriteLine($"Part One: {tiles.Where(kvp => kvp.Value.Color).Count()}");

            for (int i = 0; i < 100; i++)
            {
                HashSet<string> changedTiles = new HashSet<string>();
                Queue<Tile> toCheck = new Queue<Tile>(tiles.Where(kvp => kvp.Value.Color).Select(kvp => kvp.Value));
                HashSet<string> keysChecked = new HashSet<string>(toCheck.Select(t => $"{t.Row}+{t.Column}"));

                while (toCheck.Any())
                {
                    Tile tile = toCheck.Dequeue();
                    int count = 0;
                    
                    foreach (var neighbor in Neighbors)
                    {
                        string key = $"{tile.Row+neighbor.Item1}+{tile.Column+neighbor.Item2}";
                        if (TileIsBlack(key, tiles))
                        {
                            count++;
                        }
                        else if (tile.Color && keysChecked.Add(key))
                        {
                            Tile toAdd = new Tile() { Row = tile.Row+neighbor.Item1, Column = tile.Column+neighbor.Item2 };
                            toCheck.Enqueue(toAdd);
                            tiles[key] = toAdd;
                        }
                    }

                    if ((!tile.Color && count == 2) ||
                        (tile.Color && (count == 0 || count > 2)))
                    {
                        changedTiles.Add($"{tile.Row}+{tile.Column}");
                    }
                }

                foreach(var tile in changedTiles)
                {
                    tiles[tile].Color = !tiles[tile].Color;
                }
            }

            Console.WriteLine($"Part Two: {tiles.Where(kvp => kvp.Value.Color).Count()}");
        }

        public static bool TileIsBlack(string key, Dictionary<string, Tile> tiles)
        {
            if (tiles.TryGetValue(key, out Tile tile))
            {
                return tile.Color;
            }
            return false;
        }

        public static void DumpGrid(List<Tile> tiles)
        {
            for(int i = tiles.Select(t => t.Row).Min(); i <= tiles.Select(t => t.Row).Max(); i++)
            {
                for (double j = tiles.Select (t => t.Column * 2).Min(); j <= tiles.Select(t => t.Column * 2).Max(); j+=1)
                {
                    Tile tile = tiles.FirstOrDefault(t => t.Row == i && t.Column*2 == j);
                    if (tile != null && tile.Color) Console.Write("#");
                    else if (tile != null && !tile.Color) Console.Write(".");
                    else Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public class Tile
        {
            public bool Color { get; set; } // white = false; black = true
            public int Row { get; set; }
            public double Column { get; set; }
        }
    }
}