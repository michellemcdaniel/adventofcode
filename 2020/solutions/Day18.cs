using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace adventofcode
{
    class Day18
    {
        public static void Execute(string filename)
        {
            List<string> input = File.ReadAllLines(filename).ToList();

            long noPrecedence = 0;

            foreach(var i in input)
            {
                noPrecedence+= DoMath(i.Split(" "));
            }


            long precedence = 0;
            foreach(var i in input)
            {
                precedence+= DoMathPrecedence(i.Split(" "));
            }
            

            Console.WriteLine($"Part one: {noPrecedence}");

            Console.WriteLine($"Part two: {precedence}");
        }

        public static long DoMathPrecedence(string[] expression)
        {
            int bracketCounter = 0;
            int operatorIndex = -1;

            for(int i = 0; i < expression.Length; i++)
            {
                string c = expression[i];
                if (c == "(") bracketCounter++;
                else if (c == ")") bracketCounter--;
                else if ((c == "*") && bracketCounter == 0)
                {
                    operatorIndex = i;
                    break;
                }
                else if (c == "+" && bracketCounter == 0 && operatorIndex < 0)
                {
                    operatorIndex = i;
                }
            }

            if (operatorIndex < 0)
            {
                if (expression[0] == "(" && expression[expression.Length-1] == ")")
                {
                    int end = expression.Length-1;
                    return DoMathPrecedence(expression[1..end]);
                }
                else return long.Parse(expression.First());
            }

            else
            {
                int second = operatorIndex+1;
                if (expression[operatorIndex] == "+")
                {
                    return DoMathPrecedence(expression[0..operatorIndex]) + DoMathPrecedence(expression[second..]);
                }
                else
                {
                    return DoMathPrecedence(expression[0..operatorIndex]) * DoMathPrecedence(expression[second..]);
                }
            }
        }
        
        public static long DoMath(string[] expression)
        {
            int bracketCounter = 0;
            int lastCloseBracket = 0;
            int operatorIndex = -1;

            for(int i = 0; i < expression.Length; i++)
            {
                string c = expression[i];
                if (c == "(") bracketCounter++;
                else if (c == ")") bracketCounter--;
                else if ((c == "*" || c == "+") && bracketCounter == 0)
                {
                    operatorIndex = i;
                    break;
                }
            }

            if (operatorIndex < 0)
            {
                if (expression[0] == "(" && expression[expression.Length-1] == ")")
                {
                    int end = expression.Length-1;
                    return DoMath(expression[1..end]);
                }
                else return long.Parse(expression.First());
            }

            else
            {
                if (operatorIndex == 1)
                {
                    long rightSide = long.Parse(expression[0]);
                    
                    if (expression[operatorIndex+1] != "(")
                    {
                        if (expression[operatorIndex] == "+")
                        {
                            List<string> newExpression = new List<string>();
                            newExpression.Add($"{rightSide+int.Parse(expression[2])}");
                            newExpression.AddRange(expression[3..]);
                            return DoMath(newExpression.ToArray());
                        }
                        else
                        {
                            List<string> newExpression = new List<string>();
                            newExpression.Add($"{rightSide*int.Parse(expression[2])}");
                            newExpression.AddRange(expression[3..]);
                            return DoMath(newExpression.ToArray());
                        }
                    }
                    else
                    {
                        int start = operatorIndex+1;
                        bracketCounter = 0;
                        for(int i = start; i < expression.Length; i++)
                        {
                            string c = expression[i];
                            if (c == "(") bracketCounter++;
                            else if (c == ")") bracketCounter--;
                            if (bracketCounter == 0)
                            {
                                lastCloseBracket = i+1;
                                break;
                            }
                        }
                        if (expression[operatorIndex] == "+")
                        {
                            List<string> newExpression = new List<string>();
                            newExpression.Add($"{rightSide + DoMath(expression[start..lastCloseBracket])}");
                            int newStart = lastCloseBracket;
                            newExpression.AddRange(expression[newStart..]);
                            return DoMath(newExpression.ToArray());
                        }
                        else
                        {
                            List<string> newExpression = new List<string>();
                            newExpression.Add($"{rightSide * DoMath(expression[start..lastCloseBracket])}");
                            int newStart = lastCloseBracket;
                            newExpression.AddRange(expression[newStart..]);
                            return DoMath(newExpression.ToArray());
                        }
                    }
                }
                else
                {
                    int start = operatorIndex+1;
                    int end = operatorIndex;
                    if (expression[operatorIndex] == "+")
                    {
                        List<string> newExpression = new List<string>();
                        newExpression.Add($"{DoMath(expression[0..end])}");
                        newExpression.Add("+");
                        newExpression.AddRange(expression[start..]);
                        return DoMath(newExpression.ToArray());
                    }
                    else
                    {
                        List<string> newExpression = new List<string>();
                        newExpression.Add($"{DoMath(expression[0..end])}");
                        newExpression.Add("*");
                        newExpression.AddRange(expression[start..]);
                        return DoMath(newExpression.ToArray());
                    }
                }
            }
        }
    }
}