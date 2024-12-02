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
            Text = ToString(sb).ToString();
        }

        public override ExpressionKind Kind => ExpressionKind.Literal;
        public Token Token { get; }

        public override string Text { get; set; }
        public override StringBuilder ToString(StringBuilder sb)
        {
            sb.Append(Token.Value);
            return sb;
        }
    }
}
