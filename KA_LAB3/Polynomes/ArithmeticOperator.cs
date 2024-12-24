using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.Polynomes
{
    static class ArithmeticOperator
    {
        public static double[] Add(double[] _a, double[] _b)
        {
            double[] a = _a;
            double[] b = _b;
            if (a.Length > b.Length)
            {
                for (int i = 0; i < b.GetLength(0); i++)
                {
                    a[i] += b[i];
                }
                return a;
            }
            else
            {
                for (int i = 0; i < a.GetLength(0); i++)
                {
                    b[i] += a[i];
                }
                return b;
            }
        }
    }
}
