using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.MyToken
{
    internal class SingleTokenLexer
    {
        private string _text;
        public SingleTokenLexer(string text)
        {
            _text = text;
        }
        public List<Token> Split()
        {
            var tokens = new List<Token>();
            int i = 0;
            foreach (var symbol in _text)
            {
                switch (symbol)
                {
                    case ' ': tokens.Add(new Token(TokenKind.Space, symbol,i)); break;
                    case '+': tokens.Add(new Token(TokenKind.Plus, symbol, i)); break;
                    case '-': tokens.Add(new Token(TokenKind.Minus, symbol, i)); break;
                    case '*': tokens.Add(new Token(TokenKind.Star, symbol, i)); break;
                    case '/': tokens.Add(new Token(TokenKind.Slash, symbol, i)); break;
                    case '^': tokens.Add(new Token(TokenKind.Caret, symbol, i)); break;
                    case '(': tokens.Add(new Token(TokenKind.OpenBracket, symbol, i)); break;
                    case ')': tokens.Add(new Token(TokenKind.ClosedBracket, symbol, i)); break;
                    case '.': tokens.Add(new Token(TokenKind.Dot, symbol, i)); break;
                    case ',': tokens.Add(new Token(TokenKind.Comma, symbol, i)); break;
                    case '\'': tokens.Add(new Token(TokenKind.Apostrophe, symbol, i)); break;
                    default: tokens.Add(GetTokenWithDifferentValues(symbol,i)); break;
                }
                i++;
            }
            tokens.Add(new Token(TokenKind.End, "\0",i));
            return tokens;
        }
        private Token GetTokenWithDifferentValues(char symbol,int position)
        {
            if (char.IsDigit(symbol))
                return new Token(TokenKind.Number, symbol,position);
            if (char.IsLetter(symbol))
                return new Token(TokenKind.Char, symbol, position);
            return new Token(TokenKind.Bad, null,position);
        }

    }
}
