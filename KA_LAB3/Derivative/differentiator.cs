using KA_LAB3.Error;
using KA_LAB3.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.Derivative
{
    class Differentiator
    {
        private NodeExpression _node;
        private string _nameVar;
        private bool isContainsVar;

        public Differentiator(NodeExpression node,Token dx)
        {
            _node = node;
            _nameVar = (string)dx.Value;
        }
        private bool IsVarDiffVar(Token token)
        {
            return (string)token.Value == _nameVar;
        }
        public NodeExpression Differentiate()
        {
            return DiffRoot(_node);
        }
        private NodeExpression DiffRoot(NodeExpression root)
        {
            switch (root.Kind)
            {
                case ExpressionKind.Binary:
                    {
                        BinaryExpression binaryExpression = (BinaryExpression)root;
                        Token sign = binaryExpression.OperatorToken;
                        NodeExpression leftRoot = binaryExpression.Left;
                        NodeExpression rightRoot = binaryExpression.Right;
                        switch (sign.Kind)
                        {
                            case TokenKind.Plus: 
                            case TokenKind.Minus:
                                return new BinaryExpression(DiffRoot(leftRoot), sign, DiffRoot(rightRoot));
                            case TokenKind.Star: return DiffMulti(leftRoot,sign,rightRoot);
                            case TokenKind.Slash: return DiffDivide(leftRoot, sign, rightRoot);
                            case TokenKind.Caret: return Math.Pow(leftRoot, rightRoot);
                            default:
                                ErrorWriting.ShowBadToken(sign);
                                return new BadExpression();
                        }

                    }
                    //\/
                case ExpressionKind.Bracket:
                    {
                        BracketExpression bracketExpression = (BracketExpression)root;
                        return new BracketExpression(bracketExpression.OpenBracket,DiffRoot(bracketExpression.Node),bracketExpression.ClosedBracet);
                    }
                    //\/
                case ExpressionKind.Literal:
                    {
                        LiteralExpression literalExpression = (LiteralExpression)root;
                        Token token = literalExpression.Token;
                        if (token.Kind == TokenKind.Var)
                            if (IsVarDiffVar(token))
                            {
                                isContainsVar = true;
                                return new LiteralExpression(new Token(TokenKind.Number, 1, token.Position));
                            }
                        if (token.Kind == TokenKind.Number)
                            return new LiteralExpression(new Token(TokenKind.Number, 0, token.Position));
                        ErrorWriting.ShowBadToken(token);
                        return new BadExpression();
                    }
                case ExpressionKind.Unary:
                    {
                        UnaryExpression unaryExpression = (UnaryExpression)root;
                        Token tokenOperator = unaryExpression.TokenOperator;
                        NodeExpression node = unaryExpression.Node;
                        if (tokenOperator.Kind == TokenKind.Plus)
                            return Evaluate(node);
                        if (tokenOperator.Kind == TokenKind.Minus)
                            return -Evaluate(node);
                        if (tokenOperator.Kind == TokenKind.Function)
                            return SolveFunction(tokenOperator, node);
                        ErrorWriting.ShowBadToken(tokenOperator);
                        return 0;
                    }
                default:
                    throw new Exception($"неизвестное выражение {root.Kind}");
            }
        }
        private NodeExpression DiffMulti(NodeExpression u,Token star, NodeExpression v)
        {
            NodeExpression du = DiffRoot(u);
            NodeExpression dv = DiffRoot(v);
            int position = star.Position;
            return new BinaryExpression(
                                        new BinaryExpression(du, star, v),
                                        new Token(TokenKind.Plus, "+", position),
                                        new BinaryExpression(u, star, dv)
                                        );
        }
        private NodeExpression DiffDivide(NodeExpression u, Token slash, NodeExpression v)
        {
            NodeExpression du = DiffRoot(u);
            NodeExpression dv = DiffRoot(v);
            int position = slash.Position;
            Token star = new Token(TokenKind.Star, "*", position);
            return new BinaryExpression(
                                        new BinaryExpression(
                                                            new BinaryExpression(du, star, v),
                                                            new Token(TokenKind.Plus, "-", position),
                                                            new BinaryExpression(u, star, dv)
                                                            ),
                                        slash,
                                        new BinaryExpression(
                                                            v,
                                                            new Token(TokenKind.Caret, "^", position),
                                                            new LiteralExpression(new Token(TokenKind.Number, 2, position))
                                                            )
                                        );
        }
        private NodeExpression DiffPow(NodeExpression u, Token caret, NodeExpression v)
        {
            isContainsVar = false;
            NodeExpression du = DiffRoot(u);
            int position = caret.Position;
            Token star = new Token(TokenKind.Star, "*", position);
            if (isContainsVar)
            {
                return GetNode("v*u^(v-1)*du", new Dictionary<string, NodeExpression>() { ["u"] = u, ["v"] = u, ["du"] = du, });
            }
            //if (isContainsVar)
            //{
            //    return new BinaryExpression
            //                                (
            //                                new BinaryExpression(
            //                                                    v,
            //                                                    star,
            //                                                    new BinaryExpression
            //                                                                        (u,
            //                                                                        caret,
            //                                                                        new BinaryExpression
            //                                                                                            (v,
            //                                                                                            new Token(TokenKind.Minus, "-", position),
            //                                                                                            new LiteralExpression(new Token(TokenKind.Number, 1, position))
            //                                                                                            )
            //                                                                        )
            //                                                    ),
            //                                star,
            //                                du
            //                                );
            //}
            //if (IsVarDiffVar(u))
            //{

            //}
        }

    }
}
