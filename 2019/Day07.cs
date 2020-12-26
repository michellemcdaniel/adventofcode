using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Nineteen
{
    class Day07
    {
        public static void Execute(string filename)
        {
            string intcode = File.ReadAllText(filename);

            List<List<int>> permutations = GetPermutations(new List<int>{0,1,2,3,4});
            long maxOutput = long.MinValue;
            List<int> maxPermutation = new List<int>();

            foreach (var permutation in permutations)
            {
                long output = CalculateOutput(permutation, intcode, false);

                if (output > maxOutput)
                {
                    maxOutput = output;
                }
            }

            Console.WriteLine($"Part one: {maxOutput}");

            permutations = GetPermutations(new List<int>{5,6,7,8,9});
            maxOutput = long.MinValue;
            maxPermutation = new List<int>();

            foreach(var permutation in permutations)
            {
                long output = CalculateOutput(permutation, intcode, true);

                if (output > maxOutput)
                {
                    maxOutput = output;
                }
            }

            Console.WriteLine($"Part Two: {maxOutput}");
        }

        public static long CalculateOutput(List<int> permutation, string intcode, bool pause)
        {
            List<IntCode> intCodes = new List<IntCode>()
            {
                new IntCode(intcode.Split(",").ToList().Select(o => long.Parse(o)).ToArray(), pause),
                new IntCode(intcode.Split(",").ToList().Select(o => long.Parse(o)).ToArray(), pause),
                new IntCode(intcode.Split(",").ToList().Select(o => long.Parse(o)).ToArray(), pause),
                new IntCode(intcode.Split(",").ToList().Select(o => long.Parse(o)).ToArray(), pause),
                new IntCode(intcode.Split(",").ToList().Select(o => long.Parse(o)).ToArray(), pause)
            };

            long inputToIntCode = 0;

            for (int i = 0; i < permutation.Count; i++)
            {
                inputToIntCode = intCodes[i].Compute(inputToIntCode, permutation[i]);
            }

            while (!intCodes.Last().Halted)
            {
                for(int i = 0; i < intCodes.Count; i++)
                {
                    intCodes[i].Resume(inputToIntCode);
                    inputToIntCode = intCodes[i].ComputeResult();
                }
            }
            
            return inputToIntCode;
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
