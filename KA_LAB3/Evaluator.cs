using KA_LAB3.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3
{
    internal class Evaluator
    {
        public Evaluator()
        {
        }
        public double Evaluate(NodeExpression root)
        {
            if(root.Kind == ExpressionKind.Binary)
            {
                BinaryExpression binaryExpression = (BinaryExpression)root;
                Token sign = binaryExpression.OperatorToken;
                double leftroot = Evaluate(binaryExpression.Left);
                double rightroot = Evaluate(binaryExpression.Right);
                if (sign.Kind == TokenKind.Plus)
                    return leftroot + rightroot;
                if (sign.Kind == TokenKind.Minus)
                    return leftroot - rightroot;
                if (sign.Kind == TokenKind.Star)
                    return leftroot * rightroot;
                if (sign.Kind == TokenKind.Slash)
                    return leftroot / rightroot;
                if (sign.Kind == TokenKind.Caret)
                    return Math.Pow(leftroot,rightroot);
                if (sign.Kind == TokenKind.Dot)
                {
                    while (rightroot >= 1)
                    {
                        rightroot /= 10;
                    }
                    return leftroot + rightroot;
                }
            }
            else if(root.Kind == ExpressionKind.Bracket)
            {
                BracketExpression bracketExpression = (BracketExpression)root;
                return Evaluate(bracketExpression.Node);
            }
            else if (root.Kind == ExpressionKind.Function)
            {
                FunctionExpression functionExpression = (FunctionExpression)root;
                BracketExpression bracket = (BracketExpression)functionExpression.Node;
                NodeExpression nodeInBracket = bracket.Node;
                string word = (string)functionExpression.Function.Value;
                if(nodeInBracket.Kind != ExpressionKind.Comma)
                {
                    double argument = Evaluate(nodeInBracket);
                    if (word == "arctg")
                    {
                        return Math.Atan(argument);
                    }
                }
                else
                {
                    CommaExpression commaExpression = (CommaExpression)nodeInBracket;
                    List<double> args = new List<double>();
                    foreach(NodeExpression node in commaExpression.GetNodes())
                    {
                        args.Add(Evaluate(node));
                    }
                    if (word == "min")
                    {
                        return args.Min();
                    }

                }
                
            }
            else if (root.Kind == ExpressionKind.Sign)
            {
                SignExpression signExpression = (SignExpression)root;
                return -Evaluate(signExpression.Node);
            }

            NumberExpression number = (NumberExpression)root;
            return Convert.ToDouble(number.Token.Value);
        }
    }
}
