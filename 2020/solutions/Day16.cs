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
            
            // Rules
            for (int i = 0; i < input.Count; i++)
            {
                nextInput = i+1;
                if (string.IsNullOrEmpty(input[i]))
                {
                    break;
                }

                RegexHelper.Match(input[i], @"(.*): (.*) or (.*)$", out string field, out string rangeOne, out string rangeTwo);
                rules.Add(field, (rangeOne, rangeTwo));
            }

            nextInput++;
            // My ticket
            int[] myTicket = input[nextInput].Split(",").Select(n => int.Parse(n)).ToArray();

            List<int[]> allTickets = new ();
            List<int> badValue = new();

            // Example tickets
            for(int i = nextInput+3; i < input.Count(); i++)
            {
                int[] ticket = input[i].Split(",").Select(n => int.Parse(n)).ToArray();
                bool goodTicket = true;

                foreach(int field in ticket)
                {
                    bool good = false;
                    
                    foreach(var rule in rules)
                    {
                        (string rangeOne, string rangeTwo) = rule.Value;

                        RegexHelper.Match(rangeOne, @"(\d+)-(\d+)", out int low, out int high);
                        RegexHelper.Match(rangeTwo, @"(\d+)-(\d+)", out int low2, out int high2);

                        if (((field <= high && field >= low) || (field <= high2 && field >= low2)))
                        {
                            good = true;
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

            Dictionary<int, List<string>> newTicketOrder = new();
            Enumerable.Range(0,rules.Count()).ToList().ForEach(n => newTicketOrder.Add(n, rules.Keys.ToList()));

            foreach(var ticket in allTickets)
            {
                int index = 0;
                foreach(int field in ticket)
                {
                    foreach(var rule in rules)
                    {
                        (string rangeOne, string rangeTwo) = rule.Value;

                        RegexHelper.Match(rangeOne, @"(\d+)-(\d+)", out int low, out int high);
                        RegexHelper.Match(rangeTwo, @"(\d+)-(\d+)", out int low2, out int high2);

                        if (!((field <= high && field >= low) || (field <= high2 && field >= low2)))
                        {
                            newTicketOrder[index].Remove(rule.Key);
                        }
                    }
                    index++;
                }
            }

            Console.WriteLine($"Part one: {badValue.Sum()}");

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

            Console.WriteLine($"Part two: {total}");
        }
    }
}