using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.Expression
{
    class SignExpression:UnaryExpression
    {
        public SignExpression(Token sign, NodeExpression node)
        {
            Sign = sign;
            Node = node;
        }
        public override ExpressionKind Kind => ExpressionKind.Sign;

        public Token Sign{ get; }
        public NodeExpression Node { get; }
    }
}
