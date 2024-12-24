using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.Polynomes
{
    internal class Polynom
    {
        public Polynom(double[] koef)
        {
            Koefs = koef;
        }
        public virtual int Count => Koefs.Length;
        public virtual int deg => Koefs.Length - 1;
        public virtual double this[int i]
        {
            get { return Koefs[i]; }
            set { Koefs[i] = value; }
        }
        public virtual double lc => Koefs.Last();
        public virtual double[] ToVector()
        {
            return Koefs;
        }
        private double[] polynomKoefs;
        private double[] Koefs
        {
            get
            {
                return polynomKoefs;
            }
            set
            {
                if (value.Last() == 0)
                {
                    polynomKoefs = PolynomialPruner.PrunZeroPart(value);
                }
                else polynomKoefs = value;
            }
        }

        public virtual Polynom Add(Polynom other)
        {
            return PolynomArithmetic.Add(this, other);
        }
        public virtual Polynom Minus(Polynom other)
        {
            return PolynomArithmetic.Minus(this, other);
        }
        public virtual Polynom Multi(Polynom other)
        {
            return PolynomArithmetic.Multi(this, other);
        }
        public virtual (Polynom, Polynom) Divide(Polynom other)
        {
            return PolynomArithmetic.Divide(this, other);
        }
        public virtual double Gorner(double x)
        {
            return ValuePolynomialByX.GornerMethod(this, x);
        }
        public virtual double Solve(double x)
        {
            return ValuePolynomialByX.DefaultMethod(this, x);
        }
        public virtual void MultiOnXPow(int step)
        {
            Koefs = PolynomArithmetic.MultiOnXPow(step, Koefs);
        }
        public static Polynom operator +(Polynom polynom1, Polynom polynom2)
        {
            return polynom1.Add(polynom2);
        }
        public static Polynom operator -(Polynom polynom1, Polynom polynom2)
        {
            return polynom1.Minus(polynom2);
        }
        public static Polynom operator *(Polynom polynom1, Polynom polynom2)
        {
            return polynom1.Multi(polynom2);
        }
        public static (Polynom, Polynom) operator /(Polynom polynom1, Polynom polynom2)
        {
            return polynom1.Divide(polynom2);
        }
        public static Polynom operator *(Polynom polynom, double lamda)
        {
            return PolynomArithmetic.Scale(polynom, lamda);
        }
        public static Polynom operator *(double lamda, Polynom polynom)
        {
            return polynom * lamda;
        }
        public static Polynom operator /(Polynom polynom, double lamda)
        {
            return PolynomArithmetic.Scale(polynom, 1 / lamda);
        }
        public static Polynom operator /(double lamda, Polynom polynom)
        {
            return polynom / lamda;
        }
        public virtual Polynom Clone()
        {
            return new Polynom(Koefs);
        }
    }
}
