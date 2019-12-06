using System;
using PostSharpCodeRewriting.Logic;

namespace PostSharpCodeRewriting
{
    internal class Program
    {
        private static void Main()
        {
            var service = new Service(new Repository());

            Guid guid = service.CreateRandom();
            var entity = service.Get(guid);

            Console.WriteLine(entity);

            Console.WriteLine("Finish");
            Console.ReadKey();
        }
    }
}
