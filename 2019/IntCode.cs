using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventofcode
{
    public class IntCode
    {
        public long[] Opcodes { get; }
        public bool Halted { get; set; }
        public long RelativeBase { get; set; }

        public IntCode(long[] opcodes)
        {
            Opcodes = new long[100000000];
            for (long i = 0; i < opcodes.Length; i++)
            {
                Opcodes[i] = opcodes[i];
            }
            RelativeBase = 0;
        }

        public long GetParameter(long location, long mode)
        {
            if (mode == 0)
            {
                // Position mode
                return Opcodes[location];
            }
            else if (mode == 1)
            {
                // Immediate mode
                return location;
            }
            else if (mode == 2)
            {
                // Relative mode
                return Opcodes[location]+RelativeBase;
            }
            return location;
        }

        public Queue<long> Compute(Queue<long> input)
        {
            long currentOpcode = Opcodes[0];
            long currentLocation = 0;
            long replaceLocation = 0;
            long firstInput = 0;
            long secondInput = 0;

            long condition = 0;
            long jumpLocation = 0;

            Queue<long> currentInput = input;

            while(currentOpcode != 99 && currentLocation < Opcodes.Length)
            {
                Instruction inst = new Instruction(currentOpcode);
                switch (inst.Opcode)
                {
                    case 1:
                        replaceLocation = GetParameter(currentLocation+3, inst.ThirdParameterMode);
                        firstInput = GetParameter(currentLocation+1, inst.FirstParameterMode);
                        secondInput = GetParameter(currentLocation+2, inst.SecondParameterMode);

                        Opcodes[replaceLocation] = Opcodes[firstInput] + Opcodes[secondInput];
                        currentLocation = currentLocation+4;
                        break;
                    case 2:
                        replaceLocation = GetParameter(currentLocation+3, inst.ThirdParameterMode);
                        firstInput = GetParameter(currentLocation+1, inst.FirstParameterMode);
                        secondInput = GetParameter(currentLocation+2, inst.SecondParameterMode);

                        Opcodes[replaceLocation] = Opcodes[firstInput] * Opcodes[secondInput];
                        currentLocation = currentLocation+4;
                        break;
                    case 3: 
                        replaceLocation = GetParameter(currentLocation+1, inst.FirstParameterMode);
                        Opcodes[replaceLocation] = currentInput.Dequeue();
                        currentLocation = currentLocation+2;
                        break;
                    case 4:
                        replaceLocation = GetParameter(currentLocation+1, inst.FirstParameterMode);
                        currentInput.Enqueue(Opcodes[replaceLocation]);
                        currentLocation = currentLocation+2;
                        break;
                    case 5:
                        condition = GetParameter(currentLocation+1, inst.FirstParameterMode);
                        jumpLocation = GetParameter(currentLocation+2, inst.SecondParameterMode);
                        if (Opcodes[condition] != 0)
                        {
                            currentLocation = Opcodes[jumpLocation];
                        }
                        else 
                        {
                            currentLocation = currentLocation+3;
                        }
                        break;
                    case 6:
                        condition = GetParameter(currentLocation+1, inst.FirstParameterMode);
                        jumpLocation = GetParameter(currentLocation+2, inst.SecondParameterMode);
                        if (Opcodes[condition] == 0)
                        {
                            currentLocation = Opcodes[jumpLocation];
                        }
                        else 
                        {
                            currentLocation = currentLocation+3;
                        }
                        break;
                    case 7:
                        firstInput = GetParameter(currentLocation+1, inst.FirstParameterMode);
                        secondInput = GetParameter(currentLocation+2, inst.SecondParameterMode);
                        replaceLocation = GetParameter(currentLocation+3, inst.ThirdParameterMode);

                        if (Opcodes[firstInput] < Opcodes[secondInput])
                        {
                            Opcodes[replaceLocation] = 1;
                        }
                        else
                        {
                            Opcodes[replaceLocation] = 0;
                        }
                        currentLocation = currentLocation+4;
                        break;
                    case 8:
                        firstInput = GetParameter(currentLocation+1, inst.FirstParameterMode);
                        secondInput = GetParameter(currentLocation+2, inst.SecondParameterMode);
                        replaceLocation = GetParameter(currentLocation+3, inst.ThirdParameterMode);

                        if (Opcodes[firstInput] == Opcodes[secondInput])
                        {
                            Opcodes[replaceLocation] = 1;
                        }
                        else
                        {
                            Opcodes[replaceLocation] = 0;
                        }
                        currentLocation = currentLocation+4;
                        break;
                    case 9:
                        firstInput = GetParameter(currentLocation+1, inst.FirstParameterMode);
                        RelativeBase += Opcodes[firstInput];
                        currentLocation = currentLocation+2;
                        break;
                    default:
                        break;
                }
                
                currentOpcode = Opcodes[currentLocation];
            }

            if (currentLocation < Opcodes.Length)
            {
                Halted = true;
            }

            return currentInput;
        }

        public Queue<long> Compute()
        {
            return Compute(new Queue<long>());
        }

        public Queue<long> Compute(long input)
        {
            Queue<long> inputQueue = new Queue<long>();
            inputQueue.Enqueue(input);
            return Compute(inputQueue);
        }

        public Queue<long> Compute(long input, long phase)
        {
            Queue<long> inputQueue = new Queue<long>();
            inputQueue.Enqueue(phase);
            inputQueue.Enqueue(input);

            return Compute(inputQueue);
        }
    }

    public class Instruction
    {
        public long Opcode { get; }
        public long FirstParameterMode { get; }
        public long SecondParameterMode { get; }
        public long ThirdParameterMode { get; }

        public Instruction(long instruction)
        {
            string inst = instruction.ToString();
            inst = new string(inst.Reverse().ToArray());

            while (inst.Length < 5)
            {
                inst = $"{inst}0";
            }

            Opcode = Int32.Parse(new string(inst[0..1].Reverse().ToArray()));
            FirstParameterMode = Int32.Parse(inst[2].ToString());
            SecondParameterMode = Int32.Parse(inst[3].ToString());
            ThirdParameterMode = Int32.Parse(inst[4].ToString());
        }
    }
}