using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.Polynomes
{
    static internal class PolynomialPruner
    {
        public static double[] PrunZeroPart(double[] koefs)
        {
            int maxNoZeroIKoef = MaxNoZeroPow(koefs);
            double[] res = koefs;
            if (maxNoZeroIKoef == koefs.Length - 1) { }
            else
            {
                res = new double[maxNoZeroIKoef + 1];
                for (int i = 0; i <= maxNoZeroIKoef; i++)
                {
                    res[i] = koefs[i];
                }
            }
            return res;
        }
        private static int MaxNoZeroPow(double[] koefs)
        {
            int i;
            for (i = koefs.Length - 1; i >= 0; i--)
            {
                if (koefs[i] != 0) break;
            }
            return i;
        }
    }
}
