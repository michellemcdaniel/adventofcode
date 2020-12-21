using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace adventofcode
{
    class Day21
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();
            Dictionary<string, List<string>> possibleAllergens = new();
            List<string> allIngredients = new();

            foreach(string i in input)
            {
                RegexHelper.Match(i, @"(.*) \(contains (.*)\)", out string ingredients, out string allergens);
                allIngredients.AddRange(ingredients.Split(" "));

                foreach (string allergen in allergens.Split(", "))
                {
                    if (possibleAllergens.TryGetValue(allergen, out List<string> current))
                    {
                        possibleAllergens[allergen] = current.Intersect(ingredients.Split(" ")).ToList();
                    }
                    else
                    {
                        possibleAllergens.Add(allergen, ingredients.Split(" ").ToList());
                    }
                }
            }

            while (possibleAllergens.Any(a => a.Value.Count() > 1))
            {
                foreach(var kvp in possibleAllergens.Where(a => a.Value.Count() == 1))
                {
                    foreach (var kvp2 in possibleAllergens.Where(a => a.Key != kvp.Key))
                    {
                        kvp2.Value.Remove(kvp.Value.First());
                    }
                }
            }
            
            Console.WriteLine($"Part One: {allIngredients.Where(i => !possibleAllergens.Values.SelectMany(x => x).Contains(i)).Count()}");
            Console.WriteLine($"Part Two: {string.Join(",", possibleAllergens.OrderBy(a => a.Key).Select(a => a.Value.First()))}");
        }
    }
}