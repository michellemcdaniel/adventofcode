using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day02
    {
        public static void Execute(string filename)
        {
            List<string> inputPasswords = File.ReadAllLines(filename).ToList();

            int problemOnePasswords = 0;
            int problemTwoPasswords = 0;

            string pattern = @"^(\d+)-(\d+) ([a-z]): ([a-z]+)$";

            foreach (var input in inputPasswords)
            {
                RegexHelper.Match(input, pattern, out int min, out int max, out char letter, out string password);
                int countRequired = RegexHelper.Matches(password, letter.ToString());

                if (countRequired >= min && countRequired <= max)
                {
                    problemOnePasswords++;
                }
                if (password[min-1] == letter ^ password[max-1] == letter)
                {
                    problemTwoPasswords++;
                }
            }

            Console.WriteLine($"Part One: {problemOnePasswords}/{inputPasswords.Count}");
            Console.WriteLine($"Part Two: {problemTwoPasswords}/{inputPasswords.Count}");
        }
    }
}
