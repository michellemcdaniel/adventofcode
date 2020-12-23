using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day20
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();
            input.Add("");

            Dictionary<int, char[,]> tiles = new Dictionary<int, char[,]>();
            Dictionary<long, Location> locations = new Dictionary<long, Location>();
            int currentTileNumber = 0;
            List<string> currentTile = new List<string>();

            foreach (string i in input)
            {
                if (RegexHelper.Match(i, @"Tile (\d+):", out int tileNumber))
                {
                    currentTileNumber = tileNumber;
                    currentTile = new List<string>();
                }
                else if (!string.IsNullOrEmpty(i))
                {
                    currentTile.Add(i.Replace('.',' ').Replace('#', '.'));
                }
                else
                {
                    char[,] tile = new char[currentTile.Count(), currentTile.First().Length];
                    for(int x = 0; x < tile.GetLength(0); x++)
                    {
                        for(int y = 0; y < tile.GetLength(1); y++)
                        {
                            tile[x,y] = currentTile[x][y];
                        }
                    }
                    tiles.Add(currentTileNumber, tile);
                }
            }

            // Prepopulate the list of locations with the first tile. The first tile will always be in the map
            locations.Add(tiles.First().Key, new Location(0,0,tiles.First().Value));
            tiles.Remove(tiles.First().Key);

            while(tiles.Any())
            {
                DumpTile(BuildMap(locations));
                List<int> tilesToRemove = new();
                Dictionary<int, Location> tilesToAdd = new();
                foreach(var location in locations)
                {
                    foreach (var tile in tiles)
                    {
                        if (tilesToAdd.ContainsKey(tile.Key))
                        {
                            continue;
                        }

                        if (CompareRotated(location.Value.Tile, tile.Value, out char[,] changed, out int rowChange, out int colChange))
                        {
                            int row = location.Value.Row + rowChange;
                            int col = location.Value.Col + colChange;

                            tilesToAdd.Add(tile.Key, new Location(row, col, changed));
                            tilesToRemove.Add(tile.Key);
                        }
                    }
                }

                foreach(int i in tilesToRemove)
                {
                    tiles.Remove(i);
                }
                foreach (var kvp in tilesToAdd)
                {
                    locations.Add(kvp.Key, kvp.Value);
                }
            }

            var minMin = locations.OrderBy(l => l.Value.Row).OrderBy(l => l.Value.Col).First();
            var minMax = locations.OrderBy(l => l.Value.Row).OrderByDescending(l => l.Value.Col).First();
            var maxMin = locations.OrderByDescending(l => l.Value.Row).OrderBy(l => l.Value.Col).First();
            var maxMax = locations.OrderByDescending(l => l.Value.Row).OrderByDescending(l => l.Value.Col).First();

            long multiplied = minMin.Key * minMax.Key * maxMin.Key * maxMax.Key;
            char[,] map = BuildMap(locations);
            DumpTile(map);

            

            int rotateCount = 0;
            char[,] mapWithMonsters = map;
            List<string> seaMonster = new List<string>() {
                "                  # ",
                "#    ##    ##    ###",
                " #  #  #  #  #  #   "
            };
            List<(int, int)> seaMonsterLocations = new();

            for(int i = 0; i < seaMonster.Count(); i++)
            {
                for(int j = 0; j < seaMonster[i].Length; j++)
                {
                    if (seaMonster[i][j] == '#') seaMonsterLocations.Add((i,j));
                }
            }

            while (rotateCount < 4)
            {
                if (SearchForSeaMonsters(seaMonsterLocations, map, out mapWithMonsters) ||
                    SearchForSeaMonsters(seaMonsterLocations, FlipHorizontal(map), out mapWithMonsters) ||
                    SearchForSeaMonsters(seaMonsterLocations, FlipVertical(map), out mapWithMonsters) ||
                    SearchForSeaMonsters(seaMonsterLocations, FlipHorizontal(FlipVertical(map)), out mapWithMonsters)) break;

                map = Rotate(map);
                rotateCount++;
            }

            int roughWaters = 0;
            foreach(char c in mapWithMonsters)
            {
                if (c == '.')
                {
                    roughWaters++;
                }
            }

            DumpTile(mapWithMonsters);

            Console.WriteLine($"Part one: {multiplied}");
            Console.WriteLine($"Part two: {roughWaters}");
        }

        public class Location
        {
            public int Row { get; set; }
            public int Col { get; set; }
            public char[,] Tile { get; set;}

            public Location(int row, int col, char[,] tile)
            {
                Row = row;
                Col = col;
                Tile = tile;
            }
        }

        public static char[,] BuildMap(Dictionary<long, Location> locations)
        {
            int minRow = locations.Select(l => l.Value.Row).Min();
            int maxRow = locations.Select(l => l.Value.Row).Max();
            int minCol = locations.Select(l => l.Value.Col).Min();
            int maxCol = locations.Select(l => l.Value.Col).Max();

            int tileHeight = locations.First().Value.Tile.GetLength(0)-2;
            int tileWidth = locations.First().Value.Tile.GetLength(1)-2;

            int height = maxRow - minRow + 1;
            int width = maxCol - minCol + 1;

            char[,] map = new char[height*tileHeight, width*tileWidth];

            for(int i = 0; i < map.GetLength(0); i++)
            {
                for(int j = 0; j < map.GetLength(1); j++)
                {
                    map[i,j] = ' ';
                }
            }

            foreach(var location in locations)
            {
                Location loc = location.Value;
                int newRow = loc.Row - minRow;
                int newCol = loc.Col - minCol;

                for (int i = 0; i < loc.Tile.GetLength(0)-2; i++)
                {
                    for (int j = 0; j < loc.Tile.GetLength(1)-2; j++)
                    {
                        map[newRow*tileHeight+i, newCol*tileWidth+j] = loc.Tile[i+1,j+1];
                    }
                }
            }

            return map;
        }

        public static char[,] Rotate(char[,] original)
        {
            char[,] newTile = new char[original.GetLength(1), original.GetLength(0)];
            for(int x = 0; x < original.GetLength(0); x++)
            {
                for (int y = 0; y < original.GetLength(1); y++)
                {
                    newTile[y,x] = original[x,y];
                }
            }

            return newTile;
        }

        public static void DumpTile(char[,] tile)
        {
            for(int i = 0; i < tile.GetLength(0); i++)
            {
                for(int j = 0; j < tile.GetLength(1); j++)
                {
                    Console.Write(tile[i,j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        public static bool CompareFlipped(char[,] tileOne, char[,] tileTwo, out char[,] flipped, out int rowChange, out int colChange)
        {
            flipped = FlipVertical(tileTwo);

            if (Compare(tileOne, flipped, out rowChange, out colChange))
            {
                return true;
            }

            flipped = FlipHorizontal(tileTwo);

            if (Compare(tileOne, flipped, out rowChange, out colChange))
            {
                return true;
            }

            flipped = FlipHorizontal(FlipVertical(tileTwo));

            if (Compare(tileOne, flipped, out rowChange, out colChange))
            {
                return true;
            }

            flipped = tileTwo;
            return false;
        }

        public static char[,] FlipVertical(char[,] original)
        {
            char[,] newTile = new char[original.GetLength(1), original.GetLength(0)];
            int originalLength = original.GetLength(0);

            for(int x = 0; x < original.GetLength(0); x++)
            {
                for (int y = 0; y < original.GetLength(1); y++)
                {
                    newTile[x, originalLength - y - 1] = original[x,y];
                }
            }

            return newTile;
        }

        public static char[,] FlipHorizontal(char[,] original)
        {
            char[,] newTile = new char[original.GetLength(1), original.GetLength(0)];
            int originalLength = original.GetLength(1);

            for(int x = 0; x < original.GetLength(0); x++)
            {
                for (int y = 0; y < original.GetLength(1); y++)
                {
                    newTile[originalLength - x - 1,y] = original[x,y];
                }
            }

            return newTile;
        }

        public static bool CompareRotated(char[,] tileOne, char[,] tileTwo, out char[,] rotated, out int rowChange, out int colChange)
        {
            rowChange = colChange = 0;
            rotated = tileTwo;
            for (int i = 0; i < 4; i++)
            {
                if (Compare(tileOne, rotated, out rowChange, out colChange))
                {
                    return true;
                }
                else if(CompareFlipped(tileOne, rotated, out char[,] alsoFlipped, out rowChange, out colChange))
                {
                    rotated = alsoFlipped;
                    return true;
                }
                rotated = Rotate(rotated);
            }
            return false;
        }

        public static bool CheckBelow(char[,] tileOne, char[,] tileTwo)
        {
            string tileOneBorder = string.Join("",Enumerable.Range(0, tileOne.GetLength(1)).Select(x => tileOne[tileOne.GetLength(0)-1,x]));
            string tileTwoBorder = string.Join("", Enumerable.Range(0, tileTwo.GetLength(1)).Select(x => tileTwo[0,x]));
            return tileOneBorder == tileTwoBorder;
        }

        public static bool CheckLeft(char[,] tileOne, char[,] tileTwo)
        {
            string tileOneBorder = string.Join("",Enumerable.Range(0, tileOne.GetLength(0)).Select(x => tileOne[x, tileOne.GetLength(1)-1]));
            string tileTwoBorder = string.Join("", Enumerable.Range(0, tileTwo.GetLength(0)).Select(x => tileTwo[x,0]));
            return tileOneBorder == tileTwoBorder;
        }

        public static bool Compare(char[,] tileOne, char[,] tileTwo, out int rowChange, out int colChange)
        {
            colChange = CheckLeft(tileOne, tileTwo) ? 1 : 0;
            colChange = colChange == 0 && CheckLeft(tileTwo, tileOne) ? -1 : colChange;
            rowChange = colChange == 0 && CheckBelow(tileOne, tileTwo) ? 1 : 0;
            rowChange = colChange == 0 && rowChange == 0 && CheckBelow(tileTwo, tileOne) ? -1 : rowChange;

            return rowChange != colChange;
        }

        public static bool SearchForSeaMonsters(List<(int, int)> seaMonsterLocations, char[,] map, out char[,] newMap)
        {
            bool found = false;
            newMap = map;

            for (int i = 0; i < map.GetLength(0)-seaMonsterLocations.Select(x => x.Item1).Max(); i++)
            {
                for (int j = 0; j < map.GetLength(1) - seaMonsterLocations.Select(x => x.Item2).Max(); j++)
                {
                    found = ContainsSeaMonster(seaMonsterLocations, newMap, i, j, out newMap) ? true : found;
                }
            }
            return found;
        }

        public static bool ContainsSeaMonster(List<(int, int)> seaMonsterLocations, char[,] location, int iOffset, int jOffset, out char[,] newLocation)
        {
            newLocation = location.Clone() as char[,];

            foreach((int i, int j) in seaMonsterLocations)
            {
                if (location[i+iOffset,j+jOffset] == '.')
                {
                    newLocation[i+iOffset,j+jOffset] = '@';
                }
                else
                {
                    newLocation = location;
                    return false;
                }
            }

            return true;
        }
    }
}