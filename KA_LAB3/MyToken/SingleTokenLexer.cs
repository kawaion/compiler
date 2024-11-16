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
            foreach (var symbol in _text)
            {
                switch (symbol)
                {
                    case ' ': tokens.Add(new Token(TokenKind.Space, symbol)); break;
                    case '+': tokens.Add(new Token(TokenKind.Plus, symbol)); break;
                    case '-': tokens.Add(new Token(TokenKind.Minus, symbol)); break;
                    case '*': tokens.Add(new Token(TokenKind.Star, symbol)); break;
                    case '/': tokens.Add(new Token(TokenKind.Slash, symbol)); break;
                    case '^': tokens.Add(new Token(TokenKind.Caret, symbol)); break;
                    case '(': tokens.Add(new Token(TokenKind.OpenBracket, symbol)); break;
                    case ')': tokens.Add(new Token(TokenKind.ClosedBracket, symbol)); break;
                    case '.': tokens.Add(new Token(TokenKind.Dot, symbol)); break;
                    default: tokens.Add(GetTokenWithDifferentValues(symbol)); break;
                }
            }
            tokens.Add(new Token(TokenKind.End, "\0"));
            return tokens;
        }
        private Token GetTokenWithDifferentValues(char symbol)
        {
            if (char.IsDigit(symbol))
                return new Token(TokenKind.Number, symbol);
            if (char.IsLetter(symbol))
                return new Token(TokenKind.Char, symbol);
            throw new Exception($"неициализированный символ {symbol}");
            return new Token(TokenKind.Bad, null);
        }

    }
}
