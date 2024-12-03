using KA_LAB3.Error;
using KA_LAB3.Expression;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace KA_LAB3.Derivative
{
    class Differentiator
    {
        private NodeExpression _node;
        private string _nameVar;
        private bool isContainsX=false;

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
                                return DiffAdd(leftRoot, sign, rightRoot);
                            case TokenKind.Star: return DiffMulti(leftRoot,sign,rightRoot);
                            case TokenKind.Slash: return DiffDivide(leftRoot, sign, rightRoot);
                            case TokenKind.Caret: return DiffPow(leftRoot, sign, rightRoot);
                            default:
                                ErrorWriting.ShowBadToken(sign);
                                return new BadExpression();
                        }
                    }
                case ExpressionKind.Bracket:
                    {
                        BracketExpression bracketExpression = (BracketExpression)root;
                        return new BracketExpression(bracketExpression.OpenBracket,DiffRoot(bracketExpression.Node),bracketExpression.ClosedBracet);
                    }
                case ExpressionKind.Literal:
                    {
                        LiteralExpression literalExpression = (LiteralExpression)root;
                        Token token = literalExpression.Token;
                        if (token.Kind == TokenKind.Var)
                            if (IsVarDiffVar(token))
                            {
                                isContainsX = true;
                                return GetLiteral(1, token.Position);
                            }
                        if (token.Kind == TokenKind.Number)
                            return GetLiteral(0, token.Position);
                        ErrorWriting.ShowBadToken(token);
                        return new BadExpression();
                    }
                case ExpressionKind.Unary:
                    {
                        UnaryExpression unaryExpression = (UnaryExpression)root;
                        Token tokenOperator = unaryExpression.TokenOperator;
                        NodeExpression node = unaryExpression.Node;
                        if (tokenOperator.Kind == TokenKind.Plus)
                            return new UnaryExpression(tokenOperator,DiffRoot(node));
                        if (tokenOperator.Kind == TokenKind.Minus)
                            return new UnaryExpression(tokenOperator, DiffRoot(node));
                        if (tokenOperator.Kind == TokenKind.Function)
                            return DiffFunction(tokenOperator, node);
                        ErrorWriting.ShowBadToken(tokenOperator);
                        return new BadExpression();
                    }
                default:
                    throw new Exception($"неизвестное выражение {root.Kind}");
            }
        }
        private NodeExpression DiffAdd(NodeExpression u, Token sign, NodeExpression v)
        {
            (var du, var dv, var isUx, var isVx) = dudv(u, v);
            if (isUx & isVx)
            {
                return BuildTreeNodeExpression($"du{sign.Value}dv", new Dictionary<string, NodeExpression>() { ["du"] = du, ["dv"] = dv, });
            }
            if (isUx & !isVx)
            {
                return du;
            }
            if (!isUx & isVx)
            {
                return dv;
            }
            if (!isUx & !isVx)
            {
                return GetLiteral(0);
            }

            return new BadExpression();
        }
        private NodeExpression DiffMulti(NodeExpression u,Token star, NodeExpression v)
        {
            (var du, var dv, var isUx, var isVx) = dudv(u, v);
            if (isUx & isVx)
            {
                return BuildTreeNodeExpression("du*v+u*dv", new Dictionary<string, NodeExpression>() { ["u"] = u, ["v"] = v, ["du"] = du, ["dv"] = dv, });
            }
            if (isUx & !isVx)
            {
                return BuildTreeNodeExpression("du*v", new Dictionary<string, NodeExpression>() { ["v"] = v, ["du"] = du, });
            }
            if (!isUx & isVx)
            {
                return BuildTreeNodeExpression("u*dv", new Dictionary<string, NodeExpression>() { ["u"] = u,  ["dv"] = dv, });
            }
            if (!isUx & !isVx)
            {
                return GetLiteral(0);
            }

            return new BadExpression();
        }
        private NodeExpression DiffDivide(NodeExpression u, Token slash, NodeExpression v)
        {
            (var du, var dv, var isUx, var isVx) = dudv(u, v);
            if (isUx & isVx)
            {
                return BuildTreeNodeExpression("(du*v-u*dv)/v^2", new Dictionary<string, NodeExpression>() { ["u"] = u, ["v"] = v, ["du"] = du, ["dv"] = dv, });
            }
            if (isUx & !isVx)
            {
                return BuildTreeNodeExpression("du*v/v^2", new Dictionary<string, NodeExpression>() { ["v"] = v, ["du"] = du, });
            }
            if (!isUx & isVx)
            {
                return BuildTreeNodeExpression("u*dv/v^2", new Dictionary<string, NodeExpression>() { ["u"] = u, ["v"] = v, ["dv"] = dv, });
            }
            if (!isUx & !isVx)
            {
                return GetLiteral(0);
            }

            return new BadExpression();
        }
        private NodeExpression DiffPow(NodeExpression u, Token caret, NodeExpression v)
        {
            (var du, var dv, var isUx, var isVx) = dudv(u, v);
            if (isUx & isVx)
            {
                NodeExpression lnu = BuildTreeNodeExpression("ln(u)", new Dictionary<string, NodeExpression>() { ["u"] = u });
                NodeExpression dlnuv = DiffMulti(lnu, new Token(TokenKind.Star, "*", caret.Position), v);
                return BuildTreeNodeExpression("u^v*dlnuv", new Dictionary<string, NodeExpression>() { ["u"] = u, ["v"] = v, ["dlnuv"] = dlnuv });
            }
            if (isUx & !isVx)
            {
                return BuildTreeNodeExpression("v*u^(v-1)*du", new Dictionary<string, NodeExpression>() { ["u"] = u, ["v"] = v, ["du"] = du, });
            }
            if (!isUx & isVx)
            {
                return BuildTreeNodeExpression("u^v*ln(u))", new Dictionary<string, NodeExpression>() { ["u"] = u, ["v"] = v});
            }
            if (!isUx & !isVx)
            {
                return GetLiteral(0);
            }

            return new BadExpression();
        }
        private NodeExpression DiffFunction(Token tokenOperator,NodeExpression bracket)
        {
            string nameFunction = (string)tokenOperator.Value;
            NodeExpression x = ((BracketExpression)bracket).Node;
            if (bracket.Kind != ExpressionKind.Set)
            {
                NodeExpression dx = DiffRoot(x);
                if (true)
                {
                    if (nameFunction == "arctg")
                        return BuildTreeNodeExpression("1/(1+x^2)*dx", new Dictionary<string, NodeExpression>() { ["x"] = x, ["dx"] = dx });
                    if (nameFunction == "sin")
                        return BuildTreeNodeExpression("cos(x)*dx", new Dictionary<string, NodeExpression>() { ["x"] = x, ["dx"] = dx });
                    if (nameFunction == "cos")
                        return BuildTreeNodeExpression("-sin(x)*dx", new Dictionary<string, NodeExpression>() { ["x"] = x, ["dx"] = dx });
                    if (nameFunction == "tg")
                        return BuildTreeNodeExpression("1/cos(x)^2*dx", new Dictionary<string, NodeExpression>() { ["x"] = x, ["dx"] = dx });
                    if (nameFunction == "ctg")
                        return BuildTreeNodeExpression("-1/sin(x)^2*dx", new Dictionary<string, NodeExpression>() { ["x"] = x, ["dx"] = dx });
                    if (nameFunction == "ln")
                        return BuildTreeNodeExpression("1/x*dx", new Dictionary<string, NodeExpression>() { ["x"] = x, ["dx"] = dx });
                }
                else
                    return GetLiteral(0);
            }
            else
            {
                SetExpression set = (SetExpression)x;
                var nodes=set.GetNodes();
                if (nameFunction == "log")
                {
                    var a = nodes[1];
                    NodeExpression xForLog = nodes[0];
                    NodeExpression dx = DiffRoot(xForLog);
                    return BuildTreeNodeExpression("1/(xForLog*ln(a))*dx", new Dictionary<string, NodeExpression>() { ["xForLog"] = xForLog, ["dx"] = dx, ["a"] = a });
                }
            }
            return new BadExpression();
        }
        private NodeExpression BuildTreeNodeExpression(string expression, Dictionary<string, NodeExpression> nodes)
        {
            Lexer lexer = new Lexer(expression);
            lexer.SetNodeExpressions(nodes);
            var tokens = lexer.Tokenisation();
            BuilderExpressionTree builder = new BuilderExpressionTree(tokens);
            return builder.Build();
        }
        private (NodeExpression du,NodeExpression dv , bool isUx, bool isVx) dudv(NodeExpression u, NodeExpression v)
        {
            isContainsX = false;
            NodeExpression du = DiffRoot(u);
            bool isUx = isContainsX;

            isContainsX = false;
            NodeExpression dv = DiffRoot(v);
            bool isVx = isContainsX;

            return (du,dv,isUx,isVx);
        }
        private NodeExpression GetLiteral(double number,int tokenPosition=0)
        {
            return new LiteralExpression(new Token(TokenKind.Number, number, tokenPosition));
        }

    }
}
