using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.Polynomes
{
    class PolynomArithmetic
    {
        public static double[] MultiOnXPow(int step, double[] vector)
        {
            double[] res = (double[])vector.Clone();
            res = IncreaseLengthVectorTo(vector.Length + step, res);
            for (int i = res.Length - 1; i > step - 1; i--)
            {
                res[i] = res[i - step];
            }
            for (int i = step - 1; i >= 0; i--)
            {
                res[i] = 0;
            }
            return res;
        }
        public static Polynom Pow(Polynom polynom, int step)
        {
            return new Polynom(MultiOnXPow(step, polynom.ToVector()));
        }
        public static Polynom Add(Polynom a, Polynom b)
        {
            return new Polynom(ArithmeticOperator.Add(a.ToVector(), b.ToVector()));
        }
        public static Polynom Minus(Polynom a, Polynom b)
        {
            return PolynomArithmetic.Add(a, PolynomArithmetic.Scale(b, -1));
        }
        public static Polynom Multi(Polynom a, Polynom b)
        {
            //double[] incA = new double[a.Count + b.Count - 1];
            double[] resVector = new double[a.Count + b.Count - 1];
            double[] incA = a.ToVector();
            //incA = IncreaseLengthVectorTo(a.Count + b.Count - 1, a.ToVector());
            for (int i = b.Count - 1; i >= 0; i--)
            {
                double[] tmp = MultiOnXPow(i, incA);
                tmp = Scaler.Scale(tmp, b[i]);
                resVector = ArithmeticOperator.Add(resVector, tmp);
            }
            Polynom resPolynom = new Polynom(resVector);
            return resPolynom;
        }
        public static (Polynom, Polynom) Divide(Polynom _a, Polynom _b)
        {
            Polynom a = _a.Clone();
            Polynom b = _b.Clone();
            Polynom q = new Polynom(new double[] { 0 });
            while (a.deg >= b.deg)
            {
                q += Pow(new Polynom(new double[] { (a.lc / b.lc) }), a.deg - b.deg);
                a = a - Pow((a.lc / b.lc) * b, a.deg - b.deg);
            }
            Polynom r = a;
            return (q, r);
        }
        public static Polynom Scale(Polynom polynom, double lamda)
        {
            return new Polynom(Scaler.Scale(polynom.ToVector(), lamda));
        }

        private static double[] IncreaseLengthVectorTo(int len, double[] vector)
        {
            double[] res = new double[len];
            for (int i = 0; i < vector.Length; i++)
            {
                res[i] = vector[i];
            }
            return res;
        }
    }
}
