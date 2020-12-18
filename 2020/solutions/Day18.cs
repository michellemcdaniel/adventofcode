using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Irony.Parsing;

namespace adventofcode
{
    class Day18
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();

            long withGrammarNoPrecedence = 0;
            long withGrammarPrecedence = 0;

            long withStackNoPrecedence = 0;
            long withStackAddPrecedence = 0;

            foreach(var i in input)
            {
                withGrammarNoPrecedence += Evaluate(i, Precedence.NoPrecedence);
                withGrammarPrecedence += Evaluate(i, Precedence.SwappedPrecedence);

                withStackNoPrecedence += StackMath(i, Precedence.NoPrecedence);
                withStackAddPrecedence += StackMath(i, Precedence.SwappedPrecedence);
            }

            Console.WriteLine($"Part one, grammar: {withGrammarNoPrecedence}");
            Console.WriteLine($"Part two, grammar: {withGrammarPrecedence}");

            Console.WriteLine($"Part one, stack: {withStackNoPrecedence}");
            Console.WriteLine($"Part two, stack: {withStackAddPrecedence}");
        }

        public static long Evaluate(string input, Precedence precedence)
        {
            Parser p = new Parser(new AoCGrammar(precedence));
            var tree = p.Parse(input);

            var root = ((IAoCValueNode) tree.Root.AstNode);
            return root.Evaluate();
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
                        long total = 0;
                        switch (precedence)
                        {
                            case Precedence.NoPrecedence:
                                total = math.Pop().Calculate();
                                break;
                            case Precedence.SwappedPrecedence:
                                total = math.Pop().CalculateWithAddPrecedence();
                                break;
                            default: 
                                throw new ArgumentException($"{precedence} is not a recognized precedence", "precedence");
                        }
                        math.Peek().Values.Add(total);
                        break;
                    default:
                        long value = long.Parse(c.ToString());
                        math.Peek().Values.Add(value);
                        break;
                }
            }

            switch (precedence)
            {
                case Precedence.NoPrecedence:
                    return math.Peek().Calculate();
                case Precedence.SwappedPrecedence:
                    return math.Peek().CalculateWithAddPrecedence();
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
                long currentVal = -1;

                for (int i = 0; i < Values.Count(); i++)
                {
                    if (Operators[i] == '+')
                    {
                        currentVal = stack.Pop();
                    }
                    else
                    {
                        stack.Push(Values[i]);
                    }
                    if (currentVal != -1)
                    {
                        stack.Push(currentVal + Values[i]);
                        currentVal = -1;
                    }
                }

                long total = 1;

                while(stack.Any())
                {
                    total*=stack.Pop();
                }

                return total;
            }
        }
    }
}