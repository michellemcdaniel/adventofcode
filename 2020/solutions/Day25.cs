using System;
using System.IO;

namespace AdventOfCode.Twenty
{
    class Day25
    {
        public static void Execute(string filename)
        {
            string[] input = File.ReadAllLines(filename);

            int modValue = 20201227;
            int cardPublicKey = int.Parse(input[0]);
            int doorPublicKey = int.Parse(input[1]);
            long doorLoopSize = 0;

            int val = 1;
            while (val != doorPublicKey)
            {
                doorLoopSize++;
                val = (val*7)%modValue;
            }

            long encryptionKey = 1;
            for (int i = 0; i < doorLoopSize; i++)
            {
                encryptionKey = (encryptionKey*cardPublicKey)%modValue;
            }

            Console.WriteLine($"Part One: {encryptionKey}");
        }
    }
}