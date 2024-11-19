using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.Error
{
    static class ErrorWriting
    {
        public static bool ShowBadToken(Token token)
        {
            throw new Exception($"ошибка на позиции {token.Position} из-за {(TokenKind)token.Value}");
        }
        internal static void IsVarStartsWithNumber(Token token, string number)
        {
            if (token.Kind == TokenKind.Char)
            {
                throw new Exception($"пременная начинается с числа {number}{token.Value} на позиции {token.Position}");
            }
        }
    }
}
