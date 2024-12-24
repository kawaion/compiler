using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.Polynomes
{
    static class ValuePolynomialByX
    {
        public static double GornerMethod(Polynom polynom, double x)
        {
            double res = polynom[polynom.Count - 1] * x + polynom[polynom.Count - 2];
            for (int i = polynom.Count - 3; i >= 0; i--)
            {
                res = res * x + polynom[i];
            }
            return res;
        }
        public static double DefaultMethod(Polynom polynom, double x)
        {
            double res = polynom[0];
            for (int i = 1; i < polynom.Count; i++)
            {
                res += polynom[i] * Math.Pow(x, i);
            }
            return res;
        }
    }
}
