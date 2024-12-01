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
                NodeExpression rightNode = GetBranch();
                leftNode = new UnaryExpression(tokenOperator, rightNode);
            }
            
            currentPrecedency = precedencyToken.GetLiteral(Current);
            if (currentPrecedency != 0 & currentPrecedency >= previousPrecedency)
            {
                Token literal = NextToken();
                leftNode = new LiteralExpression(literal);
            }

            currentPrecedency = precedencyToken.GetBracket(Current);
            if (currentPrecedency != 0 & currentPrecedency >= previousPrecedency)
            {
                leftNode = BracketNode();
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
        //private NodeExpression CommaNode()
        //{
        //    NodeExpression leftNode = TermNode();
        //    CommaExpression comma = new CommaExpression(leftNode);
        //    while (Current.Kind == TokenKind.Comma)
        //    {
        //        NextToken();
        //        NodeExpression rightNode = TermNode();
        //        comma.Add(rightNode);
        //        leftNode = comma;
        //    }
        //    return leftNode;
        //}
        //private NodeExpression TermNode()
        //{
        //    NodeExpression leftNode = FactorNode();
        //    while (Current.Kind == TokenKind.Plus ||
        //           Current.Kind == TokenKind.Minus)
        //    {
        //        Token sign = NextToken();
        //        NodeExpression rightNode = FactorNode();
        //        leftNode = new BinaryExpression(leftNode, sign, rightNode);
        //    }
        //    return leftNode;
        //}
        //private NodeExpression FactorNode()
        //{
        //    NodeExpression leftNode = PowNode();
        //    while (Current.Kind == TokenKind.Star ||
        //           Current.Kind == TokenKind.Slash)
        //    {
        //        Token sign = NextToken();
        //        NodeExpression rightNode = PowNode();
        //        leftNode = new BinaryExpression(leftNode, sign, rightNode);
        //    }
        //    return leftNode;
        //}
        //private NodeExpression PowNode() 
        //{
        //    NodeExpression leftNode = DoubleNode();
        //    while (Current.Kind == TokenKind.Caret)
        //    {
        //        Token pow = NextToken();
        //        NodeExpression rightNode = DoubleNode();
        //        leftNode = new BinaryExpression(leftNode, pow,rightNode);
        //    }
        //    return leftNode;
        //}
        //private NodeExpression DoubleNode()
        //{
        //    NodeExpression leftNode = PrimaryNode();
        //    while (Current.Kind == TokenKind.Dot)
        //    {
        //        Token dot = NextToken();
        //        NodeExpression rightNode = PrimaryNode();
        //        leftNode = new BinaryExpression(leftNode, dot, rightNode);
        //    }
        //    return leftNode;
        //}
        //private NodeExpression PrimaryNode()
        //{
        //    switch (Current.Kind)
        //    {

        //        case TokenKind.Var: return new VarExpression(NextToken());

        //        case TokenKind.OpenBracket: return BracketNode();

        //        default:
        //            {
        //                Token number = Match(TokenKind.Number);
        //                return new NumberExpression(number);
        //            }
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
