using KA_LAB3.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3
{
    class UnbuilderExpressionTree
    {
        public UnbuilderExpressionTree(NodeExpression root)
        {
            Root = root;
        }

        public NodeExpression Root { get; }

        public string Unbuild()
        {
            StringBuilder sb = new StringBuilder();
            sb = Root.ToString(sb);
            string str=sb.ToString();
            return str;
        }
    }
}
