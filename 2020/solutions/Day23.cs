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
            List<int> order = new List<int>() { 6, 5, 3, 4, 2, 7, 9, 1, 8 };
            //List<int> order = new List<int>() {3,8,9,1,2,5,4,6,7}; // sample
            int currentCup = 0;

            for(int i = 0; i < 100; i++)
            {
                int currentLabel = order[currentCup];
                int label = currentLabel;

                List<int> toRemove = new List<int> { order[(currentCup+1)%9], order[(currentCup+2)%9], order[(currentCup+3)%9]};
                
                foreach (var remove in toRemove)
                {
                    order.Remove(remove);
                }

                int insertAt = -1;

                while (insertAt < 0)
                {
                    label = label-1 <= 0 ? order.Max() : label - 1;

                    if (order.Contains(label))
                    {
                        insertAt = order.IndexOf(label);
                    }
                }

                order.InsertRange(insertAt+1, toRemove);
                currentCup = (order.IndexOf(currentLabel)+1)%order.Count();
            }
            
            List<int> newcurrentList = order.Skip(order.IndexOf(1)+1).ToList();
            newcurrentList.AddRange(order.Take(order.IndexOf(1)));

            Console.WriteLine($"Part One: {string.Join("", newcurrentList)}");

            order = new List<int>() { 6, 5, 3, 4, 2, 7, 9, 1, 8 };
            order.AddRange(Enumerable.Range(10,(1000000-9)));

            Stopwatch sw = new Stopwatch();
            sw.Start();
            currentCup = 0;

            for(int i = 0; i < 10000000; i++)
            {
                int currentLabel = order[currentCup];
                int label = currentLabel;
                if (i%10000 == 0)
                {
                    sw.Stop();
                    Console.WriteLine($"{i}: {currentLabel} {currentCup} {sw.ElapsedMilliseconds}");
                    sw.Restart();
                }
                
                List<int> toRemove = new List<int> { order[(currentCup+1)%order.Count()], order[(currentCup+2)%order.Count()], order[(currentCup+3)%order.Count()]};
                
                if (currentCup+3 < order.Count())
                {
                    order.RemoveRange(currentCup+1, 3);
                }
                else if (currentCup+2 < order.Count())
                {
                    order.RemoveRange(currentCup+1, 2);
                    order.RemoveAt(0);
                }
                else if (currentCup+1 < order.Count())
                {
                    order.RemoveRange(currentCup+1, 1);
                    order.RemoveRange(0, 2);
                }
                else
                {
                    order.RemoveRange(0, 3);
                }

                foreach (var remove in toRemove)
                {
                    order.Remove(remove);
                }

                int insertAt = -1;

                while (insertAt < 0)
                {
                    label = label-1 <= 0 ? order.Max() : label - 1;

                    if (order.Contains(label))
                    {
                        insertAt = order.IndexOf(label);
                    }
                }

                if (insertAt+1 < order.Count())
                    order.InsertRange(insertAt+1, toRemove);
                else
                    order.AddRange(toRemove);

                currentCup = (order.IndexOf(currentLabel)+1)%order.Count();
            }
            
            Console.WriteLine($"Part Two: {order[order.IndexOf(1)+1]} * {order[order.IndexOf(1)+2]} = {order[order.IndexOf(1)+1] * order[order.IndexOf(1)+2]}");
        }
    }
}