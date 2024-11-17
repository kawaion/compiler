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
        public override ExpressionKind Kind => ExpressionKind.Number;
        public Token Token { get; }
    }
}
