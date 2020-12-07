using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventofcode
{
    class Day07
    {
        public static void Execute()
        {
            List<string> input = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "input", "day07.txt")).ToList();
            
            List<Bag> bags = new List<Bag>();

            string pattern = @"^(?<color>.*) bags contain (?<contents>.*)\.?$";
            foreach(string rule in input)
            {
                Match match = Regex.Match(rule, pattern);

                if (match.Success)
                {
                    string color = match.Groups["color"].Value;
                    List<string> contents = match.Groups["contents"].Value.Split(", ").ToList();

                    Dictionary<string, int> newContents = new Dictionary<string, int>();

                    foreach (string content in contents)
                    {
                        string contentPattern = @"^(?<count>\d+) (?<content>.*) bag";
                        Match contentMatch = Regex.Match(content, contentPattern);
                        if (contentMatch.Success)
                        {
                            newContents.Add(contentMatch.Groups["content"].Value.Trim(), int.Parse(contentMatch.Groups["count"].Value));
                        }
                    }

                    bags.Add(new Bag(color, newContents));
                }
            }

            Queue<string> colors = new Queue<string>();
            colors.Enqueue("shiny gold");
            HashSet<string> foundColors = new HashSet<string>();

            while (colors.Count() > 0)
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

            while (bagsInGold.Count() > 0)
            {
                (string color, int count) = bagsInGold.Dequeue();
                Bag bag = bags.First(b => b.Color == color);
                foreach (var innerBag in bag.Contents)
                {
                    totalBags += count*innerBag.Value;
                    bagsInGold.Enqueue((innerBag.Key, count*innerBag.Value));
                }
            }

            Console.WriteLine($"Total bags that can contain shiny gold: {foundColors.Count()}");   
            Console.WriteLine($"Total bags in a shiny gold: {totalBags}");
        }
    }

    public class Bag
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