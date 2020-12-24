using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day18
    {
        public enum Precedence
        {
            NoPrecedence,
            SwappedPrecedence
        }
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();

            long withStackNoPrecedence = 0;
            long withStackAddPrecedence = 0;

            foreach(var i in input)
            {
                withStackNoPrecedence += StackMath(i, Precedence.NoPrecedence);
                withStackAddPrecedence += StackMath(i, Precedence.SwappedPrecedence);
            }

            Console.WriteLine($"Part One, stack: {withStackNoPrecedence}");
            Console.WriteLine($"Part Two, stack: {withStackAddPrecedence}");
        }

        public static long StackMath(string expression, Precedence precedence)
        {
            Stack<Operation> math = new Stack<Operation>();
            math.Push(new Operation());

            foreach(char c in expression.Replace(" ", ""))
            {
                switch (c)
                {
                    case '+':
                    case '*':
                        math.Peek().Operators.Add(c);
                        break;
                    case '(':
                        math.Push(new Operation());
                        break;
                    case ')':
                        long total = DoMath(math.Pop(), precedence);
                        math.Peek().Values.Add(total);
                        break;
                    default:
                        long value = long.Parse(c.ToString());
                        math.Peek().Values.Add(value);
                        break;
                }
            }

            return DoMath(math.Pop(), precedence);
        }

        public static long DoMath(Operation operation, Precedence precedence)
        {
            switch (precedence)
            {
                case Precedence.NoPrecedence:
                    return operation.Calculate();
                case Precedence.SwappedPrecedence:
                    return operation.CalculateWithAddPrecedence();
                default: 
                    throw new ArgumentException($"{precedence} is not a recognized precedence", "precedence");
            }
        }

        public class Operation
        {
            public List<char> Operators {get;set;}
            public List<long> Values {get;set;}

            public Operation()
            {
                Operators = new List<char>();
                // Each operator starts with a + so that calculate works properly.
                Operators.Add('+');
                Values = new List<long>();
            }

            public long Calculate()
            {
                long total = 0;
                for (int i = 0; i < Values.Count(); i++)
                {
                    if (Operators[i] == '+')
                    {
                        total += Values[i];
                    }
                    else
                    {
                        total *= Values[i];
                    }
                }
                return total;
            }

            public long CalculateWithAddPrecedence()
            {
                Stack<long> stack = new Stack<long>();
                stack.Push(0);

                for (int i = 0; i < Values.Count(); i++)
                {
                    if (Operators[i] == '+')
                    {
                        stack.Push(stack.Pop() + Values[i]);
                    }
                    else
                    {
                        stack.Push(Values[i]);
                    }
                }

                return stack.Aggregate(1L, (x,y) => x*y);
            }
        }
    }
}