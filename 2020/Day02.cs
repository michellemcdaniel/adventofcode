using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace adventofcode
{
    class Day02
    {
        public static void Execute()
        {
            List<string> inputPasswords = File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "input", "day02.txt")).ToList();

            int problemOnePasswords = 0;
            int problemTwoPasswords = 0;

            string pattern = @"^(?<min>\d+)-(?<max>\d+) (?<letter>[a-z]): (?<password>[a-z]+)$";

            foreach (var input in inputPasswords)
            {
                Match match = Regex.Match(input, pattern);

                string password = match.Groups["password"].Value;
                char letter = match.Groups["letter"].Value[0];

                int min = Int32.Parse(match.Groups["min"].Value);
                int max = Int32.Parse(match.Groups["max"].Value);

                int countRequired = Regex.Matches(password, letter.ToString()).Count;

                if (countRequired >= min && countRequired <= max)
                {
                    problemOnePasswords++;
                }

                if (password[min-1] == letter ^ password[max-1] == letter)
                {
                    problemTwoPasswords++;
                }
            }

            Console.WriteLine($"{problemOnePasswords}/{inputPasswords.Count}");
            Console.WriteLine($"{problemTwoPasswords}/{inputPasswords.Count}");
        }
    }
}
