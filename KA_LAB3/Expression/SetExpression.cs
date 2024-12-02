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
        public override ExpressionKind Kind => ExpressionKind.Comma;
        public void Add(NodeExpression node)
        {
            nodes.Add(node);
        }
        public List<NodeExpression> GetNodes()
        {
            return nodes;
        }
        //
        public override StringBuilder ToString(StringBuilder sb)
        {
            foreach(var node in nodes)
            {
                sb=node.ToString(sb);
                sb.Append(",");
            }
            sb.Remove(sb.Length - 1,1);
            return sb;
        }

        
    }
}
