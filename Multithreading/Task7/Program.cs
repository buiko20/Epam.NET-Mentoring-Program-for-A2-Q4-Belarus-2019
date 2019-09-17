using System;
using System.Threading;
using System.Threading.Tasks;

namespace Task7
{
    internal class Program
    {
        private static void Main()
        {
            // a.   Continuation task should be executed regardless of the result of the parent task.
            var taskA = Task.Run(() => Console.WriteLine($"Task A. Thread id {Thread.CurrentThread.ManagedThreadId}"))
                .ContinueWith(task => Console.WriteLine("Task A"), TaskContinuationOptions.None);
            taskA.Wait();

            // b.	Continuation task should be executed when the parent task finished without success.
            var taskB = Task.Run(() => throw new Exception("some error Task B"))
                .ContinueWith(task => Console.WriteLine("Task B"), TaskContinuationOptions.None);
            taskB.Wait();

            // c.	Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.
            var taskC = Task.Run(() =>
                    {
                        Console.WriteLine($"Task C. Thread id for task C {Thread.CurrentThread.ManagedThreadId}");
                        throw new Exception("some error Task C");
                    })
                .ContinueWith(task => Console.WriteLine($"Task C. Thread id for task C continuation {Thread.CurrentThread.ManagedThreadId}"), TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);
            taskC.Wait();

            // d.	Continuation task should be executed outside of the thread pool when the parent task would be cancelled.
            var tcs = new TaskCompletionSource<object>();
            var taskD = tcs.Task;
            tcs.SetCanceled();
            var taskResult = taskD.ContinueWith(task => Console.WriteLine($"Task D. {nameof(Thread.CurrentThread.IsThreadPoolThread)}: {Thread.CurrentThread.IsThreadPoolThread}"), TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning);
            taskResult.Wait();

            Console.WriteLine("Finish");
            Console.ReadKey();
        }
    }
}
