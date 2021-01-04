using AdventOfCode.Cli.Options;
using AdventOfCode.Eighteen;
using System;
using System.IO;
using System.Linq;

namespace AdventOfCode.Cli.Operations
{
    public class AdventOfCodeEighteenOperation : Operation
    {
        private readonly AdventOfCodeEighteenCommandLineOptions _options;

        public AdventOfCodeEighteenOperation(AdventOfCodeEighteenCommandLineOptions options)
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

            string filename = Path.Combine(Environment.CurrentDirectory, "..", "AdventOfCode.2018", "input", $"day{_options.Day.ToString("D2")}{sample}.txt");

            switch (_options.Day)
            {
                case 1:
                    Day01.Execute(filename);
                    break;
                case 2:
                    Day02.Execute(filename);
                    break;
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                    throw new ArgumentException($"Day {_options.Day} has not been implemented yet", "--day");
                default:
                    throw new ArgumentException($"Day {_options.Day} is outside the scope of Advent of Code.", "--day");
            }
        }
    }
}