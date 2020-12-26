using CommandLine;
using AdventOfCode.Cli.Operations;

namespace AdventOfCode.Cli.Options
{
    [Verb("2020", HelpText = "Run problem from 2020.")]
    public class AdventOfCodeTwentyCommandLineOptions : CommandLineOptions
    {
        public override Operation GetOperation()
        {
            return new AdventOfCodeTwentyOperation(this);
        }
    }
}