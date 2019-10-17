using System;
using System.Collections.Generic;

namespace Expressions.Task2
{
    internal class Program
    {
        private static void Main()
        {
            var foo = new Foo { i = 4, Str = "str123", Value2 = 1.32d };
            Bar bar;
            var bindings = new Dictionary<string, string>
            {
                { nameof(foo.Map1), nameof(bar.Map3) },
                { nameof(foo.Map2), nameof(bar.Map4) }
            };
            bar = Mapper.Map<Foo, Bar>(foo, bindings);

            Console.WriteLine("Finish");
            Console.ReadKey();
        }
    }
}
