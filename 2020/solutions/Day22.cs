using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace adventofcode
{
    class Day22
    {
        static Dictionary<int, List<string>> playerOneHistory = new();
        static Dictionary<int, List<string>> playerTwoHistory = new();
        public static void Execute(string filename)
        {
            Queue<string> input = new Queue<string>(File.ReadAllLines(filename));
            Queue<int> playerOne = new Queue<int>();
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

            Queue<int> winner = PlayGame(0, new Queue<int>(playerOne), new Queue<int>(playerTwo), false, out int result);
            Console.WriteLine($"Part One: {GetScore(winner)}");
            
            winner = PlayGame(0, new Queue<int>(playerOne), new Queue<int>(playerTwo), true, out result);
            Console.WriteLine($"Part Two: {GetScore(winner)}");
        }

        public static int GetScore(Queue<int> winner)
        {
            int score = 0;
            while (winner.Any())
            {
                int count = winner.Count();
                score += count * winner.Dequeue();
            }
            return score;
        }

        public static Queue<int> PlayGame(int game, Queue<int> playerOne, Queue<int> playerTwo, bool recursiveCombat, out int winnerId)
        {
            Queue<int> winner = new Queue<int>();
            winnerId = 0;
            playerOneHistory[game] = new List<string>();
            playerTwoHistory[game] = new List<string>();

            while (playerOne.Any() && playerTwo.Any())
            {
                if (recursiveCombat && (CheckHistory(playerOne, playerOneHistory[game]) || CheckHistory(playerTwo, playerTwoHistory[game])))
                {
                    winnerId = 1;
                    return playerOne;
                }

                playerOneHistory[game].Add(string.Join(" ", playerOne));
                playerTwoHistory[game].Add(string.Join(" ", playerTwo));

                int p1 = playerOne.Dequeue();
                int p2 = playerTwo.Dequeue();

                if (recursiveCombat && (p1 <= playerOne.Count() && p2 <= playerTwo.Count()))
                {
                    PlayGame(game+1, new Queue<int>(playerOne.Take(p1)), new Queue<int>(playerTwo.Take(p2)), recursiveCombat, out int newWinnerId);

                    if (newWinnerId == 1)
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
                else if (p1 > p2)
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

            winnerId = playerOne.Count() > 0 ? 1 : 2;
            return playerOne.Count() > 0 ? playerOne : playerTwo;
        }

        public static bool CheckHistory(Queue<int> deck, List<string> decks)
        {
            foreach (var oldDeck in decks.Where(d => d.StartsWith(deck.Peek().ToString())))
            {
                if (oldDeck == string.Join(" ", deck))
                {
                    return true;
                }
            }

            return false;
        }
    }
}