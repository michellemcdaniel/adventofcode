using CommandLine;
using AdventOfCode.Cli.Operations;

namespace AdventOfCode.Cli.Options
{
    [Verb("2022", HelpText = "Run problem from 2022.")]
    public class AdventOfCodeTwentyTwoCommandLineOptions : CommandLineOptions
    {
        public override Operation GetOperation()
        {
            return new AdventOfCodeTwentyTwoOperation(this);
        }
    }
}