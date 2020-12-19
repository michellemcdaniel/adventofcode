using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace adventofcode
{
    class Day19
    {
        public static Dictionary<int, List<string>> matchedRules = new();
        public static void Execute(string filename)
        {
            Queue<string> input = new Queue<string>(File.ReadAllLines(filename));

            Dictionary<int, string> rules = new();
            List<string> messages = input.Where(i => !i.Contains(":")).ToList();

            while(!string.IsNullOrEmpty(input.Peek()))
            {
                RegexHelper.Match(input.Dequeue(), @"(\d+): (.*)", out int index, out string rule);
                rules.Add(index, rule.Replace("\"",""));
            }

            List<List<string>> reducedZero = Reduce(rules[0], rules);
            List<string> notFirst = new();
            int count = 0;
            int maxLength = reducedZero.Select(r => r.Select(s => s.Length).Max()).Sum();

            foreach(string message in messages)
            {
                if (Matches(message, reducedZero, maxLength)) count++;
                else notFirst.Add(message);
            }

            Console.WriteLine($"Part one: {count}");

            rules[8] = "42 | 42 8";
            rules[11] = "42 31 | 42 11 31";
            List<List<List<string>>> recursiveReduced = ReduceWithRecursion(rules[0], rules, 12);

            foreach (string message in notFirst)
            {
                foreach (var patternList in recursiveReduced)
                {
                    int patternLength = patternList.Select(r => r.Select(s => s.Length).Max()).Sum();

                    if (Matches(message, patternList, patternLength))
                    {
                        count++;
                        break;
                    }
                }
            }

            Console.WriteLine($"Part two: {count}");
        }

        public static bool Matches(string message, List<List<string>> patterns, int maxLength)
        {
            List<string> matches = new List<string>();

            if (message.Length != maxLength)
            {
                return false;
            }
            
            int offset = 0;
            bool good = false;

            for (int i = 0; i < patterns.Count(); i++)
            {
                good = false;
                foreach (var pattern in patterns[i])
                {
                    if (message.Substring(i*offset, pattern.Length) == pattern)
                    {
                        good = true;
                        offset = pattern.Length;
                        break;
                    }
                }

                if (!good) break;
            }

            return good;
        }

        public static List<List<string>> Reduce(string rule, Dictionary<int, string> rules)
        {
            return GetReducedString(rule, rules).Split(" ").Select(token => GetRules(rules, int.Parse(token))).ToList();
        }

        public static string GetReducedString(string rule, Dictionary<int, string> rules)
        {
            string reducedRule = rule;
            string next = rule;

            while (!next.Contains("|"))
            {
                reducedRule = next;
                foreach(var token in next.Split(" "))
                {
                    next = next.Replace(token, rules[int.Parse(token)]);
                }
            }

            return reducedRule;
        }

        public static List<List<List<string>>> ReduceWithRecursion(string rule, Dictionary<int, string> rules, int maxTokens)
        {
            List<List<string>> tokens = new();
            List<int> loopValues = new();

            foreach (var token in GetReducedString(rule, rules).Split(" "))
            {
                int tokenVal = int.Parse(token);
                string tokenRule = rules[tokenVal];

                if (tokenRule.Split(" ").Any(t => t == token))
                {
                    loopValues.Add(tokenVal);
                    tokens.Add(tokenRule.Split(" | ").ToList());
                }
            }

            List<string> newRules = new();
            List<string> rulesWithLoops = tokens.First()
                .SelectMany(a => tokens.Last().Select(b => $"{a} {b}"))
                .Where(r => r.Split(" ").Any(t => loopValues.Any(v => v.ToString() == t))).ToList();

            while (rulesWithLoops.Any())
            {
                List<string> newRulesWithLoops = new();
                foreach(var r in rulesWithLoops)
                {
                    string newRule = r;
                    for (int i = 0; i < loopValues.Count(); i++)
                    {
                        int replaceSize = tokens[i].First().Split(" ").Count();
                        if (newRule.Contains(loopValues[i].ToString()))
                        {
                            while(newRule.Split(" ").Count() + replaceSize -1 <= maxTokens)
                            {
                                newRulesWithLoops.Add(newRule.Replace(loopValues[i].ToString(), tokens[i].First()));
                                newRule = newRule.Replace(loopValues[i].ToString(), tokens[i].Last());
                            }
                        }
                    }
                }

                newRules.AddRange(newRulesWithLoops.Where(r => !r.Split(" ").Any(t => loopValues.Any(v => v.ToString() == t))));
                rulesWithLoops = newRulesWithLoops.Except(newRules).ToList();
            }

            List<List<List<string>>> reduced = newRules.Distinct().Select(r => Reduce(r, rules)).ToList();

            return reduced;
        }

        public static List<string> GetRules(Dictionary<int, string> rules, int index)
        {
            string rule = rules[index];
            List<string> subRules = rule.Split(" | ").ToList();
            List<string> finalReturnList = new();

            foreach(string subRule in subRules)
            {
                if (subRule.Length == 1 && subRule == "a" || subRule == "b")
                {
                    return new List<string>() { subRule };
                }

                string[] tokens = subRule.Split(" ");
                List<string> returnList = new List<string>() { "" };
                
                foreach (var token in tokens)
                {
                    int tokenValue = int.Parse(token);
            
                    if (!matchedRules.TryGetValue(tokenValue, out List<string> tokenRuleList))
                    {
                        tokenRuleList = GetRules(rules, tokenValue);
                        matchedRules.Add(tokenValue, tokenRuleList);
                    }

                    returnList = returnList.SelectMany(a => tokenRuleList.Select(b => $"{a}{b}")).ToList();
                }
                finalReturnList.AddRange(returnList);
            }

            return finalReturnList.Distinct().ToList();
        }
    }
}