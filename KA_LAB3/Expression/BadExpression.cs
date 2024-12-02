using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.Expression
{
    class BadExpression:NodeExpression
    {
        public BadExpression()
        {
            //throw new Exception("неизвестное выражение");
        }

        public override ExpressionKind Kind => ExpressionKind.Bad;

        public override StringBuilder ToString(StringBuilder sb)
        {
            throw new Exception("синтаксическая ошибка");
        }
    }
}
