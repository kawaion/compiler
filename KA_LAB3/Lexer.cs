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
            ErrorWriting.IsNumberOpenAndClosedBrackets(tokens);
            ErrorWriting.IsTwoSignInRow(tokens);
            tokens = MergeDigitsAndChars(tokens);
            return tokens;
        }
        private List<Token> MergeDigitsAndChars(List<Token> tokens)
        {
            List<Token> resTokens = new List<Token>();
            for(int i = 0; i < tokens.Count; i++)
            {
                Token token = tokens[i];
                if (token.Kind == TokenKind.Number)
                {
                    int startPosition = i;
                    while(token.Kind == TokenKind.Number & token.Kind != TokenKind.End)
                    {
                        i++;
                        token = tokens[i];
                    }
                    int endPosition = i;
                    int count = endPosition - startPosition;
                    var digitTokens = tokens.GetRange(startPosition, count);
                    string number = MergeToString(digitTokens);
                    if(int.TryParse(number, out int value))
                    {
                        resTokens.Add(new Token(TokenKind.Number, value));
                    }
                    else
                        resTokens.Add(new Token(TokenKind.Bad, null));
                    ErrorWriting.IsVarStartsWithNumber(token, number);
                    i--;
                }
                else if (token.Kind == TokenKind.Char)
                {
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
                        resTokens.Add(new Token(TokenKind.Var, word));
                    }
                    else if (IsTokenFunction(word))
                    {
                        resTokens.Add(new Token(TokenKind.function, word));
                    }
                    else
                        resTokens.Add(new Token(TokenKind.Bad, null));
                    i--;
                }
                else
                    if(token.Kind != TokenKind.Space)
                    resTokens.Add(token);
            }
            return resTokens;
        }
        private bool IsTokenFunction(string word)
        {
            foreach (string name in Enum.GetNames(typeof(TokenFunction)))
            {
                if(word==name) 
                    return true;
            }
            return false;
        }
        private string MergeToString(List<Token> tokens)
        {
            StringBuilder sb = new StringBuilder();
            foreach(Token token in tokens)
            {
                sb.Append(token.Value.ToString());
            }
            return sb.ToString();
        }
    }
}
