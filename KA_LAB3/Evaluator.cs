using KA_LAB3.Error;
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
        private readonly Dictionary<string, double> _dictVars;

        public Evaluator(Dictionary<string, double> dictVars)
        {
            _dictVars = dictVars;
        }
        public double Evaluate(NodeExpression root)
        {
            switch (root.Kind)
            {
                case ExpressionKind.Binary:
                    {
                        BinaryExpression binaryExpression = (BinaryExpression)root;
                        Token sign = binaryExpression.OperatorToken;
                        double leftRoot = Evaluate(binaryExpression.Left);
                        double rightRoot = Evaluate(binaryExpression.Right);
                        switch (sign.Kind)
                        {
                            case TokenKind.Plus: return leftRoot + rightRoot;
                            case TokenKind.Minus: return leftRoot - rightRoot;
                            case TokenKind.Star: return leftRoot * rightRoot;
                            case TokenKind.Slash: return leftRoot / rightRoot;
                            case TokenKind.Caret: return Math.Pow(leftRoot, rightRoot);
                            default:
                                ErrorWriting.ShowBadToken(sign);
                            return 0;
                        }
                    
                    }
                case ExpressionKind.Bracket:
                    {
                        BracketExpression bracketExpression = (BracketExpression)root;
                        return Evaluate(bracketExpression.Node);
                    }
                case ExpressionKind.Literal:
                    {
                        LiteralExpression literalExpression = (LiteralExpression)root;
                        Token token = literalExpression.Token;
                        if (token.Kind == TokenKind.Var)
                            return _dictVars[(string)token.Value];
                        if (token.Kind == TokenKind.Number)
                            return (double)token.Value;
                        ErrorWriting.ShowBadToken(token);
                        return 0;
                    }
                case ExpressionKind.Unary:
                    {
                        UnaryExpression unaryExpression = (UnaryExpression)root;
                        Token tokenOperator = unaryExpression.TokenOperator;
                        NodeExpression node = unaryExpression.Node;
                        if (tokenOperator.Kind == TokenKind.Plus)
                            return Evaluate(node);
                        if (tokenOperator.Kind == TokenKind.Minus)
                            return -Evaluate(node);
                        if (tokenOperator.Kind == TokenKind.Function)
                            return SolveFunction(tokenOperator,node);
                        ErrorWriting.ShowBadToken(tokenOperator);
                        return 0;
                    }
                default:
                    throw new Exception($"неизвестное выражение {root.Kind}");
            }
        }
        private double SolveFunction(Token function,NodeExpression bracketNode)
        {
            BracketExpression bracket = (BracketExpression)bracketNode;
            NodeExpression nodeInBracket = bracket.Node;
            string word = (string)function.Value;
            if (nodeInBracket.Kind != ExpressionKind.Comma)
            {
                double argument = Evaluate(nodeInBracket);
                if (word == "arctg")
                {
                    return Math.Atan(argument);
                }
            }
            else
            {
                SetExpression commaExpression = (SetExpression)nodeInBracket;
                List<double> args = new List<double>();
                foreach (NodeExpression node in commaExpression.GetNodes())
                {
                    args.Add(Evaluate(node));
                }
                if (word == "min")
                {
                    return args.Min();
                }
                if (word == "f1")
                {
                    return 1 / (Math.Pow(args[0], 2) + Math.Pow(args[1], 2));
                }

            }
            ErrorWriting.ShowBadToken(function);
            return 0;
        }
    }
}
