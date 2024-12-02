using KA_LAB3.Derivative;
using KA_LAB3.Expression;
using KA_LAB3.MyToken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3
{
    internal class BuilderExpressionTree
    {
        private readonly List<Token> _tokens;
        private int _position;

        private PrecedencyToken precedencyToken=new PrecedencyToken();
        public BuilderExpressionTree(List<Token> tokens)
        {
            _tokens = tokens;
            _position = 0;
        }
        private Token Current => Peek(0);
        private Token Peek(int offset) => _tokens[_position + offset];
        private Token NextToken()
        {
            var token = Current;
            _position++;
            return token;
        }
        int currentPrecedency;
        private Token Match(TokenKind kind)
        {
            if(Current.Kind == kind)
                return NextToken();
            return new Token(TokenKind.Bad, Current.Kind, Current.Position);
        }
        public NodeExpression Build()
        {
            return GetRoot();
        }
        private NodeExpression GetRoot()
        {
            return GetBranch();
        }
        private NodeExpression GetBranch(int previousPrecedency = 0)
        {
            NodeExpression leftNode = new BadExpression();
            
            currentPrecedency = precedencyToken.GetUnary(Current);
            if (currentPrecedency != 0 & currentPrecedency >= previousPrecedency)
            {
                Token tokenOperator = NextToken();
                NodeExpression rightNode = GetBranch(currentPrecedency);
                leftNode = new UnaryExpression(tokenOperator, rightNode);
            }

            currentPrecedency = precedencyToken.GetLiteral(Current);
            if (currentPrecedency != 0 & currentPrecedency >= previousPrecedency)
            {
                Token literal = NextToken();
                leftNode = new LiteralExpression(literal);
            }

            currentPrecedency = precedencyToken.GetNodeExpression(Current);
            if (currentPrecedency != 0 & currentPrecedency >= previousPrecedency)
            {
                Token node = NextToken();
                leftNode = (NodeExpression)node.Value;
            }

            currentPrecedency = precedencyToken.GetBracket(Current);
            if (currentPrecedency != 0 & currentPrecedency >= previousPrecedency)
            {
                leftNode = BracketNode();
            }
            
            

            while (true)
            {
                currentPrecedency = precedencyToken.GetDiff(Current);
                if (currentPrecedency == 0 || currentPrecedency < previousPrecedency)
                    break;
                Token tokenOperator = NextToken();
                Token rightNode = Match(TokenKind.Var);
                Differentiator differenciator = new Differentiator(leftNode, rightNode);
                leftNode = differenciator.Differentiate();
            }
            while (true)
            {
                currentPrecedency = precedencyToken.GetBinary(Current);
                if (currentPrecedency == 0 || currentPrecedency < previousPrecedency)
                    break;
                Token tokenOperator = NextToken();
                NodeExpression rightNode = GetBranch(currentPrecedency);
                leftNode = new BinaryExpression(leftNode, tokenOperator, rightNode);
            }
            SetExpression set = new SetExpression(leftNode);
            while (true)
            {
                currentPrecedency = precedencyToken.GetSet(Current);
                if (currentPrecedency == 0 || currentPrecedency < previousPrecedency)
                    break;
                NextToken();
                NodeExpression rightNode = GetBranch(currentPrecedency+1);
                set.Add(rightNode);
                leftNode = set;
            }
            
            return leftNode;
        }
        //private NodeExpression LiteralForUnary(int precedency)
        //{
        //    switch (Current.Kind)
        //    {
        //        case TokenKind.Var:
        //        case TokenKind.Number:
        //            return new LiteralExpression(NextToken());
        //        case TokenKind.OpenBracket: return BracketNode();
        //        default: return GetBranch(precedency);
        //    }
        //}
        private BracketExpression BracketNode()
        {
            Token openBracket = Match(TokenKind.OpenBracket);
            NodeExpression nodeExpression = GetBranch();
            Token closedBracket = Match(TokenKind.ClosedBracket);
            return new BracketExpression(openBracket, nodeExpression, closedBracket);
        }
    }
}
