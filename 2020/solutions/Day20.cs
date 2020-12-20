using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

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

            List<string> seaMonster = new List<string>() {
                "                  # ",
                "#    ##    ##    ###",
                " #  #  #  #  #  #   "
            };

            foreach (string i in input)
            {
                if (RegexHelper.Match(i, @"Tile (\d+):", out int tileNumber))
                {
                    currentTileNumber = tileNumber;
                    currentTile = new List<string>();
                }
                else if (!string.IsNullOrEmpty(i))
                {
                    currentTile.Add(i);
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

            int minRow, maxRow, minCol, maxCol = minRow = maxRow = minCol = 0;

            while(tiles.Any())
            {
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

                            minRow = Math.Min(row, minRow);
                            maxRow = Math.Max(row, maxRow);
                            minCol = Math.Min(col, minCol);
                            maxCol = Math.Max(col, maxCol);
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

            long multiplied = locations.First(l => l.Value.Col == minCol && l.Value.Row == minRow).Key *
                locations.First(l => l.Value.Col == maxCol && l.Value.Row == minRow).Key *
                locations.First(l => l.Value.Col == minCol && l.Value.Row == maxRow).Key *
                locations.First(l => l.Value.Col == maxCol && l.Value.Row == maxRow).Key;

            Console.WriteLine($"Part one: {multiplied}");

            int width = maxCol - minCol + 1;
            int height = maxRow - minRow + 1;
            int tileHeight = locations.First().Value.Tile.GetLength(0)-2;
            int tileWidth = locations.First().Value.Tile.GetLength(1)-2;
            char[,] map = new char[height*tileHeight, width*tileWidth];

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

            int rotateCount = 0;
            char[,] mapWithMonsters = map;

            while (rotateCount < 4)
            {
                if (SearchForSeaMonsters(seaMonster, map, out mapWithMonsters) ||
                    SearchForSeaMonsters(seaMonster, FlipHorizontal(map), out mapWithMonsters) ||
                    SearchForSeaMonsters(seaMonster, FlipVertical(map), out mapWithMonsters) ||
                    SearchForSeaMonsters(seaMonster, FlipHorizontal(FlipVertical(map)), out mapWithMonsters)) break;

                map = Rotate(map);
                rotateCount++;
            }

            int roughWaters = 0;
            foreach(char c in mapWithMonsters)
            {
                if (c == '#')
                {
                    roughWaters++;
                }
            }

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
            for (int i = 0; i < tileOne.GetLength(1); i++)
            {
                if (tileOne[tileOne.GetLength(0)-1,i] != tileTwo[0,i])
                {
                    return false;
                }
            }
            return true;
        }

        public static bool CheckLeft(char[,] tileOne, char[,] tileTwo)
        {
            for (int i = 0; i < tileOne.GetLength(0);i++)
            {
                if (tileOne[i,tileOne.GetLength(1)-1] != tileTwo[i,0])
                {
                    return false;
                }
            }
            return true;
        }

        public static bool Compare(char[,] tileOne, char[,] tileTwo, out int rowChange, out int colChange)
        {
            rowChange = colChange = 0;

            colChange = CheckLeft(tileOne, tileTwo) ? 1 : colChange;
            colChange = CheckLeft(tileTwo, tileOne) ? -1 : colChange;
            rowChange = CheckBelow(tileOne, tileTwo) ? 1 : rowChange;
            rowChange = CheckBelow(tileTwo, tileOne) ? -1 : rowChange;

            return rowChange != colChange;
        }

        public static bool SearchForSeaMonsters(List<string> seaMonster, char[,] map, out char[,] newMap)
        {
            bool found = false;
            newMap = map;
            for (int i = 0; i < map.GetLength(0)-seaMonster.Count(); i++)
            {
                for (int j = 0; j < map.GetLength(1) - seaMonster.First().Length; j++)
                {
                    char[,] subMap = new char[seaMonster.Count(),seaMonster.First().Length];
                    for (int y = 0; y < seaMonster.Count(); y++)
                    {
                        for (int x = 0; x < seaMonster.First().Length; x++)
                        {
                            subMap[y,x] = map[i+y, j+x];
                        }
                    }
                    if (ContainsSeaMonster(seaMonster, subMap, out char[,] newSubMap))
                    {
                        for(int y = 0; y < newSubMap.GetLength(0); y++)
                        {
                            for (int x = 0; x < newSubMap.GetLength(1); x++)
                            {
                                newMap[i+y, j+x] = newSubMap[y,x];
                            }
                        }
                        found = true;
                    }
                }
            }
            return found;
        }

        public static bool ContainsSeaMonster(List<string> seaMonster, char[,] location, out char[,] newLocation)
        {
            newLocation = location;
            for (int i = 0; i < seaMonster.Count(); i++)
            {
                for(int j = 0; j < seaMonster[i].Length; j++)
                {
                    if (seaMonster[i][j] == '#' && seaMonster[i][j] != location[i,j])
                    {
                        return false;
                    }
                    else if (seaMonster[i][j] == '#')
                    {
                        newLocation[i,j] = 'O';
                    }
                }
            }

            return true;
        }
    }
}