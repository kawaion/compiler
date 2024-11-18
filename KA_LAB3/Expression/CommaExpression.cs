using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.Expression
{
    internal class CommaExpression:NodeExpression
    {
        List<NodeExpression> nodes=new List<NodeExpression>();
        public CommaExpression(NodeExpression first)
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
