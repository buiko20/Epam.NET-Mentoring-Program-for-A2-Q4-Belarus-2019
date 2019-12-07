using System;
using CastleAsyncProxy.Logic;

namespace CastleAsyncProxy
{
    internal class Program
    {
        private static void Main()
        {
            var service = DependencyRoot.Resolve<IService>();

            Guid guid = service.CreateRandom();
            var entity = service.GetAsync(guid).Result;

            Console.WriteLine(entity);

            DependencyRoot.Dispose();
            Console.WriteLine("Finish");
            Console.ReadKey();
        }
    }
}
