using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.Expression
{
    class BracketExpression:UnaryExpression
    {
        public BracketExpression(Token openBracket,NodeExpression node,Token closedBracet)
        {
            OpenBracket = openBracket;
            Node = node;
            ClosedBracet = closedBracet;
        }
        public override ExpressionKind Kind => ExpressionKind.Bracket;

        public Token OpenBracket { get; }
        public NodeExpression Node { get; }
        public Token ClosedBracet { get; }
    }
}
