using System;
using System.Threading;

namespace Task4
{
    internal class Program
    {
        private static void Main()
        {
            const int state = 10;

            var thread = new Thread(ThreadFunc);
            thread.Start(state);
            thread.Join();

            Console.WriteLine("Finish");
            Console.ReadKey();
        }

        private static void ThreadFunc(object data)
        {
            int state = (int)data - 1;
            Console.WriteLine($"Thread id: {Thread.CurrentThread.ManagedThreadId}. state: {state}");

            if (state == 0)
                return;

            var newThread = new Thread(ThreadFunc);
            newThread.Start(state);
            newThread.Join();
        }
    }
}
