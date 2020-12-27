using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Nineteen
{
    public class Day16
    {
        public static int[] pattern = new int[] { 0, 1, 0, -1 };
        public static void Execute(string filename)
        {
            string input = File.ReadAllText(filename);

            int[] currentList = new int[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                currentList[i] = int.Parse(input[i].ToString());
            }

            currentList = DoFFT(currentList);
            string locationString = string.Join("",currentList.Take(8));
            Console.WriteLine($"Part One: {locationString}");

            int location = int.Parse(string.Join("",input.Take(7)));
            Console.WriteLine(location);
            currentList = new int[input.Length*10000];
            
            for(int i = 0; i < currentList.Length; i++)
            {
                currentList[i] = int.Parse(input[i%input.Length].ToString());
            }
            Console.WriteLine(string.Join("", currentList.Take(7)));

            currentList = currentList.Skip(location).ToArray();
            

            currentList = DoPartialSums(currentList);
            Console.WriteLine($"Part Two: {string.Join("",currentList.Take(8))}");
        }

        public static int[] DoPartialSums(int[] original)
        {
            int[] current = original;
            int[] output = new int[original.Length];
            output[output.Length-1] = original[original.Length-1];
            Console.WriteLine(output[output.Length-1]);
            for (int p = 0; p < 100; p++)
            {
                for (int i = original.Length-2; i >= 0; i--)
                {
                    output[i] = current[i] + output[i+1];
                    output[i] %= 10; 
                }

                current = output;
            }

            return output;
        }

        public static int[] DoFFT(int[] original)
        {
            int[] currentList = original;
            
            for (int p = 0; p < 100; p++)
            {
                Console.WriteLine($"Starting phase {p}");
                int[] nextList = new int[currentList.Length];

                for(int i = 0; i < currentList.Length; i++)
                {
                    int index = 0;
                    int sum = 0;
                    for (int j = i; j < currentList.Length; j++)
                    {
                        if ((j+1)%(i+1) == 0)
                        {
                            index = (index+1)%4;
                        }

                        if (pattern[index] == 0)
                        {
                            j+=i;
                            continue;
                        }
                        
                        sum += currentList[j]*pattern[index];
                    }
                    nextList[i] = Math.Abs(sum)%10;
                }

                currentList = nextList;
            }
            return currentList;
        }
    }
}
