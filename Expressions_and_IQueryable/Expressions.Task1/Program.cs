using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Expressions.Task1
{
    internal class Program
    {
        private static void Main()
        {
            // Part 1.
            Expression<Func<int, int, int>> expression = (a, b) => a + (a + 1) * (a + 5) * (a - 1) + (1 + a) / (1 - a) * (b + 1) + (b - 1);
            var result1 = new TaskVisitor1().Visit(expression);
            Console.WriteLine(result1);

            // Part 2.
            var wildcardData = new Dictionary<string, int> { { "a", 123 }, { "b", 321 } };
            var result2 = new TaskVisitor2<int>(wildcardData).Visit(expression);
            Console.WriteLine(result2);

            Console.WriteLine("Finish");
            Console.ReadKey();
        }
    }
}
