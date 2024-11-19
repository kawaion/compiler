using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KA_LAB3.Error;
using KA_LAB3.MyToken;

namespace KA_LAB3
{
    internal class Lexer
    {
        private readonly string _text;
        private readonly List<string> _vars;

        public Lexer(string text, List<string> vars=null)
        {
            _text = text;
            _vars = vars;
        }
        
        public List<Token> Tokenisation()
        {
            var tokens = new SingleTokenLexer(_text).Split();
            tokens = new TokenMerger(tokens,_vars).Merge();
            return tokens;
        }
    }
}
