using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.Expression
{
    class UnaryExpression : NodeExpression
    {
        public override ExpressionKind Kind => ExpressionKind.Unary;
        //public NodeExpression Expression { get; }
    }
}
