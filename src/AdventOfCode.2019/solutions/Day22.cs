using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;

namespace AdventOfCode.Nineteen
{
    public class Day22
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();

            int deckSize = 10007;
            List<Rule> initialRules = ParseRules(input);
            List<Rule> reducedRules = ReduceRules(initialRules, deckSize);
            long value = GetCard(reducedRules, deckSize, 2019);

            Console.WriteLine($"Part One: {value}");

            List<Rule> iterationRules = new List<Rule>(initialRules);
            reducedRules = new List<Rule>();

            long iterations = 101741582076661;
            long count = 119315717514047;

            // The card at location x on iteration y is the same as the position of card with value x on iteration (count-iterations-1)
            // So reduce the rules to find the rules for iteration (count-iterations-1), and then find card x.
            for (long i = (count-iterations-1); i > 0; i/=2)
            {
                if (i%2 == 1)
                {
                    reducedRules.AddRange(iterationRules);
                    reducedRules = ReduceRules(reducedRules, count);
                }
                iterationRules.AddRange(iterationRules);
                iterationRules = ReduceRules(iterationRules, count);
            }

            long position = GetCard(reducedRules, count, 2020);
            Console.WriteLine($"Part Two: {position}");
        }

        public static long GetCard(List<Rule> rules, long count, long value)
        {
            long output = value;

            foreach(var rule in rules)
            {
                switch (rule.Type)
                {
                    case RuleType.Cut:
                        if (output < rule.Value)
                        {
                            output += (count-rule.Value);
                        }
                        else
                        {
                            output -= rule.Value;
                            output %= count;
                        }
                        break;
                    case RuleType.DealWithInc:
                        BigInteger cur = output*new BigInteger(rule.Value);
                        cur = cur%count;
                        output = (long)cur;
                        break;
                    case RuleType.Deal:
                        output = (count-1) - output;
                        break;
                }
            }

            return output;
        }

        public static List<Rule> ParseRules(List<string> input)
        {
            List<Rule> rules = new List<Rule>();
            foreach(var inst in input)
            {
                if (inst.StartsWith("cut"))
                {
                    rules.Add(new Rule(RuleType.Cut, int.Parse(inst.Split(" ").Last())));
                }
                else if (inst == "deal into new stack") // deal
                {
                    rules.Add(new Rule(RuleType.Deal, 0));
                }
                else if (inst.StartsWith("deal with increment"))
                {
                    rules.Add(new Rule(RuleType.DealWithInc, int.Parse(inst.Split(" ").Last())));
                }
            }
            return rules;
        }

        public static List<Rule> ReduceRules(List<Rule> input, long deckSize)
        {
            List<Rule> rules = new List<Rule>(input);
            bool changed = true;
            while(changed)
            {
                changed = false;
                // Reduce rules.
                List<Rule> reduced = new();
                for(int i = 0; i < rules.Count; i++)
                {
                    if (i == rules.Count-1)
                    {
                        reduced.Add(rules[i]);
                    }
                    else
                    {
                        switch(rules[i].Type)
                        {
                            case RuleType.Cut:
                                if (rules[i].Value == 0)
                                {
                                    continue;
                                }
                                
                                i++;
                                changed = true;

                                switch(rules[i+1].Type)
                                {
                                    case RuleType.Cut: // combine the two cuts.
                                        reduced.Add(new Rule (RuleType.Cut, (rules[i].Value+rules[i+1].Value)%deckSize));
                                        break;
                                    case RuleType.Deal: // swap them, and invert the cut amount
                                        reduced.Add(rules[i+1]);
                                        reduced.Add(new Rule(RuleType.Cut, -1*rules[i].Value));
                                        break;
                                    case RuleType.DealWithInc: // swap them, and change the cut amount
                                        reduced.Add(rules[i+1]);
                                        reduced.Add(new Rule(RuleType.Cut, (long)((new BigInteger(rules[i].Value)*new BigInteger(rules[i+1].Value))%deckSize)));
                                        break;
                                }
                                break;
                            case RuleType.Deal:
                                switch(rules[i+1].Type)
                                {
                                    case RuleType.DealWithInc: // Becomes 3 instructions: deal with inc, cut, deal
                                        reduced.Add(rules[i+1]);
                                        reduced.Add(new Rule(RuleType.Cut, -1*rules[i+1].Value+1));
                                        reduced.Add(rules[i]);
                                        i++;
                                        changed = true;
                                        break;
                                    case RuleType.Deal: // Cancel each other out, remove both.
                                        i++;
                                        changed = true;
                                        break;
                                    default:
                                        reduced.Add(rules[i]); // leave everything else alone
                                        break;
                                }
                                break;
                            case RuleType.DealWithInc:
                                if (rules[i].Value == 1)
                                {
                                    continue;
                                }
                                switch(rules[i+1].Type)
                                {
                                    case RuleType.DealWithInc: // combine them
                                        reduced.Add(new Rule(RuleType.DealWithInc, (long)((new BigInteger(rules[i].Value)*new BigInteger(rules[i+1].Value))%deckSize)));
                                        i++;
                                        changed = true;
                                        break;
                                    default: // leave everything else alone.
                                        reduced.Add(rules[i]);
                                        break;
                                }
                                break;
                        }
                    }
                }

                rules = reduced;
            }

            return rules;
        }

        public enum RuleType
        {
            Cut,
            DealWithInc,
            Deal
        }

        public class Rule
        {
            public RuleType Type { get; set; }
            public long Value { get; set; }

            public Rule(RuleType type, long val)
            {
                Type = type;
                Value = val;
            }
        }
    }
}
