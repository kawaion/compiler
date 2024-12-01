using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.Expression
{
    internal class SetExpression:NodeExpression
    {
        List<NodeExpression> nodes=new List<NodeExpression>();
        public SetExpression(NodeExpression first)
        {
            nodes.Add(first);
        }
        public void Add(NodeExpression node)
        {
            nodes.Add(node);
        }
        public List<NodeExpression> GetNodes()
        {
            return nodes;
        }
        public override ExpressionKind Kind => ExpressionKind.Comma;
    }
}
