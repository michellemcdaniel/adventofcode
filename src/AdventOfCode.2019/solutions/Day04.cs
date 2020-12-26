using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Nineteen
{
    public class Day04
    {
        public static void Execute(string filename)
        {
            int inputMin = 134792;
            int inputMax = 675810;

            List<int> goodPasswords = new List<int>();
            string pattern = @"^(?=\d{6}$)1*2*3*4*5*6*7*8*9*$";
            string duplicateMiddle = "[^1]11[^1]|[^2]22[^2]|[^3]33[^3]|[^4]44[^4]|[^5]55[^5]|[^6]66[^6]|[^7]77[^7]|[^8]88[^8]|[^9]99[^9]";
            string duplicateStart = "^(11[^1]|22[^2]|33[^3]|44[^4]|55[^5]|66[^6]|77[^7]|88[^8]|99[^9])";
            string duplicateEnd = "([^1]11|[^2]22|[^3]33|[^4]44|[^5]55|[^6]66|[^7]77|[^8]88|[^9]99)$";

            for (int i = inputMin; i < inputMax; i++)
            {
                string inputString = i.ToString();
                if (Regex.Match(inputString, pattern).Success &&
                    (Regex.Match(inputString, duplicateMiddle).Success ||
                    Regex.Match(inputString, duplicateStart).Success ||
                    Regex.Match(inputString, duplicateEnd).Success))
                {
                    Console.WriteLine(i);
                    goodPasswords.Add(i);
                }
            }


            Console.WriteLine($"Good passwords: {goodPasswords.Count}");
        }
    }
}
