﻿using KA_LAB3.Error;
using KA_LAB3.Expression;
using KA_LAB3.Polynomes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
            if (nodeInBracket.Kind != ExpressionKind.Set)
            {
                double argument = Evaluate(nodeInBracket);
                if (word == "arctg")
                    return Math.Atan(argument);
                if (word == "sin")
                    return Math.Sin(argument);
                if (word == "cos")
                    return Math.Cos(argument);
                if (word == "tg")
                    return Math.Tan(argument);
                if (word == "ctg")
                    return 1/Math.Tan(argument);
                if (word == "ln")
                    return Math.Log(argument);
            }
            else
            {
                SetExpression setExpression = (SetExpression)nodeInBracket;
                List<double> args = new List<double>();
                foreach (NodeExpression node in setExpression.GetNodes())
                {
                    args.Add(Evaluate(node));
                }
                if (word == "min")
                {
                    return args.Min();
                }
                if (word == "f1")
                {
                    if(args.Count!=2) ErrorWriting.ShowBadToken(function);
                    return 1 / (Math.Pow(args[0], 2) + Math.Pow(args[1], 2));
                }
                if (word == "var")
                {
                    return Var(args);
                }
                if (word == "log")
                {
                    if (args.Count != 2) ErrorWriting.ShowBadToken(function);
                    return Math.Log(args[0],args[1]);
                }
                if (word == "pol")
                {
                    if (args.Count < 2) ErrorWriting.ShowBadToken(function);
                    double x=args[0];
                    double[] koefs = new double[args.Count-1];
                    for (int i = 1; i < args.Count; i++)
                    {
                        koefs[i-1] = args[i];
                    }
                    Polynom polynom = new Polynom(koefs);
                    return polynom.Solve(x);
                }
            }
            ErrorWriting.ShowBadToken(function);
            return 0;
        }
        private double Var(List<double> args)
        {
            double m = args.Average();
            return args.Sum(x => Math.Pow(x - m, 2)) / (args.Count - 1);
        }
    }
}
