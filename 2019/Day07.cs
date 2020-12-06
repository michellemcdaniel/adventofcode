using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day07
    {
        public static void Execute()
        {
            List<string> input = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "input", "day07-sample.txt")).ToList();

            List<List<int>> permutations = GetPermutations(new List<int>{5,6,7,8,9});

            int maxOutput = Int32.MinValue;
            List<int> maxPermutation = new List<int>();

            foreach(var permutation in permutations)
            {
                foreach(string intcode in input)
                {
                    List<IntCode> intCodes = new List<IntCode>()
                    {
                        new IntCode(intcode.Split(",").ToList().Select(o => Int32.Parse(o)).ToArray()),
                        new IntCode(intcode.Split(",").ToList().Select(o => Int32.Parse(o)).ToArray()),
                        new IntCode(intcode.Split(",").ToList().Select(o => Int32.Parse(o)).ToArray()),
                        new IntCode(intcode.Split(",").ToList().Select(o => Int32.Parse(o)).ToArray()),
                        new IntCode(intcode.Split(",").ToList().Select(o => Int32.Parse(o)).ToArray())
                    };

                    Queue<int> inputToIntCode = new Queue<int>();
                    inputToIntCode.Enqueue(0);
                    int startInput = 0;

                    while (true)
                    {
                        for (int i = 0; i < permutation.Count; i++)
                        {
                            startInput = inputToIntCode.Dequeue();
                            inputToIntCode = intCodes[i].Compute(startInput, permutation[i]);
                        }

                        if(inputToIntCode.Count == 1)
                        {
                            break;
                        }

                        for(int i = 0; i < permutation.Count; i++)
                        {
                            inputToIntCode = intCodes[i].Compute(inputToIntCode);
                        }
                    }

                    int output = inputToIntCode.Dequeue();
                    if (output > maxOutput)
                    {
                        maxOutput = output;
                        maxPermutation = permutation;
                    }
                }
            }

            Console.WriteLine($"Max ouptut: {maxOutput}");
            Console.WriteLine(string.Join(",", maxPermutation));
        }

        public static List<List<int>> GetPermutations(List<int> startList)
        {
            List<List<int>> returnList = new List<List<int>>();
            returnList.Add(startList);

            if (startList.Count == 1)
            {
                return returnList;
            }
            if (startList.Count == 2)
            {
                returnList.Add(
                    new List<int>()
                    { 
                        startList.ElementAt(1), 
                        startList.ElementAt(0)
                    }
                );
            }

            foreach (int i in startList)
            {
                List<int> copy = new List<int>(startList);
                copy.Remove(i);
                List<List<int>> perms = GetPermutations(copy);
                
                foreach(var perm in perms)
                {
                    perm.Insert(0, i);
                    returnList.Add(perm);
                }
            }
            return returnList;
        }
    }
}
