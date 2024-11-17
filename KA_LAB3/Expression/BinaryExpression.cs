using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.Expression
{
    internal class BinaryExpression : NodeExpression
    {
        public BinaryExpression(NodeExpression left,Token operatorToken,NodeExpression right)
        {
            Left = left;
            OperatorToken = operatorToken;
            Right = right;
        }
        public override ExpressionKind Kind => ExpressionKind.Binary;
        public NodeExpression Left { get; }
        public Token OperatorToken { get; }
        public NodeExpression Right { get; }

        
    }
}
