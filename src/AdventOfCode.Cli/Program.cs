using AdventOfCode.Cli.Options;
using AdventOfCode.Cli.Operations;
using CommandLine;
using System;

namespace AdventOfCode.Cli
{
    class Program
    {
        static int Main(string[] args)
        {
            return Parser.Default.ParseArguments(args, GetOptions())
                .MapResult( (CommandLineOptions opts) => RunOperation(opts),
                    (errs => 1));
        }

        /// <summary>
        /// Runs the operation and calls dispose afterwards, returning the operation exit code.
        /// </summary>
        /// <param name="operation">Operation to run</param>
        /// <returns>Exit code of the operation</returns>
        /// <remarks>The primary reason for this is a workaround for an issue in the logging factory which
        /// causes it to not dispose the logging providers on process exit.  This causes missed logs, logs that end midway through
        /// and cause issues with the console coloring, etc.</remarks>
        private static int RunOperation(CommandLineOptions opts)
        {
            try
            {
                Operation operation = opts.GetOperation();

                operation.Execute();
                return 0;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Unhandled exception while running {typeof(Operation).Name}");
                Console.WriteLine(e);
                return -1;
            }
        }

        private static Type[] GetOptions()
        {
            // This order will mandate the order in which the commands are displayed if typing just 'dn-rel'
            // so keep these sorted.
            return new Type[]
                {
                    typeof(AdventOfCodeEighteenCommandLineOptions),
                    typeof(AdventOfCodeNineteenCommandLineOptions),
                    typeof(AdventOfCodeTwentyCommandLineOptions)
                };
        }
    }
}
