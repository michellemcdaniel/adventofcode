using AdventOfCode.Cli.Options;
using AdventOfCode.Nineteen;
using System;
using System.IO;
using System.Linq;

namespace AdventOfCode.Cli.Operations
{
    public class AdventOfCodeNineteenOperation : Operation
    {
        private readonly AdventOfCodeNineteenCommandLineOptions _options;

        public AdventOfCodeNineteenOperation(AdventOfCodeNineteenCommandLineOptions options)
        {
            _options = options;
        }

        public override void Execute()
        {
            string sample = "";
            if (_options.Sample)
            {
                sample = "-sample";
            }

            string filename = Path.Combine(Environment.CurrentDirectory, "..", "AdventOfCode.2019", "input", $"day{_options.Day.ToString("D2")}{sample}.txt");

            switch (_options.Day)
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
                case 21:
                    Day21.Execute(filename);
                    break;
                case 22:
                    Day22.Execute(filename);
                    break;
                case 23:
                    Day23.Execute(filename);
                    break;
                case 24:
                case 25:
                    throw new NotImplementedException($"Day {_options.Day} is not implemented yet.");
                default:
                    throw new ArgumentException($"Day {_options.Day} is outside the scope of Advent of Code.", "--day");
            }
        }
    }
}