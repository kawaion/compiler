using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.Expression
{
    class bracketExpression:UnaryExpression
    {
        public bracketExpression(NodeExpression node)
        {
            Node = node;
        }
        public override ExpressionKind Kind => ExpressionKind.Bracket;
        public NodeExpression Node { get; }
    }
}
