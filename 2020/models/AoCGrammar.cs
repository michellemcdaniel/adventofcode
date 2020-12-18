using Irony.Ast;
using Irony.Parsing;
using System;

namespace adventofcode
{
    public enum Precedence
    {
        NoPrecedence,
        SwappedPrecedence,
        RegularPrecedence
    }
    public class AoCGrammar : Grammar
    {
        public AoCGrammar(Precedence precedence)
        {
            var number = new NumberLiteral("Number", NumberOptions.IntOnly, typeof(NumberNode));

            var binOp = new NonTerminal("BinaryOperator", "operator");
            var parenExpr = new NonTerminal("ParenthesisExpresion");
            var binExpr = new NonTerminal("BinaryExpression", typeof(BinaryOperationNode));
            var expr = new NonTerminal("Expression");

            expr.Rule = number | parenExpr | binExpr;
            parenExpr.Rule = "(" + expr + ")";
            binExpr.Rule = expr + binOp + expr;
            binOp.Rule = ToTerm("+") | "*";

            switch (precedence)
            {
                case Precedence.NoPrecedence:
                    RegisterOperators(1, "*", "+");
                    break;
                case Precedence.SwappedPrecedence:
                    RegisterOperators(1, "*");
                    RegisterOperators(2, "+");
                    break;
                case Precedence.RegularPrecedence:
                    RegisterOperators(2, "*");
                    RegisterOperators(1, "+");
                    break;
            }

            MarkPunctuation("(",")");
            RegisterBracePair("(",")");
            MarkTransient(expr, binOp, parenExpr);

            LanguageFlags = LanguageFlags.CreateAst;

            Root = expr;
        }
    }

    public interface IAoCValueNode
    {
        long Evaluate();
    }

    public class NumberNode : IAstNodeInit, IAoCValueNode
    {
        long Value { get;set; }
        public void Init(AstContext context, ParseTreeNode node)
        {
            Value = Int64.Parse(node.Token.Text);
        }

        public long Evaluate()
        {
            return Value;
        }
    }

    public class BinaryOperationNode : IAstNodeInit, IAoCValueNode
    {
        public IAoCValueNode Left;
        public IAoCValueNode Right;
        public char Op;

        public void Init(AstContext context, ParseTreeNode node)
        {
            Left = (IAoCValueNode) node.ChildNodes[0].AstNode;
            Op = node.ChildNodes[1].Token.Text[0];
            Right = (IAoCValueNode) node.ChildNodes[2].AstNode;
        }

        public long Evaluate()
        {
            switch(Op)
            {
                case '+': return Left.Evaluate() + Right.Evaluate();
                case '*': return Left.Evaluate() * Right.Evaluate();
            }
            throw new ArgumentException($"{Op} is not a recognized operator!");
        }
    }
}