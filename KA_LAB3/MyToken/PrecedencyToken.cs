using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.MyToken
{
    class PrecedencyToken
    {
        
        public int GetLiteral(Token token)
        {
            switch (token.Kind)
            {

                case TokenKind.Number:
                case TokenKind.Var:
                    return 9;
                default:
                    return 0;
            }
        }
        
        public int GetNodeExpression(Token token)
        {
            switch (token.Kind)
            {

                case TokenKind.Node:
                    return 8;
                default:
                    return 0;
            }
        }
        public int GetBracket(Token token)
        {
            switch (token.Kind)
            {
                case TokenKind.OpenBracket:
                    return 7;
                default:
                    return 0;
            }
        }
        public int GetUnary(Token token)
        {
            switch (token.Kind)
            {

                case TokenKind.Function:
                case TokenKind.Plus:
                case TokenKind.Minus:
                    return 6;
                default:
                    return 0;
            }
        }
        public int GetDiff(Token token)
        {
            switch (token.Kind)
            {
                case TokenKind.Apostrophe:
                    return 5;
                default:
                    return 0;
            }
        }
        public int GetBinary(Token token)
        {
            switch (token.Kind)
            {
                
                case TokenKind.Caret:
                    return 4;
                case TokenKind.Star:
                case TokenKind.Slash:
                    return 3;
                case TokenKind.Plus:
                case TokenKind.Minus:
                    return 2;
                default:
                    return 0;
            }
        }
        public int GetSet(Token token)
        {
            switch (token.Kind)
            {
                case TokenKind.Comma:
                    return 1;
                default:
                    return 0;
            }
        }
        

    }
}
