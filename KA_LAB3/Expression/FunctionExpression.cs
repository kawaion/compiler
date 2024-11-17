using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.Expression
{
    class FunctionExpression : UnaryExpression
    {
        public FunctionExpression(Token function,NodeExpression node)
        {
            Function = function;
            Node = node;
        }
        public override ExpressionKind Kind => ExpressionKind.Function;

        public Token Function { get; }
        public NodeExpression Node { get; }
    }
}
