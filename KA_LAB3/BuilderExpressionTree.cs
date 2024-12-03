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

            leftNode = BuildUnaryExpressionIfTokenSuitable(previousPrecedency, leftNode);

            leftNode = BuildLiteralExpressionIfTokenSuitable(previousPrecedency, leftNode);

            leftNode = BuildNodeExpressionIfTokenSuitable(previousPrecedency, leftNode);

            leftNode = BuildBracketExpressionIfTokenSuitable(previousPrecedency, leftNode);

            leftNode = DiffNodeExpressionIfTokenSuitable(previousPrecedency, leftNode);

            leftNode = BuildBinaryExpressionIfTokenSuitable(previousPrecedency, leftNode);

            leftNode = BuildSetExpressionIfTokenSuitable(previousPrecedency, leftNode);

            return leftNode;
        }

        private NodeExpression BuildSetExpressionIfTokenSuitable(int previousPrecedency, NodeExpression leftNode)
        {
            SetExpression set = new SetExpression(leftNode);
            while (true)
            {
                currentPrecedency = precedencyToken.GetSet(Current);
                if (currentPrecedency == 0 || currentPrecedency < previousPrecedency)
                    break;
                NextToken();
                NodeExpression rightNode = GetBranch(currentPrecedency + 1);
                set.Add(rightNode);
                leftNode = set;
            }

            return leftNode;
        }

        private NodeExpression BuildBinaryExpressionIfTokenSuitable(int previousPrecedency, NodeExpression leftNode)
        {
            while (true)
            {
                currentPrecedency = precedencyToken.GetBinary(Current);
                if (currentPrecedency == 0 || currentPrecedency < previousPrecedency)
                    break;
                Token tokenOperator = NextToken();
                NodeExpression rightNode = GetBranch(currentPrecedency);
                leftNode = new BinaryExpression(leftNode, tokenOperator, rightNode);
            }

            return leftNode;
        }

        private NodeExpression DiffNodeExpressionIfTokenSuitable(int previousPrecedency, NodeExpression leftNode)
        {
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

            return leftNode;
        }

        private NodeExpression BuildBracketExpressionIfTokenSuitable(int previousPrecedency, NodeExpression leftNode)
        {
            currentPrecedency = precedencyToken.GetBracket(Current);
            if (currentPrecedency != 0 & currentPrecedency >= previousPrecedency)
            {
                leftNode = BracketNode();
            }

            return leftNode;
        }

        private NodeExpression BuildNodeExpressionIfTokenSuitable(int previousPrecedency, NodeExpression leftNode)
        {
            currentPrecedency = precedencyToken.GetNodeExpression(Current);
            if (currentPrecedency != 0 & currentPrecedency >= previousPrecedency)
            {
                Token node = NextToken();
                leftNode = (NodeExpression)node.Value;
            }

            return leftNode;
        }

        private NodeExpression BuildLiteralExpressionIfTokenSuitable(int previousPrecedency, NodeExpression leftNode)
        {
            currentPrecedency = precedencyToken.GetLiteral(Current);
            if (currentPrecedency != 0 & currentPrecedency >= previousPrecedency)
            {
                Token literal = NextToken();
                leftNode = new LiteralExpression(literal);
            }

            return leftNode;
        }

        private NodeExpression BuildUnaryExpressionIfTokenSuitable(int previousPrecedency, NodeExpression leftNode)
        {
            currentPrecedency = precedencyToken.GetUnary(Current);
            if (currentPrecedency != 0 & currentPrecedency >= previousPrecedency)
            {
                Token tokenOperator = NextToken();
                NodeExpression rightNode = GetBranch(currentPrecedency);
                leftNode = new UnaryExpression(tokenOperator, rightNode);
            }

            return leftNode;
        }

        private BracketExpression BracketNode()
        {
            Token openBracket = Match(TokenKind.OpenBracket);
            NodeExpression nodeExpression = GetBranch();
            Token closedBracket = Match(TokenKind.ClosedBracket);
            return new BracketExpression(openBracket, nodeExpression, closedBracket);
        }
    }
}
