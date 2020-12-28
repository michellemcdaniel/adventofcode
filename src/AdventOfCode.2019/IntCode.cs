using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace AdventOfCode.Nineteen
{
    public class IntCode
    {
        public long[] Opcodes { get; }
        public bool Halted { get; set; }
        public bool Paused { get; set; }
        public bool EmptyQueue { get; set; }
        public long InstructionPointer { get; set; }
        public long RelativeBase { get; set; }
        public bool Pause { get; set; }
        public Queue<long> Input { get; set; }

        public IntCode(long[] opcodes)
        {
            Opcodes = new long[100000000];
            for (long i = 0; i < opcodes.Length; i++)
            {
                Opcodes[i] = opcodes[i];
            }
            RelativeBase = 0;
            InstructionPointer = 0;
            Input = new Queue<long>();
        }

        public IntCode(long[] opcode, bool pause) : this(opcode)
        {
            Pause = pause;
        }

        public IntCode(long[] opcode, bool pause, bool empty) : this(opcode, pause)
        {
            EmptyQueue = empty;
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

        public void Resume(long input)
        {
            Input.Enqueue(input);
            Paused = false;
        }

        public void Resume()
        {
            Paused = false;
        }

        public void Resume(List<int> input)
        {
            foreach (var i in input)
            {
                Input.Enqueue(i);
            }
            Paused = false;
        }

        public long ComputeResult()
        {
            long currentOpcode = Opcodes[InstructionPointer];
            long replaceLocation = 0;
            long firstInput = 0;
            long secondInput = 0;

            long condition = 0;
            long jumpLocation = 0;

            while(!Halted && !Paused && InstructionPointer < Opcodes.Length)
            {
                Instruction inst = new Instruction(currentOpcode);
                switch (inst.Opcode)
                {
                    case 1:
                        replaceLocation = GetParameter(InstructionPointer+3, inst.ThirdParameterMode);
                        firstInput = GetParameter(InstructionPointer+1, inst.FirstParameterMode);
                        secondInput = GetParameter(InstructionPointer+2, inst.SecondParameterMode);

                        Opcodes[replaceLocation] = Opcodes[firstInput] + Opcodes[secondInput];
                        InstructionPointer = InstructionPointer+4;
                        break;
                    case 2:
                        replaceLocation = GetParameter(InstructionPointer+3, inst.ThirdParameterMode);
                        firstInput = GetParameter(InstructionPointer+1, inst.FirstParameterMode);
                        secondInput = GetParameter(InstructionPointer+2, inst.SecondParameterMode);

                        Opcodes[replaceLocation] = Opcodes[firstInput] * Opcodes[secondInput];
                        InstructionPointer = InstructionPointer+4;
                        break;
                    case 3: 
                        replaceLocation = GetParameter(InstructionPointer+1, inst.FirstParameterMode);
                        Opcodes[replaceLocation] = Input.Dequeue();
                        InstructionPointer = InstructionPointer+2;
                        break;
                    case 4:
                        replaceLocation = GetParameter(InstructionPointer+1, inst.FirstParameterMode);

                        if (Pause)
                        {
                            Paused = true;
                        }

                        Input.Enqueue(Opcodes[replaceLocation]);
                        InstructionPointer = InstructionPointer+2;
                        break;
                    case 5:
                        condition = GetParameter(InstructionPointer+1, inst.FirstParameterMode);
                        jumpLocation = GetParameter(InstructionPointer+2, inst.SecondParameterMode);
                        if (Opcodes[condition] != 0)
                        {
                            InstructionPointer = Opcodes[jumpLocation];
                        }
                        else 
                        {
                            InstructionPointer = InstructionPointer+3;
                        }
                        break;
                    case 6:
                        condition = GetParameter(InstructionPointer+1, inst.FirstParameterMode);
                        jumpLocation = GetParameter(InstructionPointer+2, inst.SecondParameterMode);
                        if (Opcodes[condition] == 0)
                        {
                            InstructionPointer = Opcodes[jumpLocation];
                        }
                        else 
                        {
                            InstructionPointer = InstructionPointer+3;
                        }
                        break;
                    case 7:
                        firstInput = GetParameter(InstructionPointer+1, inst.FirstParameterMode);
                        secondInput = GetParameter(InstructionPointer+2, inst.SecondParameterMode);
                        replaceLocation = GetParameter(InstructionPointer+3, inst.ThirdParameterMode);

                        if (Opcodes[firstInput] < Opcodes[secondInput])
                        {
                            Opcodes[replaceLocation] = 1;
                        }
                        else
                        {
                            Opcodes[replaceLocation] = 0;
                        }
                        InstructionPointer = InstructionPointer+4;
                        break;
                    case 8:
                        firstInput = GetParameter(InstructionPointer+1, inst.FirstParameterMode);
                        secondInput = GetParameter(InstructionPointer+2, inst.SecondParameterMode);
                        replaceLocation = GetParameter(InstructionPointer+3, inst.ThirdParameterMode);

                        if (Opcodes[firstInput] == Opcodes[secondInput])
                        {
                            Opcodes[replaceLocation] = 1;
                        }
                        else
                        {
                            Opcodes[replaceLocation] = 0;
                        }
                        InstructionPointer = InstructionPointer+4;
                        break;
                    case 9:
                        firstInput = GetParameter(InstructionPointer+1, inst.FirstParameterMode);
                        RelativeBase += Opcodes[firstInput];
                        InstructionPointer = InstructionPointer+2;
                        break;
                    default:
                        break;
                }
                
                currentOpcode = Opcodes[InstructionPointer];
                if (Opcodes[InstructionPointer] == 99)
                {
                    Halted = true;
                }
            }

            long returnValue = 0;

            // Trying to make this work for day 7 and everything else.
            if (Input.Count() == 1 || EmptyQueue)
            {
                while (Input.Any())
                {
                    returnValue = Input.Dequeue();
                }
            }
            else if (Input.Any())
            {
                return Input.Last();
            }
            
            return returnValue;
        }

        public long Compute()
        {
            return ComputeResult();
        }

        public long Compute(long input)
        {
            Input.Enqueue(input);
            return ComputeResult();
        }

        public long Compute(long input, long phase)
        {
            Input.Enqueue(phase);
            Input.Enqueue(input);

            return ComputeResult();
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