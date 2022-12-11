using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AdventOfCode.Helpers;

namespace AdventOfCode.TwentyTwo
{
    public class Day11
    {
        public static void Execute(string filename)
        {
            Dictionary<int, Monkey> partOne = new();
            Dictionary<int, Monkey> partTwo = new();
            int currentMonkey = 0;

            foreach (var line in File.ReadLines(filename))
            {
                string currentLine = line.Trim();
                if (currentLine.StartsWith("Monkey"))
                {
                    currentMonkey = int.Parse(currentLine.Split(" ")[1].Replace(":", ""));
                    partOne.Add(currentMonkey, new Monkey(currentMonkey));
                    partTwo.Add(currentMonkey, new Monkey(currentMonkey));
                }
                else if (currentLine.StartsWith("Starting items"))
                {
                    foreach (var item in currentLine.Split(" ").Last().Split(",").Select(i => int.Parse(i)))
                    {
                        partOne[currentMonkey].Items.Enqueue(item);
                        partTwo[currentMonkey].Items.Enqueue(item);
                    }
                }
                else if (currentLine.StartsWith("Operation"))
                {
                    if (RegexHelper.Match(currentLine, @"new = old [\*\+] [0-9]+"))
                    {
                        RegexHelper.Match(currentLine, @"new = old ([\*\+]) ([0-9]+)", out partOne[currentMonkey].OperationId, out partOne[currentMonkey].OperationValue);
                        RegexHelper.Match(currentLine, @"new = old ([\*\+]) ([0-9]+)", out partTwo[currentMonkey].OperationId, out partTwo[currentMonkey].OperationValue);
                    }
                    else if (RegexHelper.Match(currentLine, @"new = old [\*\+] old"))
                    {
                        RegexHelper.Match(currentLine, @"new = old ([\*\+]) old", out partOne[currentMonkey].OperationId);
                        partOne[currentMonkey].UseItemValue = true;

                        RegexHelper.Match(currentLine, @"new = old ([\*\+]) old", out partTwo[currentMonkey].OperationId);
                        partTwo[currentMonkey].UseItemValue = true;
                    }
                }
                else if (currentLine.StartsWith("Test"))
                {
                    partOne[currentMonkey].TestAmount = int.Parse(currentLine.Split(" ").Last());
                    partTwo[currentMonkey].TestAmount = int.Parse(currentLine.Split(" ").Last());
                }
                else if (currentLine.StartsWith("If true"))
                {
                    partOne[currentMonkey].TrueId = int.Parse(currentLine.Split(" ").Last());
                    partTwo[currentMonkey].TrueId = int.Parse(currentLine.Split(" ").Last());
                }
                else if (currentLine.StartsWith("If false"))
                {
                    partOne[currentMonkey].FalseId = int.Parse(currentLine.Split(" ").Last());
                    partTwo[currentMonkey].FalseId = int.Parse(currentLine.Split(" ").Last());
                }
            }

            long reduceWorryAmount = partTwo.Values.Select(m => m.TestAmount).Aggregate((x,y) => x * y);

            CalculateMonkeyBusiness(partOne, 20, item => item / 3);
            CalculateMonkeyBusiness(partTwo, 10000, item => item % reduceWorryAmount);
            
            List<Monkey> sortedMonkeys = partOne.Values.OrderByDescending(m => m.InspectionCount).ToList();
            List<Monkey> sortedPartTwoMonkeys = partTwo.Values.OrderByDescending(m => m.InspectionCount).ToList();

            Console.WriteLine($"Part one: {sortedMonkeys[0].InspectionCount*sortedMonkeys[1].InspectionCount}");
            Console.WriteLine($"Part two: {sortedPartTwoMonkeys[0].InspectionCount*sortedPartTwoMonkeys[1].InspectionCount}");
        }

        public static void CalculateMonkeyBusiness(Dictionary<int, Monkey> monkeys, int iterations, Func<long, long> decreaseWorry)
        {
            for (int i = 0; i < iterations; i++)
            {
                foreach (var kvp in monkeys)
                {
                    Monkey monkey = kvp.Value;
                    while (monkey.Items.Count > 0)
                    {
                        long value = monkey.Items.Dequeue();
                        value = monkey.Inspect(value, decreaseWorry);

                        if (value%monkey.TestAmount == 0)
                        {
                            monkeys[monkey.TrueId].Items.Enqueue(value);
                        }
                        else
                        {
                            monkeys[monkey.FalseId].Items.Enqueue(value);
                        }
                    }
                }
            }
        }
    }

    public class Monkey
    {
        public int Id;
        public Queue<long> Items;
        public char OperationId;
        public int OperationValue;
        public bool UseItemValue;
        public int TestAmount;
        public int TrueId;
        public int FalseId;
        public long InspectionCount;

        public Monkey(int id)
        {
            Id = id;
            InspectionCount = 0;
            Items = new();
        }

        public long Inspect(long value, Func<long, long> decreaseWorry)
        {
            InspectionCount++;
            switch (OperationId)
            {
                case '+':
                    value = UseItemValue ? value * 2 : value + OperationValue;
                    break;
                case '*':
                    value = UseItemValue ? value * value : value * OperationValue;
                    break;
                default:
                    break;
            }

            return decreaseWorry(value);
        }
    }
}
