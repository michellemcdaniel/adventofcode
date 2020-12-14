using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day14
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();

            Dictionary<long, long> mem = new Dictionary<long, long>();
            char[] currentMask = new char[0];
            
            string maskPattern = @"^mask = (.*)";
            string memPattern = @"^mem\[(\d+)\] = (\d+)$";

            foreach(var line in input)
            {
                if (RegexHelper.Match(line, maskPattern, out string mask))
                {
                    currentMask = mask.ToCharArray();
                }
                else
                {
                    RegexHelper.Match(line, memPattern, out string memLocation, out string originalValue);
                    char[] valueChar = Convert.ToString(long.Parse(originalValue), 2).PadLeft(36, '0').ToCharArray();

                    for (int i = 0; i < currentMask.Length; i++)
                    {
                        if (currentMask[i] != 'X')
                        {
                            valueChar[i] = currentMask[i];
                        }
                    }

                    long value = Convert.ToInt64(new string(valueChar), 2);

                    if (mem.ContainsKey(long.Parse(memLocation)))
                    {
                        mem[long.Parse(memLocation)] = value;
                    }
                    else
                    {
                        mem.Add(long.Parse(memLocation), value);
                    }
                }
            }

            Console.WriteLine($"Part One: {mem.Values.Sum()}");

            mem.Clear();
            foreach(var line in input)
            {
                if (RegexHelper.Match(line, maskPattern, out string mask))
                {
                    currentMask = mask.ToCharArray();
                }
                else
                {
                    RegexHelper.Match(line, memPattern, out string index, out long value);
                    char[] memChar = Convert.ToString(long.Parse(index), 2).PadLeft(36, '0').ToCharArray();

                    for (int i = 0; i < currentMask.Length; i++)
                    {
                        if (currentMask[i] != '0')
                        {
                            memChar[i] = currentMask[i];
                        }
                    }

                    List<string> allAddresses = GetAllMemAddresses("", memChar);

                    foreach(var address in allAddresses)
                    {
                        mem[Convert.ToInt64(address, 2)] = value;
                    }
                }
            }    
            
            Console.WriteLine($"Part Two: {mem.Values.Sum()}");
        }

        public static List<string> GetAllMemAddresses(string prior, char[] mem)
        {
            if (mem.Length == 1)
            {
                if (mem[0] == 'X')
                    return new List<string>(){
                        $"{prior}0",
                        $"{prior}1"
                    };
                else
                    return new List<string>(){ $"{prior}{mem[0]}" };
            }
            else
            {
                if (mem[0] == 'X')
                {
                    List<string> zeros = GetAllMemAddresses($"{prior}0", mem[1..]);
                    zeros.AddRange(GetAllMemAddresses($"{prior}1", mem[1..]));

                    return zeros;
                }
                else
                {
                    return GetAllMemAddresses($"{prior}{mem[0]}", mem[1..]);
                }
            }
        }
    }
}