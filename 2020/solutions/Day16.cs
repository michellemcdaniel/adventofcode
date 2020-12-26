using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Twenty
{
    class Day16
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();

            Dictionary<string, List<Range>> rules = new();
            int nextInput = 0;
            
            for (int i = 0; i < input.Count; i++)
            {
                nextInput = i+1;
                if (string.IsNullOrEmpty(input[i]))
                {
                    break;
                }

                RegexHelper.Match(input[i], @"(.*): (\d+)-(\d+) or (\d+)-(\d+)$", out string field, out int rangeOneLow, out int rangeOneHigh, out int rangeTwoLow, out int rangeTwoHigh);
                rules.Add(field, new List<Range>() { new Range(rangeOneLow, rangeOneHigh), new Range(rangeTwoLow, rangeTwoHigh)});
            }

            nextInput++;
            int[] myTicket = input[nextInput].Split(",").Select(n => int.Parse(n)).ToArray();

            List<int[]> allTickets = new();
            List<int> badValue = new();

            for(int i = nextInput+3; i < input.Count(); i++)
            {
                int[] ticket = input[i].Split(",").Select(n => int.Parse(n)).ToArray();
                bool goodTicket = true;

                foreach(int field in ticket)
                {
                    bool good = false;
                    
                    foreach(var rule in rules)
                    {
                        foreach (var range in rule.Value)
                        {
                            if (field <= range.End.Value && field >= range.Start.Value)
                            {
                                good = true;
                                break;
                            }
                        }

                        if (good)
                        {
                            break;
                        }
                    }
                    if (!good)
                    {
                        goodTicket = false;
                        badValue.Add(field);
                        break;
                    }
                }
                if (goodTicket)
                {
                    allTickets.Add(ticket);
                }
            }

            Console.WriteLine($"Part One: {badValue.Sum()}");

            Dictionary<int, List<string>> newTicketOrder = new();
            Enumerable.Range(0,rules.Count()).ToList().ForEach(n => newTicketOrder.Add(n, rules.Keys.ToList()));

            foreach(var ticket in allTickets)
            {
                int index = 0;
                foreach(int field in ticket)
                {
                    foreach(var rule in rules)
                    {
                        bool good = false;
                        foreach (var range in rule.Value)
                        {
                            if (field <= range.End.Value && field >= range.Start.Value)
                            {
                                good = true;
                                break;
                            }
                        }
                        if (!good)
                        {
                            newTicketOrder[index].Remove(rule.Key);
                        }
                    }
                    index++;
                }
            }

            HashSet<string> order = new();
            long total = 1;
            while(order.Count < rules.Count())
            {
                foreach(var kvp in newTicketOrder)
                {
                    if (kvp.Value.Count == 1)
                    {
                        order.Add(kvp.Value.First());
                        if (kvp.Value.First().StartsWith("departure"))
                        {
                            total*= myTicket[kvp.Key];
                        }
                    }
                    foreach (string rule in order)
                    {
                        kvp.Value.Remove(rule);
                    }
                }
            }

            Console.WriteLine($"Part Two: {total}");
        }
    }
}