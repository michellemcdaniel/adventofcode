using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day16
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();

            Dictionary<string, (string, string)> rules = new();
            int nextInput = 0;
            
            for (int i = 0; i < input.Count; i++)
            {
                nextInput = i+1;
                if (string.IsNullOrEmpty(input[i]))
                {
                    // Last of rules
                    break;
                }

                RegexHelper.Match(input[i], @"(.*): (.*) or (.*)$", out string field, out string rangeOne, out string rangeTwo);
                rules.Add(field, (rangeOne, rangeTwo));
            }

            nextInput++;
            int[] myTicket = input[nextInput].Split(",").Select(n => int.Parse(n)).ToArray();

            List<int[]> allTickets = new ();

            List<int> badValue = new();

            Dictionary<int, Dictionary<string, int>> ticketOrder = new();

            for(int i = nextInput+3; i < input.Count(); i++)
            {
                int[] ticket = input[i].Split(",").Select(n => int.Parse(n)).ToArray();

                bool goodTicket = true;
                int index = 0;

                foreach(int field in ticket)
                {
                    bool good = false;
                    
                    foreach(var rule in rules)
                    {
                        (string rangeOne, string rangeTwo) = rule.Value;

                        RegexHelper.Match(rangeOne, @"(\d+)-(\d+)", out int low, out int high);

                        if (field <= high && field >= low)
                        {
                            good = true;
                        }
                        RegexHelper.Match(rangeTwo, @"(\d+)-(\d+)", out low, out high);
                        if (field <= high && field >= low)
                        {
                            good = true;
                        }
                    }
                    index++;
                    if (!good)
                    {
                        goodTicket = false;
                        badValue.Add(field);
                    }
                }
                if (goodTicket)
                {
                    allTickets.Add(ticket);
                }
            }

            foreach(var ticket in allTickets)
            {
                int index = 0;
                foreach(int field in ticket)
                {
                    if (!ticketOrder.ContainsKey(index))
                    {
                        ticketOrder[index] = new();
                    }
                    
                    foreach(var rule in rules)
                    {
                        if (!ticketOrder[index].ContainsKey(rule.Key))
                        {
                            ticketOrder[index].Add(rule.Key, 0);
                        }
                        (string rangeOne, string rangeTwo) = rule.Value;

                        RegexHelper.Match(rangeOne, @"(\d+)-(\d+)", out int low, out int high);

                        if (field <= high && field >= low)
                        {
                            ticketOrder[index][rule.Key]++;
                        }
                        RegexHelper.Match(rangeTwo, @"(\d+)-(\d+)", out low, out high);
                        if (field <= high && field >= low)
                        {
                            ticketOrder[index][rule.Key]++;
                        }
                    }
                    index++;
                }
            }

            Console.WriteLine($"Part one: {badValue.Sum()}");

            Dictionary<int, string> order = new();
            while(order.Count < rules.Count())
            {
                int index2 = 0;
                foreach(var kvp in ticketOrder)
                {
                    foreach(var rule in rules)
                    {
                        if (order.ContainsValue(rule.Key))
                        {
                            kvp.Value.Remove(rule.Key);
                            continue;
                        }
                        else if (kvp.Value.ContainsKey(rule.Key))
                        {
                            int count = kvp.Value[rule.Key];
                            if (count < allTickets.Count())
                            {
                                kvp.Value.Remove(rule.Key);
                            }
                        }
                    }

                    if (kvp.Value.Count() == 1)
                    {
                        order[index2] = kvp.Value.First().Key;
                    }
                    index2++;
                }
            }

            long total = 1;
            foreach(var what in order)
            {
                if (what.Value.StartsWith("departure"))
                {
                    total*=myTicket[what.Key];
                }
            }

            Console.WriteLine($"Part two: {total}");
        }
    }
}