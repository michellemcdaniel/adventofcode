using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.TwentyTwo
{
    public class Day02
    {
        public static void Execute(string filename)
        {
            int totalScore = 0;
            int alternativeTotal = 0; 

            foreach(var line in File.ReadLines(filename))
            {
                char[] round = line.Split(" ").Select(a => char.Parse(a)).ToArray();

                int score = round[1] - 'X' + 1;
                int theirs = round[0] - 'A' + 1;

                if (score == theirs)
                {
                    score += + 3;
                }
                else if (score - 1 == theirs || score + 2 == theirs)
                {
                    score = score + 6;
                }

                totalScore += score;

                int alternativeScore = (round[1] - 'X') * 3;

                if (alternativeScore == 3)
                {
                    alternativeScore += theirs;
                }
                else if (alternativeScore == 6)
                {
                    alternativeScore += theirs % 3 + 1;
                }
                else
                {
                    // I honestly don't want to have to figure out inverse mod, which is what I need, so this is going to have to be gooooood enough.
                    int increase = theirs - 1;
                    alternativeScore += (increase == 0 ? 3 : increase);
                    
                }

                alternativeTotal += alternativeScore;
            }

            Console.WriteLine($"Part one: {totalScore}");
            Console.WriteLine($"Part two: {alternativeTotal}");
        }
    }
}
