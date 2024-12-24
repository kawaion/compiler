using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KA_LAB3.Polynomes
{
    static class Scaler
    {
        public static double[,] Scale(double[,] matrix, double lamda)
        {
            double[,] res = (double[,])matrix.Clone();
            for (int i = 0; i < matrix.GetLength(0); i++)
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    res[i, j] *= lamda;
                }
            return res;
        }
        public static double[] Scale(double[] vector, double lamda)
        {
            double[] res = (double[])vector.Clone();
            for (int i = 0; i < vector.GetLength(0); i++)
                res[i] *= lamda;
            return res;
        }
        public static double[,] ScaleColumn(double[,] matrix, double lamda, int i)
        {
            for (int j = 0; j < matrix.GetLength(1); j++)
            {
                matrix[i, j] *= lamda;
            }
            return matrix;
        }
    }
}
