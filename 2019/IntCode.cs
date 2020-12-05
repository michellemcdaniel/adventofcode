using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventofcode
{
    public class IntCode
    {
        public int[] Opcodes { get;}

        public IntCode(int[] opcodes)
        {
            Opcodes = opcodes;
        }

        public int GetParameter(int location, int mode)
        {
            if (mode == 0)
            {
                // Position mode
                return Opcodes[location];
            }
            else
            {
                // Immediate mode
                return location;
            }
        }

        public int Compute(int input)
        {
            int currentOpcode = Opcodes[0];
            int currentLocation = 0;
            int replaceLocation = 0;
            int firstInput = 0;
            int secondInput = 0;

            int condition = 0;
            int jumpLocation = 0;

            int currentInput = input;

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
                        Opcodes[replaceLocation] = currentInput;
                        currentLocation = currentLocation+2;
                        break;
                    case 4:
                        replaceLocation = GetParameter(currentLocation+1, inst.FirstParameterMode);
                        currentInput = Opcodes[replaceLocation];
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
                    default:
                        break;
                }
                
                currentOpcode = Opcodes[currentLocation];
            }

            return currentInput;
        }
    }

    public class Instruction
    {
        public int Opcode { get; }
        public int FirstParameterMode { get; }
        public int SecondParameterMode { get; }
        public int ThirdParameterMode { get; }

        public Instruction(int instruction)
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