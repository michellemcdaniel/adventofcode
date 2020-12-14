using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventofcode
{
    class Day14
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();

            Dictionary<long, long> mem = new Dictionary<long, long>();
            char[] currentMask = new char[0];
            
            foreach(var line in input)
            {
                if (line.StartsWith("mask"))
                {
                    // mask stuff
                    string pattern = @"^mask = (?<mask>.*)";
                    Match m = Regex.Match(line, pattern);
                    currentMask = m.Groups["mask"].Value.ToCharArray();
                }
                else
                {
                    string[] match = line.Split(" = ");
                    string memLocation = match[0].Replace("mem[", "").Replace("]", "");

                    long value = long.Parse(match[1]);
                    string valueString = Convert.ToString(value, 2);
                    char[] valueChar = valueString.PadLeft(36, '0').ToCharArray();

                    for (int i = 0; i < currentMask.Length; i++)
                    {
                        if (currentMask[i] == '0')
                        {
                            valueChar[i] = '0';
                        }
                        else if (currentMask[i] == '1')
                        {
                            valueChar[i] = '1';
                        }
                    }

                    value = Convert.ToInt64(new string(valueChar), 2);

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

            long partOne = 0;

            foreach(var value in mem)
            {
                partOne += value.Value;
            }

            mem = new Dictionary<long, long>();

            foreach(var line in input)
            {
                if (line.StartsWith("mask"))
                {
                    // mask stuff
                    string pattern = @"^mask = (?<mask>.*)";
                    Match m = Regex.Match(line, pattern);
                    currentMask = m.Groups["mask"].Value.ToCharArray();
                }
                else
                {
                    string[] match = line.Split(" = ");
                    string memLocation = match[0].Replace("mem[", "").Replace("]", "");

                    long memLocLong = long.Parse(memLocation);

                    long value = long.Parse(match[1]);
                    string memString = Convert.ToString(memLocLong, 2);
                    char[] memChar = memString.PadLeft(36, '0').ToCharArray();

                    for (int i = 0; i < currentMask.Length; i++)
                    {
                        if (currentMask[i] == '1')
                        {
                            memChar[i] = '1';
                        }
                        else if (currentMask[i] == 'X')
                        {
                            memChar[i] = 'X';
                        }
                    }

                    List<string> allAddresses = GetAllMemAddresses("", memChar);

                    foreach(var address in allAddresses)
                    {
                        long memAddress = Convert.ToInt64(address, 2);

                        if (mem.ContainsKey(memAddress))
                        {
                            mem[memAddress] = value;
                        }
                        else
                        {
                            mem.Add(memAddress, value);
                        }

                    }
                }
            }

            long partTwo = 0;
            foreach(var value in mem)
            {
                partTwo += value.Value;
            }
            
            Console.WriteLine($"Part One: {partOne}");
            Console.WriteLine($"Part Two: {partTwo}");
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