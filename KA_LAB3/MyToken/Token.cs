using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3
{
    internal class Token
    {
        public TokenKind Kind { get; }
        public object Value { get; }
        public int Position { get; }

        public Token(TokenKind kind, object value, int position)
        {
            Kind = kind;
            Value = value;
            Position = position;
        } 
    }
}
