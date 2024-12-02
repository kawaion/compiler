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
        public override ExpressionKind Kind => ExpressionKind.Set;
        public void Add(NodeExpression node)
        {
            nodes.Add(node);
            sb = new StringBuilder();
            Text = ToString(sb).ToString();
        }
        public List<NodeExpression> GetNodes()
        {
            return nodes;
        }
        public override string Text { get; set; }
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
