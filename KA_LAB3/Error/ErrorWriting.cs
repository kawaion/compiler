using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.Error
{
    static class ErrorWriting
    {
        public static bool IsNumberOpenAndClosedBrackets(List<Token> tokens)
        {
            int c = 0;
            foreach (Token token in tokens)
            {
                if (token.Kind == TokenKind.OpenBracket) c++;
                if (token.Kind == TokenKind.ClosedBracket) c--;
            }
            if(c != 0)
            {
                if (c > 0)
                    throw new Exception("лишнее количество открытых скобок");
                else
                    throw new Exception("лишнее количество закрытых скобок");
            }
            return true;
        }

        internal static void IsTwoSignInRow(List<Token> tokens)
        {
            List<TokenKind> signTokens = new List<TokenKind>() 
            {   TokenKind.Plus,
                TokenKind.Minus,
                TokenKind.Star,
                TokenKind.Slash,
                TokenKind.Caret,
                TokenKind.Dot
            };
            for(int i = 0; i < tokens.Count-1; i++)
            {
                if (signTokens.Contains(tokens[i].Kind) & (signTokens.Contains(tokens[i+1].Kind)))
                {
                    throw new Exception($"два знака поряд {tokens[i].Value}{tokens[i+1].Value}"); ; 
                }
            }
        }

        internal static void IsVarStartsWithNumber(Token token, string number)
        {
            if (token.Kind == TokenKind.Char)
            {
                throw new Exception($"пременная начинается с числа {number}{token.Value}");
            }
        }
    }
}
