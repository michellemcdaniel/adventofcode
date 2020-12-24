using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace adventofcode
{
    class Day24
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();

            Dictionary<string, Tile> tiles = new Dictionary<string, Tile>();
            tiles.Add("0+0", new Tile() {Color = true, Row = 0, Column = 0});

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
                    tiles.Add($"{row}+{col}", new Tile() { Color = false, Row = row, Column = col});
                }
            }

            Console.WriteLine($"Part One: {tiles.Where(kvp => !kvp.Value.Color).Count()}");

            for(int i = -65; i < 67; i++)
            {
                for (double j = -54; j < 60; j++)
                {
                    double col = 0;
                    if (i%2 == 0)
                    {
                        col = Math.Ceiling(j);
                    }
                    else
                    {
                        col = j % 1 == 0 ? j+0.5 : j;
                    }

                    if (j <= 100 && !tiles.ContainsKey($"{i}+{col}"))
                    {
                        tiles.Add($"{i}+{col}", new Tile() { Color = true, Row = i, Column = col});
                    }
                }
            }

            for (int i = 0; i < 100; i++)
            {
                List<Tile> changedTiles = new List<Tile>();
                int maxRow = tiles.Where(kvp => !kvp.Value.Color).Select(kvp => kvp.Value.Row).Max()+1;
                int minRow = tiles.Where(kvp => !kvp.Value.Color).Select(kvp => kvp.Value.Row).Min()-1;
                double minCol = tiles.Where(kvp => !kvp.Value.Color).Select(kvp => kvp.Value.Column).Min()-1;
                double maxCol = tiles.Where(kvp => !kvp.Value.Color).Select(kvp => kvp.Value.Column).Max()+1;

                foreach (var kvp in 
                    tiles.Where(kvp => kvp.Value.Row >= minRow && kvp.Value.Row <= maxRow && kvp.Value.Column >= minCol && kvp.Value.Column <= maxCol))
                {
                    int count = TileIsBlack(kvp.Value.Row, kvp.Value.Column+1, tiles) + 
                        TileIsBlack(kvp.Value.Row, kvp.Value.Column-1, tiles) + 
                        TileIsBlack(kvp.Value.Row+1, kvp.Value.Column+0.5, tiles) +
                        TileIsBlack(kvp.Value.Row-1, kvp.Value.Column+0.5, tiles) +
                        TileIsBlack(kvp.Value.Row+1, kvp.Value.Column-0.5, tiles) +
                        TileIsBlack(kvp.Value.Row-1, kvp.Value.Column-0.5, tiles);

                    if ((kvp.Value.Color && count == 2) ||
                        (!kvp.Value.Color && (count == 0 || count > 2)))
                    {
                        changedTiles.Add(new Tile() { Color = !kvp.Value.Color, Row = kvp.Value.Row, Column = kvp.Value.Column });
                    }
                }

                foreach(var tile in changedTiles)
                {
                    tiles[$"{tile.Row}+{tile.Column}"] = tile;
                }
            }

            Console.WriteLine($"Part Two: {tiles.Where(kvp => !kvp.Value.Color).Count()}");
        }

        public static int TileIsBlack(int row, double col, Dictionary<string, Tile> tiles)
        {
            if (tiles.TryGetValue($"{row}+{col}", out Tile tile))
            {
                return tile.Color ? 0 : 1;
            }
            return 0;
        }

        public class Tile
        {
            public bool Color { get; set; } // black = false; white = true
            public int Row { get; set; }
            public double Column { get; set; }
        }
    }
}