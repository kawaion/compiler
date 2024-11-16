using KA_LAB3.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.MyToken
{
    internal class TokenMerger
    {
        private readonly List<string> _vars;
        public TokenMerger(List<string> vars=null)
        {
            _vars = vars;
        }
        public List<Token> Merge(List<Token> tokens)
        {
            List<Token> resTokens = new List<Token>();
            for (int i = 0; i < tokens.Count; i++)
            {
                Token token = tokens[i];
                if (token.Kind == TokenKind.Number)
                {
                    resTokens.Add(MergeDigit(tokens, ref i));
                }
                else if (token.Kind == TokenKind.Char)
                {
                    resTokens.Add(MergeChar(tokens, ref i));
                }
                else
                    if (token.Kind != TokenKind.Space)
                    resTokens.Add(token);
            }
            return resTokens;
        }

        private Token MergeDigit(List<Token> tokens, ref int i)
        {
            Token token = tokens[i];
            Token resToken = null;
            int startPosition = i;
            while (token.Kind == TokenKind.Number & token.Kind != TokenKind.End)
            {
                i++;
                token = tokens[i];
            }
            int endPosition = i;
            int count = endPosition - startPosition;
            var digitTokens = tokens.GetRange(startPosition, count);
            string number = MergeToString(digitTokens);
            if (int.TryParse(number, out int value))
            {
                resToken = new Token(TokenKind.Number, value);
            }
            else
                resToken = new Token(TokenKind.Bad, null);
            ErrorWriting.IsVarStartsWithNumber(token, number);
            i--;
            return resToken;
        }
        private Token MergeChar(List<Token> tokens, ref int i)
        {
            Token token = tokens[i];
            Token resToken = null;
            int startPosition = i;
            while ((token.Kind == TokenKind.Number || token.Kind == TokenKind.Char) & token.Kind != TokenKind.End)
            {
                i++;
                token = tokens[i];
            }
            int endPosition = i;
            int count = endPosition - startPosition;
            var CharTokens = tokens.GetRange(startPosition, count);
            string word = MergeToString(CharTokens);
            if (_vars.Contains(word))
            {
                resToken = new Token(TokenKind.Var, word);
            }
            else if (IsTokenFunction(word))
            {
                resToken = new Token(TokenKind.function, word);
            }
            else
                resToken = new Token(TokenKind.Bad, null);
            i--;
            return resToken;
        }

        private bool IsTokenFunction(string word)
        {
            foreach (string name in Enum.GetNames(typeof(TokenFunction)))
            {
                if (word == name)
                    return true;
            }
            return false;
        }
        private string MergeToString(List<Token> tokens)
        {
            StringBuilder sb = new StringBuilder();
            foreach (Token token in tokens)
            {
                sb.Append(token.Value.ToString());
            }
            return sb.ToString();
        }
    }
}
