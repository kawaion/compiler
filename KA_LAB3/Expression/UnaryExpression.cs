using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.Expression
{
    class UnaryExpression : NodeExpression
    {
        public UnaryExpression(Token tokenOperator,NodeExpression node)
        {
            TokenOperator = tokenOperator;
            Node = node;
        }
        public override ExpressionKind Kind => ExpressionKind.Unary;

        public Token TokenOperator { get; }
        public NodeExpression Node { get; }

        public override StringBuilder ToString(StringBuilder sb)
        {
            sb.Append(TokenOperator.Value);
            sb=Node.ToString(sb);
            return sb;
        }
    }
}
