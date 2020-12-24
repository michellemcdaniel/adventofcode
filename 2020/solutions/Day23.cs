using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace adventofcode
{
    class Day23
    {
        public static void Execute(string filename)
        {
            List<int> order = File.ReadAllText(filename).ToCharArray().Select(i => int.Parse(i.ToString())).ToList();

            Dictionary<int, Cup> cupMap = SetupGame(order); 
            cupMap = PlayGame(100, cupMap, order[0]);
            Console.WriteLine($"Part One: {cupMap[1].Output()}");

            order.AddRange(Enumerable.Range(10,(1000000-9)));
            cupMap = SetupGame(order);
            cupMap = PlayGame(10000000, cupMap, order[0]);
            Console.WriteLine($"Part Two: {cupMap[1].Next.Label} * {cupMap[1].Next.Next.Label} = {(long)cupMap[1].Next.Label * (long)cupMap[1].Next.Next.Label}");
        }

        public static Dictionary<int, Cup> SetupGame(List<int> order)
        {
            Cup previousCup = null;
            Dictionary<int, Cup> cups = new();

            for (int i = 0; i < order.Count(); i++)
            {
                Cup currentCup = new Cup(order[i]);
                if (previousCup != null)
                {
                    previousCup.Next = currentCup;
                }
                cups.Add(order[i], currentCup);
                previousCup = currentCup;
            }

            previousCup.Next = cups.First().Value;
            return cups;
        }

        public static Dictionary<int, Cup> PlayGame(int iterations, Dictionary<int, Cup> map, int first)
        {
            Dictionary<int, Cup> cupMap = new Dictionary<int, Cup>(map);
            Cup currentCupNode = cupMap[first];

            for(int i = 0; i < iterations; i++)
            {
                int currentLabel = currentCupNode.Label;
                int label = currentLabel;
                
                List<Cup> toRemove = new List<Cup>();
                toRemove.Add(currentCupNode.Next);
                toRemove.Add(toRemove.Last().Next);
                toRemove.Add(toRemove.Last().Next);
                currentCupNode.Next = toRemove.Last().Next;

                Cup insertAt = null;
                while (insertAt == null)
                {
                    label = label-1 <= 0 ? cupMap.Keys.Max() : label - 1;
                    if (toRemove.Any(c => c.Label == label))
                    {
                        continue;
                    }

                    insertAt = cupMap[label];
                }

                toRemove.Last().Next = insertAt.Next;
                insertAt.Next = toRemove.First();
                currentCupNode = currentCupNode.Next;
            }

            return cupMap;
        }

        public class Cup
        {
            public int Label {get;set;}
            public Cup Next {get;set;}
            public Cup (int label)
            {
                Label = label;
            }

            public string Output()
            {
                return Next.Output(Label);
            }

            public string Output(int i)
            {
                if (Next.Label != i)
                {
                    return $"{Label}{Next.Output(i)}";
                }
                return Label.ToString();
            }
        }
    }
}