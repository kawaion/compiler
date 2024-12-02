using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.Expression
{
    class BracketExpression:NodeExpression
    {
        public BracketExpression(Token openBracket,NodeExpression node,Token closedBracet)
        {
            OpenBracket = openBracket;
            Node = node;
            ClosedBracet = closedBracet;
            Text = ToString(sb).ToString();
        }
        public override ExpressionKind Kind => ExpressionKind.Bracket;

        public Token OpenBracket { get; }
        public NodeExpression Node { get; }
        public Token ClosedBracet { get; }

        public override string Text { get; set; }
        public override StringBuilder ToString(StringBuilder sb)
        {
            sb.Append(OpenBracket.Value);
            sb=Node.ToString(sb);
            sb.Append(ClosedBracet.Value);
            return sb;
        }
    }
}
