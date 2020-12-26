using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace AdventOfCode.Twenty
{
    public class Day22
    {
        public static void Execute(string filename)
        {
            Queue<string> input = new Queue<string>(File.ReadAllLines(filename));
            Queue<int> playerOne = new();
            Queue<int> playerTwo = new();

            input.Dequeue();

            while (!string.IsNullOrEmpty(input.Peek()))
            {
                playerOne.Enqueue(int.Parse(input.Dequeue()));
            }
            
            input.Dequeue();
            input.Dequeue();

            while(input.Any() && !string.IsNullOrEmpty(input.Peek()))
            {
                playerTwo.Enqueue(int.Parse(input.Dequeue()));
            }

            Queue<int> winner = PlayGame(new Queue<int>(playerOne), new Queue<int>(playerTwo), false, out bool playerOneWins);
            Console.WriteLine($"Part One: {GetScore(winner)}");
            
            winner = PlayGame(new Queue<int>(playerOne), new Queue<int>(playerTwo), true, out playerOneWins);
            Console.WriteLine($"Part Two: {GetScore(winner)}");
        }

        public static int GetScore(Queue<int> winner)
        {
            int score = 0;
            while (winner.Any())
            {
                score += winner.Count() * winner.Dequeue();
            }
            return score;
        }

        public static Queue<int> PlayGame(Queue<int> playerOne, Queue<int> playerTwo, bool recursiveCombat, out bool playerOneWins)
        {
            HashSet<string> history = new HashSet<string>();

            while (playerOne.Any() && playerTwo.Any())
            {
                if (recursiveCombat && !history.Add($"{string.Join("", playerOne)}+{string.Join("", playerTwo)}"))
                {
                    playerOneWins = true;
                    return playerOne;
                }

                int p1 = playerOne.Dequeue();
                int p2 = playerTwo.Dequeue();
                bool playerOneWinsRound = false;

                if (recursiveCombat && (p1 <= playerOne.Count() && p2 <= playerTwo.Count()))
                {
                    PlayGame(new Queue<int>(playerOne.Take(p1)), new Queue<int>(playerTwo.Take(p2)), recursiveCombat, out playerOneWinsRound);
                }
                else
                {
                    playerOneWinsRound = p1 > p2;
                }

                if (playerOneWinsRound)
                {
                    playerOne.Enqueue(p1);
                    playerOne.Enqueue(p2);
                }
                else
                {
                    playerTwo.Enqueue(p2);
                    playerTwo.Enqueue(p1);
                }
            }

            playerOneWins = playerOne.Count() > 0;
            return playerOneWins ? playerOne : playerTwo;
        }
    }
}