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
        private Token Current => _tokens[_position];
        private Token NextToken()
        {
            var token = Current;
            _position++;
            return token;
        }
        public NodeExpression Build()
        {
            NodeExpression leftNode = GetLeftNode();
            while (Current.Kind != TokenKind.End)
            {
                

            }
        }
        private NodeExpression GetLeftNode()
        {
            switch (Current.Kind)
            {
                case TokenKind.Plus:
                case TokenKind.Minus: return GetUnarySignNode();
                case TokenKind.OpenBracket: return GetBracketNode();
                case TokenKind.Number: return GetNumberNode();
                case TokenKind.Var:return GetVarNode();
                case TokenKind.Function: return GetFunctionNode();
                default: return new BadExpression();
            }
        }
        private NodeExpression GetBracketNode()
        {
            int startPosition = _position + 1;
            while (Current.Kind != TokenKind.ClosedBracket)
            {
                NextToken();
            }
            int endPosition = _position;
            int count = endPosition - startPosition;
            List<Token> tokensInBracket = _tokens.GetRange(startPosition, count);
            NodeExpression node = (new BuilderTokensIntoExpression(tokensInBracket)).Build();
            return new bracketExpression(node);
        }
        private NodeExpression GetUnarySignNode()
        {
            Token sign = Current;
            NextToken();
            NodeExpression node = GetLeftNode();
            return new SignExpression(sign, node);
        }
        private NodeExpression GetNumberNode()
        {
            return new NumberExpression(Current);
        }
        private NodeExpression GetVarNode()
        {
            return new VarExpression(Current);
        }
        private NodeExpression GetFunctionNode()
        {
            Token func = Current;
            NextToken();
            NodeExpression node = GetBracketNode();
            return new FunctionExpression(func, node);
        }

    }
}
