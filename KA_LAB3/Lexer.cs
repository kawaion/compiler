using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KA_LAB3.Error;
using KA_LAB3.Expression;
using KA_LAB3.MyToken;

namespace KA_LAB3
{
    internal class Lexer
    {
        private readonly string _text;
        private readonly List<string> _vars;
        private Dictionary<string, NodeExpression> nodes=null;

        public Lexer(string text, List<string> vars=null)
        {
            _text = text;
            _vars = vars;
        }
        public void SetNodeExpressions(Dictionary<string, NodeExpression> dict)
        {
            nodes = dict;
        }
        public List<Token> Tokenisation()
        {
            var tokens = new SingleTokenLexer(_text).Split();
            TokenMerger tokenMerger = new TokenMerger(tokens, _vars);
            tokenMerger.SetNodeExpressions(nodes);
            tokens = tokenMerger.Merge();
            return tokens;
        }
    }
}
