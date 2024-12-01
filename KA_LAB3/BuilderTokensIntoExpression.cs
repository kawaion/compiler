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
            return new Token(TokenKind.Bad, Current.Kind, Current.Position);
        }
        public NodeExpression Build()
        {
            return StartNode();
        }
        private NodeExpression StartNode()
        {
            return diffNode();
        }
        private NodeExpression diffNode()
        {
            NodeExpression leftNode = CommaNode();
            while (Current.Kind == TokenKind.Apostrophe)
            {
                Token sign = NextToken();
                NodeExpression rightNode = CommaNode();
                leftNode = new BinaryExpression(leftNode, sign, rightNode);
            }
            return leftNode;
        }
        private NodeExpression CommaNode()
        {
            NodeExpression leftNode = TermNode();
            CommaExpression comma = new CommaExpression(leftNode);
            while (Current.Kind == TokenKind.Comma)
            {
                NextToken();
                NodeExpression rightNode = TermNode();
                comma.Add(rightNode);
                leftNode = comma;
            }
            return leftNode;
        }
        private NodeExpression TermNode()
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
            NodeExpression leftNode = PrimaryNode();
            while (Current.Kind == TokenKind.Dot)
            {
                Token dot = NextToken();
                NodeExpression rightNode = PrimaryNode();
                leftNode = new BinaryExpression(leftNode, dot, rightNode);
            }
            return leftNode;
        }
        private NodeExpression PrimaryNode()
        {
            switch (Current.Kind)
            {

                case TokenKind.Var: return new VarExpression(NextToken());

                case TokenKind.OpenBracket: return BracketNode();

                case TokenKind.Function:
                    {
                        Token function = Match(TokenKind.Function);
                        return new FunctionExpression(function, PrimaryNode());
                    }
                case TokenKind.Minus:
                    {
                        Token sign = NextToken();
                        return new SignExpression(sign, PrimaryNode());
                    }
                default:
                    {
                        Token number = Match(TokenKind.Number);
                        return new NumberExpression(number);
                    }
            }
        }
        private BracketExpression BracketNode()
        {
            Token openBracket = Match(TokenKind.OpenBracket);
            NodeExpression nodeExpression = StartNode();
            Token closedBracket = Match(TokenKind.ClosedBracket);
            return new BracketExpression(openBracket, nodeExpression, closedBracket);
        }
    }
}
