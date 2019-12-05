using System;
using CastleProxy.Logic;

namespace CastleProxy
{
    internal class Program
    {
        private static void Main()
        {
            var service = DependencyRoot.Resolve<IService>();

            Guid guid = service.CreateRandom();
            var entity = service.Get(guid);

            Console.WriteLine(entity);

            DependencyRoot.Dispose();
            Console.WriteLine("Finish");
            Console.ReadKey();
        }
    }
}
