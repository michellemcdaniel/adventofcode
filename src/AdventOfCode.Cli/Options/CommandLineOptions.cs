using System;
using CommandLine;
using AdventOfCode.Cli.Operations;

namespace AdventOfCode.Cli.Options
{
    public abstract class CommandLineOptions
    {
        [Option("verbose", HelpText = "Turn on verbose output.")]
        public bool Verbose { get; set; }

        [Option("debug", HelpText = "Turn on debug output.")]
        public bool Debug { get; set; }

        [Option("day", HelpText = "The day whose problem you want to run.")]
        public int Day { get; set; }

        [Option("sample", HelpText = "Run on the sample input.")]
        public bool Sample { get; set; }

        public abstract Operation GetOperation();
    }
}
