using System;
using System.Threading.Tasks;

namespace Task3
{
    internal class Program
    {
        private static void Main()
        {
            int[,] a =
            {
                { 1, 2, 4 },
                { 2, 0, 3 },
            };
            int[,] b =
            {
                { 2, 5 },
                { 1, 3 },
                { 1, 1 },
            };

            Print(a);
            Console.WriteLine();
            Print(b);

            Console.WriteLine();
            Print(Multiple(a, b));

            Console.WriteLine("Finish");
            Console.ReadKey();
        }

        //private static int[,] Multiple(int[,] a, int[,] b)
        //{
        //    if (a.GetLongLength(1) != b.GetLongLength(0))
        //    {
        //        throw new ArgumentException("Matrices cannot be multiplied. The number of columns in the first matrix is not equal to the number of rows in the second.");
        //    }

        //    int[,] result = new int[a.GetLongLength(0), b.GetLongLength(1)];
        //    for (int i = 0; i < a.GetLongLength(0); i++)
        //    {
        //        for (int j = 0; j < b.GetLongLength(1); j++)
        //        {
        //            for (int k = 0; k < b.GetLongLength(0); k++)
        //            {
        //                result[i, j] += a[i, k] * b[k, j];
        //            }
        //        }
        //    }

        //    return result;
        //}

        private static int[,] Multiple(int[,] a, int[,] b)
        {
            if (a.GetLongLength(1) != b.GetLongLength(0))
            {
                throw new ArgumentException("Matrices cannot be multiplied. The number of columns in the first matrix is not equal to the number of rows in the second.");
            }

            int[,] result = new int[a.GetLongLength(0), b.GetLongLength(1)];
            Parallel.For(0, a.GetLongLength(0), i =>
            {
                for (int j = 0; j < b.GetLongLength(1); j++)
                {
                    for (int k = 0; k < b.GetLongLength(0); k++)
                    {
                        result[i, j] += a[i, k] * b[k, j];
                    }
                }
            });

            return result;
        }

        private static void Print(int[,] matrix)
        {
            for (long i = 0; i < matrix.GetLongLength(0); i++)
            {
                for (long j = 0; j < matrix.GetLongLength(1); j++)
                {
                    Console.Write($"{matrix[i, j]} ");
                }

                Console.WriteLine();
            }
        }
    }
}
