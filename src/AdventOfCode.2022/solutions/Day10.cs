using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AdventOfCode.TwentyTwo
{
    public class Day10
    {
        public static int rowLength = 40;
        public static void Execute(string filename)
        {
            int cycle = 0;
            int registerX = 1;
            long signalStrength = 0;
            StringBuilder crt = new();

            foreach (var line in File.ReadLines(filename))
            {
                string[] inst = line.Split(" ");
                switch (inst[0]) {
                    case "noop":
                        DrawPixel(crt, cycle, registerX);
                        signalStrength+=IncreaseSignalStrength(++cycle, registerX);
                        break;

                    case "addx":
                        DrawPixel(crt, cycle, registerX);
                        signalStrength+=IncreaseSignalStrength(++cycle, registerX);
                        
                        DrawPixel(crt, cycle, registerX);
                        signalStrength+=IncreaseSignalStrength(++cycle, registerX);
                        registerX += int.Parse(inst[1]);
                        break;

                    default:
                        break;
                }
            }

            Console.WriteLine($"Part one: {signalStrength}");
            Console.WriteLine($"Part two: {crt.ToString()}");
        }

        public static int IncreaseSignalStrength(int cycle, int registerX)
        {
            return (cycle-20)%rowLength == 0 ? cycle*registerX : 0;
        }

        public static void DrawPixel(StringBuilder crt, int cycle, int spriteMiddle)
        {
            if ((cycle)%rowLength == 0)
            {
                crt.AppendLine();
            }

            crt.Append(Math.Abs((cycle)%rowLength - spriteMiddle) <= 1 ? '#' : " ");   
        }
    }
}
