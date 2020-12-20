using System;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("You must supply the number of the day to run.");
                return;
            }
            
            int day = Convert.ToInt32(args[0]);
            string sample = "";

            if (args.Contains("sample"))
            {
                sample = "-sample";
            }

            string filename = Path.Combine(Environment.CurrentDirectory, "input", $"day{day.ToString("D2")}{sample}.txt");

            switch (day)
            {
                case 1:
                    Day01.Execute(filename);
                    break;
                case 2:
                    Day02.Execute(filename);
                    break;
                case 3:
                    Day03.Execute(filename);
                    break;
                case 4:
                    Day04.Execute(filename);
                    break;
                case 5:
                    Day05.Execute(filename);
                    break;
                case 6:
                    Day06.Execute(filename);
                    break;
                case 7:
                    Day07.Execute(filename);
                    break;
                case 8:
                    Day08.Execute(filename);
                    break;
                case 9:
                    Day09.Execute(filename);
                    break;
                case 10:
                    Day10.Execute(filename);
                    break;
                case 11:
                    Day11.Execute(filename);
                    break;
                case 12:
                    Day12.Execute(filename);
                    break;
                case 13:
                    Day13.Execute(filename);
                    break;
                case 14:
                    Day14.Execute(filename);
                    break;
                case 15:
                    Day15.Execute(filename);
                    break;
                case 16:
                    Day16.Execute(filename);
                    break;
                case 17:
                    Day17.Execute(filename);
                    break;
                case 18:
                    Day18.Execute(filename);
                    break;
                case 19:
                    Day19.Execute(filename);
                    break;
                case 20:
                    Day20.Execute(filename);
                    break;
                default:
                    Console.WriteLine($"Day {day} does not have a corresponding puzzle.");
                    break;
            }
        }
    }
}
