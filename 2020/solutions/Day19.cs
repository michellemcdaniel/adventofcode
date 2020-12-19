using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day19
    {
        public static Dictionary<int, List<string>> matchedRules = new();
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();

            Dictionary<int, string> rules = new();
            List<string> messages = new();

            bool endRules = false;

            foreach (string i in input)
            {
                if (string.IsNullOrEmpty(i))
                {
                    endRules = true;
                    continue;
                }

                if (!endRules)
                {
                    RegexHelper.Match(i, @"(\d+): (.*)", out int index, out string rule);
                    rules.Add(index, rule);
                }
                else
                {
                    messages.Add(i);
                }
            }

            int count = 0;
            List<string> notFirst = new();
            List<string> fourtyTwo = GetRules(rules, 42);
            List<string> thirtyOne = GetRules(rules, 31);

            foreach(string message in messages)
            {
                string newMessage = message;
                bool good = false;

                foreach(string f in fourtyTwo)
                {
                    if (newMessage.StartsWith(f))
                    {
                        newMessage = newMessage.Remove(0, f.Length);
                        good = true;
                        break;
                    }
                }
                if (good)
                {
                    good = false;
                    foreach(string f in fourtyTwo)
                    {
                        if (newMessage.StartsWith(f))
                        {
                            newMessage = newMessage.Remove(0, f.Length);
                            good = true;
                            break;
                        }
                    }
                }
                if (good)
                {
                    good = false;
                    foreach (string f in thirtyOne)
                    {
                        if (newMessage.EndsWith(f))
                        {
                            good = true;
                            newMessage = newMessage.Remove(newMessage.LastIndexOf(f), f.Length);
                            break;
                        }
                    }
                }

                if (good && newMessage.Length == 0)
                {
                    count++;
                }
                else
                {
                    notFirst.Add(message);
                }
            }

            Console.WriteLine($"Part one: {count}");

            foreach (string message in notFirst)
            {
                string newMessage = message;
                bool good = true;
                List<string> front = new();
                List<string> back = new();
                
                while (newMessage.Length > 0 && good)
                {
                    good = false;

                    bool startGood = false;
                    foreach (string f in fourtyTwo)
                    {
                        if (newMessage.StartsWith(f))
                        {
                            front.Add(f);
                            newMessage = newMessage.Remove(0, f.Length);
                            startGood = true;
                            break;
                        }
                    }
                    
                    bool endGood = false;

                    if (startGood && newMessage.Length > 0)
                    {
                        foreach (string f in thirtyOne)
                        {
                            if (newMessage.EndsWith(f))
                            {
                                back.Insert(0,f);
                                newMessage = newMessage.Remove(newMessage.LastIndexOf(f), f.Length);
                                endGood = true;
                                break;
                            }
                        }
                    }

                    if ((startGood && endGood) || (startGood && !endGood && back.Count() > 0))
                    {
                        good = true;
                    }
                }

                if (good && back.Count() > 0 && front.Count() > back.Count())
                {
                    count++;
                }
            }

            
            Console.WriteLine($"Part two: {count}");
        }

        public static List<string> GetRules(Dictionary<int, string> rules, int index)
        {
            string rule = rules[index];

            List<string> subRules = rule.Split(" | ").ToList();

            List<string> finalReturnList = new();

            foreach(string subRule in subRules)
            {
                string[] subSubRules = subRule.Split(" ");
                if (subSubRules.Length == 1 && subSubRules.First().Trim('"') == "a" ||  subSubRules.First().Trim('"') == "b")
                {
                    return new List<string>(){subSubRules.First().Trim('"')};
                }

                List<string> returnList = new List<string>();
                foreach (var subSubRule in subSubRules)
                {
                    int subRuleIndex = int.Parse(subSubRule);
                    List<string> newReturnList = new();
                    List<string> subSubRuleList = new();
                    if (matchedRules.ContainsKey(subRuleIndex))
                    {
                        subSubRuleList = matchedRules[subRuleIndex];
                    }
                    else
                    {
                        subSubRuleList = GetRules(rules, subRuleIndex);
                        matchedRules.Add(subRuleIndex, subSubRuleList);
                    }
                    if (returnList.Count == 0)
                    {
                        returnList = subSubRuleList;
                    }
                    else
                    {
                        for (int i = 0; i < returnList.Count(); i++)
                        {
                            foreach (string s in subSubRuleList)
                            {
                                newReturnList.Add($"{returnList[i]}{s}");
                            }
                        }
                        returnList = newReturnList;
                    }
                }
                finalReturnList.AddRange(returnList);
            }

            return finalReturnList;
        }


    }
}