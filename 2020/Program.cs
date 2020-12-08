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
                default:
                    Console.WriteLine($"Day {day} does not have a corresponding puzzle.");
                    break;
            }
        }
    }
}
