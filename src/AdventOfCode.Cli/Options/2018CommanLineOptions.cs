using CommandLine;
using AdventOfCode.Cli.Operations;

namespace AdventOfCode.Cli.Options
{
    [Verb("2018", HelpText = "Run problem from 2018.")]
    public class AdventOfCodeEighteenCommandLineOptions : CommandLineOptions
    {
        public override Operation GetOperation()
        {
            return new AdventOfCodeEighteenOperation(this);
        }
    }
}