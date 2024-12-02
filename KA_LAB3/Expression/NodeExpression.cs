using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.Expression
{
    abstract internal class NodeExpression
    {
        public abstract ExpressionKind Kind { get;}
        public abstract StringBuilder ToString(StringBuilder sb);
    }
}
