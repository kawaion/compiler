using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.Expression
{
    class LiteralExpression:NodeExpression
    {
        public LiteralExpression(Token token)
        {
            Token = token;
        }

        public override ExpressionKind Kind => ExpressionKind.Literal;
        public Token Token { get; }
    }
}
