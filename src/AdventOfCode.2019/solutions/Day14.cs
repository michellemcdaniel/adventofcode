using AdventOfCode.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Nineteen
{
    public class Day14
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();
            Dictionary<string, Chemical> reactions = new();

            foreach (var i in input)
            {
                RegexHelper.Match(i, @"(.*) => (\d+) ([A-Z]+)", out string inputChemicals, out int quantity, out string name);
                Chemical newChemical = new Chemical(name, quantity);

                foreach (var chem in inputChemicals.Split(", "))
                {
                    RegexHelper.Match(chem, @"(\d+) ([A-Z]+)", out int inputQuantity, out string inputName);
                    newChemical.AddInputChemical(inputName, inputQuantity);
                }
                reactions.Add(name, newChemical);
            }

            reactions.Add("ORE", new Chemical("ORE", 0));

            foreach(var chem in reactions)
            {
                foreach(var child in chem.Value.InputChemicals)
                {
                    ChemicalEdge edge = new ChemicalEdge(chem.Value, reactions[child.Key], child.Value);
                    chem.Value.ChildEdges.Add(edge);
                    reactions[child.Key].ParentEdges.Add(edge);
                }
            }

            Chemical fuel = reactions["FUEL"];
            double oreTotal = DFS(fuel, 1, reactions);
            Console.WriteLine($"Part One: {oreTotal}");

            // Do a binary search.
            long targetOre = 1_000_000_000_000;
            long min = 1_000;
            long max = 1_500_000;
            long currentFuel = 0;

            while (min < max)
            {
                currentFuel = (min+max)/2;
                double result = DFS(fuel, currentFuel, reactions);
                Console.WriteLine($"Current total ore used {currentFuel} {result}");
                if (result > targetOre)
                {
                    max = currentFuel;
                }
                else
                {
                    if (min == currentFuel || result == targetOre) break;
                    min = currentFuel;
                }
            }

            Console.WriteLine($"Part Two: {currentFuel}");
        }

        public static double DFS(Chemical start, long fuelQuantity, Dictionary<string, Chemical> reactions)
        {
            Queue<Chemical> process = new();
            process.Enqueue(start);
            Dictionary<string, double> requiredQuantites = new();
            HashSet<string> processedNodes = new();
            HashSet<string> processedEdges = new();

            while (process.Any())
            {
                Chemical chemical = process.Dequeue();
                
                if (chemical.Processed)
                {
                    continue;
                }

                if (!requiredQuantites.ContainsKey(chemical.Name))
                {
                    requiredQuantites[chemical.Name] = 0;
                }
                if (chemical.Name == "FUEL")
                {
                    requiredQuantites[chemical.Name] = fuelQuantity;
                }

                bool complete = true;
                
                foreach(var edge in chemical.ParentEdges)
                {
                    if (processedEdges.Contains($"{chemical.Name}+{edge.Parent.Name}")) continue; // Already processed the edge
                    if (processedNodes.Contains($"{edge.Parent.Name}"))
                    {
                        processedEdges.Add($"{chemical.Name}+{edge.Parent.Name}");
                        requiredQuantites[chemical.Name] += edge.RequiredQuantity*
                            Math.Ceiling(requiredQuantites[edge.Parent.Name]/edge.Parent.CreatedQuantity);
                    }
                    else complete = false; // Edge isn't ready for processing yet because the parent node hasn't been processed
                }

                if (!complete)
                {
                    process.Enqueue(chemical);
                }
                else
                {
                    processedNodes.Add($"{chemical.Name}");
                    foreach (var edge in chemical.ChildEdges)
                    {
                        process.Enqueue(edge.Child);
                    }
                }
            }

            return requiredQuantites["ORE"];
        }

        public class Chemical
        {
            public string Name { get; set; }
            public int CreatedQuantity { get; set; }
            public Dictionary<string, int> InputChemicals { get; set; }

            public List<ChemicalEdge> ChildEdges { get; set; }
            public List<ChemicalEdge> ParentEdges { get; set; }
            public bool Processed { get; set; }

            public Chemical(string name, int quantity)
            {
                Name = name;
                CreatedQuantity = quantity;
                InputChemicals = new();
                ChildEdges = new();
                ParentEdges = new();
            }

            public void AddInputChemical(string name, int quantity)
            {
                InputChemicals.Add(name, quantity);
            }
        }

        public class ChemicalEdge
        {
            public Chemical Parent { get; set; }
            public Chemical Child { get; set; }
            public long RequiredQuantity { get; set; }
            public bool Processed { get; set; }

            public ChemicalEdge(Chemical parent, Chemical child, long quantity)
            {
                Parent = parent;
                Child = child;
                RequiredQuantity = quantity;
            }
        }
    }
}
