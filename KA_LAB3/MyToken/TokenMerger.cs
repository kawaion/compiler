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
        private readonly List<Token> _tokens;
        private readonly List<string> _vars = new List<string>();
        private int _position;
        public TokenMerger(List<Token> tokens,List<string> vars=null)
        {
            _tokens = tokens;
            _vars = vars;
            _position = 0;
        }
        private Token Current => _tokens[_position];
        private Token NextToken()
        {
            var token = Current;
            _position++;
            return token;
        }
        public List<Token> Merge()
        {
            List<Token> resTokens = new List<Token>();
            while (Current.Kind != TokenKind.End)
            {
                if (Current.Kind == TokenKind.Number)
                {
                    resTokens.Add(MergeDigit());
                }
                else if (Current.Kind == TokenKind.Char)
                {
                    resTokens.Add(MergeChar());
                }
                else
                {
                    if (Current.Kind != TokenKind.Space)
                        resTokens.Add(Current);
                    NextToken();
                }
            }
            resTokens.Add(new Token(TokenKind.End, "\0",_position));
            return resTokens;             
        }
            
        

        private Token MergeDigit()
        {
            Token token;

            int startPosition = _position;
            while (Current.Kind == TokenKind.Number & Current.Kind != TokenKind.End)
            {
                NextToken();
            }
            int endPosition = _position;
            int count = endPosition - startPosition;
            var digitTokens = _tokens.GetRange(startPosition, count);
            string number = MergeToString(digitTokens);
            if (int.TryParse(number, out int value))
            {
                token = new Token(TokenKind.Number, value, _tokens[startPosition].Position);
            }
            else
                token = new Token(TokenKind.Bad, null, _tokens[startPosition].Position);
            ErrorWriting.IsVarStartsWithNumber(Current, number);
            return token;
        }
        private Token MergeChar()
        {
            Token token;
            int startPosition = _position;
            while ((Current.Kind == TokenKind.Number || Current.Kind == TokenKind.Char) & Current.Kind != TokenKind.End)
            {
                NextToken();
            }
            int endPosition = _position;
            int count = endPosition - startPosition;
            var CharTokens = _tokens.GetRange(startPosition, count);
            string word = MergeToString(CharTokens);
            if (IsTokenFunction(word))
            {
                token = new Token(TokenKind.Function, word, _tokens[startPosition].Position);
            }
            else if(_vars!=null)
            {
                if(_vars.Contains(word))
                    token = new Token(TokenKind.Var, word, _tokens[startPosition].Position);
                else
                    token = new Token(TokenKind.Bad, null, _tokens[startPosition].Position);
            } 
            else
                token = new Token(TokenKind.Bad, null, _tokens[startPosition].Position);
            return token;
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
