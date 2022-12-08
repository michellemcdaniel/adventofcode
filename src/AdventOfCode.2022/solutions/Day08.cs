using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.TwentyTwo
{
    public class Day08
    {
        public static void Execute(string filename)
        {
            string[] lines = File.ReadAllLines(filename);
            int height= lines.Length;
            int width = lines[0].Length;
            int[,] map = new int[height,width];

            for(int i = 0; i < lines.Length; i++)
            {
                for(int j = 0; j < lines[i].Length; j++)
                {
                    map[i,j] = int.Parse(lines[i][j].ToString());
                }
            }

            int visibleFromOutside = height*2 + (width - 2)*2;
            int highestScenicScore = 0;

            for (int i = 1; i < height - 1; i++)
            {
                for (int j = 1; j < width - 1; j++)
                {
                    bool tallerLeft = false;
                    bool tallerRight = false;
                    bool tallerUp = false;
                    bool tallerDown = false;

                    int leftScore = 0;
                    int rightScore = 0;
                    int upScore = 0;
                    int downScore = 0;

                    for (int k = j-1; k >= 0; k--)
                    {
                        if (map[i,k] >= map[i,j])
                        {
                            // taller. Stop looking this direction
                            tallerLeft = true;
                            leftScore = j-k;
                            break;
                        }
                    }

                    if (!tallerLeft)
                    {
                        leftScore = j;
                    }

                    for (int k = j+1; k < width; k++)
                    {
                        if (map[i,k] >= map[i,j])
                        {
                            // taller. Stop looking this direction
                            tallerRight = true;
                            rightScore = k-j;
                            break;
                        }
                    }

                    if (!tallerRight)
                    {
                        rightScore = width - j - 1;
                    }
                    
                    for (int k = i-1; k >= 0; k--)
                    {
                        if (map[k,j] >= map[i,j])
                        {
                            tallerUp = true;
                            upScore = i-k;
                            break;
                        }
                    }

                    if (!tallerUp)
                    {
                        upScore = i;
                    }

                    for (int k = i+1; k < height; k++)
                    {
                        if (map[k,j] >= map[i,j])
                        {
                            tallerDown = true;
                            downScore = k-i;
                            break;
                        }
                    }

                    if (!tallerDown)
                    {
                        downScore = height - i - 1;
                    }

                    if (!(tallerRight && tallerDown && tallerUp && tallerLeft))
                    {
                        visibleFromOutside++;
                    }

                    int totalScore = leftScore * rightScore * upScore * downScore;
                    highestScenicScore = totalScore > highestScenicScore ? totalScore : highestScenicScore;
                    
                }
            }

            Console.WriteLine($"Part one: {visibleFromOutside}");
            Console.WriteLine($"Part two: {highestScenicScore}");
        }
    }
}
