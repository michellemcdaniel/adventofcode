using CommandLine;
using AdventOfCode.Cli.Operations;

namespace AdventOfCode.Cli.Options
{
    [Verb("2019", HelpText = "Run problem from 2019.")]
    public class AdventOfCodeNineteenCommandLineOptions : CommandLineOptions
    {
        public override Operation GetOperation()
        {
            return new AdventOfCodeNineteenOperation(this);
        }
    }
}