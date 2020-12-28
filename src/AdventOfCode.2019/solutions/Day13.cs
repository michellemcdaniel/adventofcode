using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Nineteen
{
    public enum TileId
    {
        Empty,
        Wall,
        Block,
        HorizontalPaddle,
        Ball
    }
    public class Day13
    {
        public static void Execute(string filename)
        {
            string input = File.ReadAllText(filename);

            IntCode intCode = new IntCode(input.Split(",").ToList().Select(o => long.Parse(o)).ToArray(), true, true);
            Dictionary<(long, long), TileId> tiles = new();
            
            long score = PlayGame(intCode, tiles);

            int count = tiles.Where(kvp => kvp.Value == TileId.Block).Count();
            Console.WriteLine($"Part One: {count}");

            intCode = new IntCode(input.Split(",").ToList().Select(o => long.Parse(o)).ToArray(), true, true);
            intCode.Opcodes[0] = 2;
            tiles = new();
            long currentScore = PlayGame(intCode, tiles);
            Console.WriteLine($"Part Two: {currentScore}");
        }

        public static long PlayGame(IntCode intCode, Dictionary<(long, long), TileId> tiles)
        {
            long currentScore = 0;

            long x = 0;
            long y = 0;
            int iterations = 0;
            int joyStick = 0;
            long ballPosition = 0;
            long paddlePosition = 0;

            while(!intCode.Halted)
            {
                long output = 0;
                joyStick = ballPosition.CompareTo(paddlePosition);

                while(!intCode.Paused && !intCode.Halted)
                {
                    output = intCode.Compute(joyStick);
                }
                
                switch(iterations%3)
                {
                    case 0:
                        x = output;
                        break;
                    case 1:
                        y = output;
                        break;
                    case 2:
                        if (x == -1 && y == 0)
                        {
                            currentScore = output;
                        }
                        else
                        {
                            switch((TileId)output)
                            {
                                case TileId.Ball:
                                    ballPosition = x;
                                    break;
                                case TileId.HorizontalPaddle:
                                    paddlePosition = x;
                                    break;
                                default:
                                    break;
                            }
                            tiles[(x,y)] = (TileId)output;
                        }
                        break;
                }
                iterations++;
                intCode.Paused = false;
            }

            return currentScore;
        }

        public static void DrawGame(Dictionary<(long, long), TileId> tiles)
        {
            long minRow = tiles.Keys.Select(key => key.Item1).Min();
            long maxRow = tiles.Keys.Select(key => key.Item1).Max();
            long minCol = tiles.Keys.Select(key => key.Item2).Min();
            long maxCol = tiles.Keys.Select(key => key.Item2).Max();

            for (long i = minRow; i < maxRow; i++)
            {
                for (long j = minCol; j < maxCol; j++)
                {
                    if(tiles.TryGetValue((i,j), out TileId tileId))
                    {
                        switch(tileId)
                        {
                            case TileId.Empty:
                                Console.Write(" ");
                                break;
                            case TileId.Block:
                                Console.Write("#");
                                break;
                            case TileId.Ball:
                                Console.Write("@");
                                break;
                            case TileId.HorizontalPaddle:
                                Console.Write("=");
                                break;
                            case TileId.Wall:
                                Console.Write("|");
                                break;
                        }
                    }
                    else
                        Console.Write(" ");
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
