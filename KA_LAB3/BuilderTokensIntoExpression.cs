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
        //public NodeExpression Build()
        //{
        //    while(Current.Kind != TokenKind.End)
        //    {
        //        if (Current.Kind == TokenKind.OpenBracket)
        //        {
        //            int startPosition = _position + 1;
        //            while(Current.Kind != TokenKind.ClosedBracket)
        //            {
        //                NextToken();
        //            }
        //        }
        //    }
        //}

    }
}
