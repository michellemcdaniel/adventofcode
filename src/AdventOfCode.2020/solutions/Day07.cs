using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Twenty
{
    public class Day07
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();
            
            List<Bag> bags = new List<Bag>();

            string pattern = @"^(.*) bags contain (.*)\.$";
            foreach(string rule in input)
            {
                bool success = RegexHelper.Match(rule, pattern, out string color, out string contentString);

                if (success)
                {
                    List<string> contents = contentString.Split(", ").ToList();

                    Dictionary<string, int> newContents = new Dictionary<string, int>();

                    foreach (string content in contents)
                    {
                        string contentPattern = @"^(\d+) (.*) bags?$";
                        bool contentSuccess = RegexHelper.Match(content, contentPattern, out int count, out string contentColor);
                        if (contentSuccess)
                        {
                            newContents.Add(contentColor.Trim(), count);
                        }
                    }

                    bags.Add(new Bag(color, newContents));
                }
            }

            Queue<string> colors = new Queue<string>();
            colors.Enqueue("shiny gold");
            HashSet<string> foundColors = new HashSet<string>();

            while (colors.Any())
            {
                string color = colors.Dequeue();
                foreach(Bag bag in bags)
                {
                    if (bag.Contents.Keys.Contains(color))
                    {
                        foundColors.Add(bag.Color);
                        colors.Enqueue(bag.Color);
                    }
                }
            }

            Queue<(string, int)> bagsInGold = new Queue<(string, int)>();
            bagsInGold.Enqueue(("shiny gold", 1));

            int totalBags = 0;

            while (bagsInGold.Any())
            {
                (string color, int count) = bagsInGold.Dequeue();
                Bag bag = bags.First(b => b.Color == color);
                foreach (var innerBag in bag.Contents)
                {
                    totalBags += count*innerBag.Value;
                    bagsInGold.Enqueue((innerBag.Key, count*innerBag.Value));
                }
            }

            Console.WriteLine($"Part One: {foundColors.Count()}");   
            Console.WriteLine($"Part Two: {totalBags}");
        }

        class Bag
        {
            public string Color { get; set; }
            public Dictionary<string, int> Contents { get; }

            public Bag(string color, Dictionary<string, int> contents)
            {
                Color = color;
                Contents = contents;
            }
        }
    }
}