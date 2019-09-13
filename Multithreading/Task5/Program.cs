using System;
using System.Threading;

namespace Task5
{
    internal class Program
    {
        private static Semaphore _semaphore;

        private static void Main()
        {
            const int state = 10;
            _semaphore = new Semaphore(initialCount: 0, maximumCount: state);

            ThreadPool.QueueUserWorkItem(ThreadFunc, state);

            for (int i = 0; i < state; i++)
            {
                _semaphore.WaitOne();
            }

            _semaphore.Dispose();
            Console.WriteLine("Finish");
            Console.ReadKey();
        }

        private static void ThreadFunc(object data)
        {
            int state = (int)data - 1;
            Console.WriteLine($"Thread id: {Thread.CurrentThread.ManagedThreadId}. state: {state}");

            if (state != 0)
            {
                ThreadPool.QueueUserWorkItem(ThreadFunc, state);
            }

            _semaphore.Release(1);
        }
    }
}
