using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.Expression
{
    class VarExpression : NodeExpression
    {
        public VarExpression(Token token)
        {
            Token = token;
        }
        public override ExpressionKind Kind => ExpressionKind.Var;
        public Token Token { get; }
    }
}
