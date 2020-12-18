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
                    // Multiplication has the least precedence. Once we find it, break.
                    operatorIndex = i;
                    break;
                }
                else if (c == "+" && bracketCounter == 0 && operatorIndex < 0)
                {
                    // We found addition, but since it has higher precedence than
                    // multiplication, let's go see if we can find a multiplication first
                    // If we don't, we'll multiply. That's fine too
                    operatorIndex = i;
                }
            }

            if (operatorIndex < 0)
            {
                if (expression[0] == "(" && expression[expression.Length-1] == ")")
                {
                    // Get rid of those pesky parens and do the math over.
                    int end = expression.Length-1;
                    return DoMathPrecedence(expression[1..end]);
                }
                
                // Rejoice, for we have found our end number.
                return long.Parse(expression.First());
            }

            else
            {
                // Do the math. So much easier with precendence
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
                    // We found our first operator. We can gtfo now.
                    operatorIndex = i;
                    break;
                }
            }

            if (operatorIndex < 0)
            {
                if (expression[0] == "(" && expression[expression.Length-1] == ")")
                {
                    // We found a paren range. Remove the parens, rejoice.
                    int end = expression.Length-1;
                    return DoMath(expression[1..end]);
                }
                
                // We have a single number. Return it. The recursion is over.
                return long.Parse(expression.First());
            }

            else
            {
                if (operatorIndex == 1)
                {
                    long rightSide = long.Parse(expression[0]);
                    
                    if (expression[operatorIndex+1] != "(")
                    {
                        // if the next token is not an open paren, it can only be a number, so do the math
                        // and then paste the math back onto the expression
                        List<string> newExpression = new List<string>();
                        if (expression[operatorIndex] == "+")
                        {
                            newExpression.Add($"{rightSide+int.Parse(expression[2])}");
                        }
                        else
                        {
                            newExpression.Add($"{rightSide*int.Parse(expression[2])}");
                        }

                        newExpression.AddRange(expression[3..]);
                        return DoMath(newExpression.ToArray());
                    }
                    else
                    {
                        // if the next token is a paren, we need to do that stuff first, so find the end of
                        // the paren section
                        int start = operatorIndex+1;
                        bracketCounter = 0;
                        for(int i = start; i < expression.Length; i++)
                        {
                            string c = expression[i];
                            if (c == "(") bracketCounter++;
                            else if (c == ")") bracketCounter--;
                            if (bracketCounter == 0)
                            {
                                // don't really know why it needs to be + 1, but it does?
                                lastCloseBracket = i+1;
                                break;
                            }
                        }

                        // Paste the math of the right side combined with the expression in the parens
                        // back onto everything that comes after the parens.
                        List<string> newExpression = new List<string>();
                        if (expression[operatorIndex] == "+")
                        {
                            newExpression.Add($"{rightSide + DoMath(expression[start..lastCloseBracket])}");
                        }
                        else
                        {
                            newExpression.Add($"{rightSide * DoMath(expression[start..lastCloseBracket])}");
                        }

                        int newStart = lastCloseBracket;
                        newExpression.AddRange(expression[newStart..]);
                        return DoMath(newExpression.ToArray());
                    }
                }
                else
                {
                    // It was not a number to start, but a thing in parens, so we need to do the paren
                    // math. Luckily, we can use the operatorIndex as the end of the paren range,
                    // since it was the first operator after all the parens closed. So do that math,
                    // and then paste that value back in front of the list, along with the operator. Means we will 
                    // have to parse this operation again, but c'est la vie.
                    int start = operatorIndex+1;

                    List<string> newExpression = new List<string>();
                    newExpression.Add($"{DoMath(expression[0..operatorIndex])}");
                    newExpression.Add(expression[operatorIndex]);
                    newExpression.AddRange(expression[start..]);
                    return DoMath(newExpression.ToArray());
                }
            }
        }
    }
}