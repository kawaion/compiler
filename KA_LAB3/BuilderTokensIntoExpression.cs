using KA_LAB3.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3
{
    internal class BuilderTokensIntoExpression
    {
        private readonly List<Token> _tokens;
        private int _position;

        public BuilderTokensIntoExpression(List<Token> tokens)
        {
            _tokens = tokens;
            _position = 0;
        }
        private Token Current => Peek(0);
        private Token Peek(int offset) => _tokens[_position + offset];
        private Token NextToken()
        {
            var token = Current;
            _position++;
            return token;
        }
        private Token Match(TokenKind kind)
        {
            if(Current.Kind == kind)
                return NextToken();
            return new Token(TokenKind.Bad, kind, Current.Position);
        }
        public NodeExpression Build()
        {
            return StartNode();
        }
        private NodeExpression StartNode()
        {
            NodeExpression leftNode = FactorNode();
            while (Current.Kind == TokenKind.Plus ||
                   Current.Kind == TokenKind.Minus)
            {
                Token sign = NextToken();
                NodeExpression rightNode = FactorNode();
                leftNode = new BinaryExpression(leftNode, sign, rightNode);
            }
            return leftNode;
        }
        private NodeExpression FactorNode()
        {
            NodeExpression leftNode = PowNode();
            while (Current.Kind == TokenKind.Star ||
                   Current.Kind == TokenKind.Slash)
            {
                Token sign = NextToken();
                NodeExpression rightNode = PowNode();
                leftNode = new BinaryExpression(leftNode, sign, rightNode);
            }
            return leftNode;
        }
        private NodeExpression PowNode() 
        {
            NodeExpression leftNode = DoubleNode();
            while (Current.Kind == TokenKind.Caret)
            {
                Token pow = NextToken();
                NodeExpression rightNode = DoubleNode();
                leftNode = new BinaryExpression(leftNode, pow,rightNode);
            }
            return leftNode;
        }
        private NodeExpression DoubleNode()
        {
            NodeExpression leftNode = CommaNode();
            while (Current.Kind == TokenKind.Dot)
            {
                Token dot = NextToken();
                NodeExpression rightNode = CommaNode();
                leftNode = new BinaryExpression(leftNode, dot, rightNode);
            }
            return leftNode;
        }
        private NodeExpression CommaNode() 
        {
            NodeExpression leftNode = PrimaryNode();
            CommaExpression comma = new CommaExpression(leftNode);
            while (Current.Kind == TokenKind.Comma)
            {
                NextToken();
                NodeExpression rightNode = PrimaryNode();
                comma.Add(rightNode);
                leftNode = comma;
            }
            return leftNode;
        }
        private NodeExpression PrimaryNode()
        {
            if (Current.Kind == TokenKind.Var)
            {
                return new VarExpression(NextToken());
            }
            else if(Current.Kind == TokenKind.OpenBracket)
            {
                Token openBracket = NextToken();
                NodeExpression nodeExpression = StartNode();
                Token closedBracket = Match(TokenKind.ClosedBracket);
                return new BracketExpression(openBracket, nodeExpression, closedBracket);
            }
            else if (Current.Kind == TokenKind.Function)
            {
                Token function = Match(TokenKind.Function);
                Token openBracket = Match(TokenKind.OpenBracket);
                NodeExpression nodeExpression = StartNode();
                Token closedBracket = Match(TokenKind.ClosedBracket);
                BracketExpression bracketExpression = new BracketExpression(openBracket, nodeExpression, closedBracket);
                return new FunctionExpression(function,bracketExpression);
            }
            else if (Current.Kind == TokenKind.Minus)
            {
                Token sign = NextToken();
                if(Current.Kind == TokenKind.OpenBracket)
                {
                    Token openBracket = Match(TokenKind.OpenBracket);
                    NodeExpression nodeExpression = StartNode();
                    Token closedBracket = Match(TokenKind.ClosedBracket);
                    BracketExpression bracketExpression = new BracketExpression(openBracket, nodeExpression, closedBracket);
                    return new SignExpression(sign, bracketExpression);
                }
                else
                {
                    if(Current.Kind == TokenKind.Number)
                    {
                        NumberExpression numberExpression = new NumberExpression(NextToken());
                        return new SignExpression(sign, numberExpression);
                    }
                    else
                    {
                        VarExpression varExpression = new VarExpression(Match(TokenKind.Var));
                        return new SignExpression(sign, varExpression);
                    }
                }
            }
            Token number = Match(TokenKind.Number);
            return new NumberExpression(number);
        }
    }
}
