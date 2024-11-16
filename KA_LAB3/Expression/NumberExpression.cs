using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.Expression
{
    internal class NumberExpression : NodeExpression
    {
        public NumberExpression(Token token)
        {
            Token = token;
        }
        public override TokenKind Kind => TokenKind.NumberExpression;
        public Token Token { get; }
    }
}
