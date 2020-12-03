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

            foreach (var input in inputPasswords)
            {
                string[] ruleToPassword = input.Split(": ");
                string[] rule = ruleToPassword[0].Split(" ");
                string[] minMax = rule[0].Split("-");

                string password = ruleToPassword[1];
                char letter = rule[1].ToCharArray()[0];

                int min = Int32.Parse(minMax[0]);
                int max = Int32.Parse(minMax[1]);

                int countRequired = Regex.Matches(password, rule[1]).Count;

                if (countRequired >= min && countRequired <= max)
                {
                    problemOnePasswords++;
                }

                char[] passwordAsCharArray = password.ToCharArray();
                if (passwordAsCharArray[min-1] == letter ^ passwordAsCharArray[max-1] == letter)
                {
                    problemTwoPasswords++;
                }
            }

            Console.WriteLine($"{problemOnePasswords}/{inputPasswords.Count}");
            Console.WriteLine($"{problemTwoPasswords}/{inputPasswords.Count}");
        }
    }
}
