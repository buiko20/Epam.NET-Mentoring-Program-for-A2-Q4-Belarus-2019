using System;
using System.Threading.Tasks;

namespace Task1
{
    internal class Program
    {
        private static void Main()
        {
            const int tasksCount = 100, iterationCount = 1000;
            Task[] tasks = new Task[tasksCount];
            for (int i = 0; i < tasks.Length; i++)
            {
                int taskNumber = i;
                var task = Task.Run(() =>
                {
                    for (int j = 0; j < iterationCount; j++)
                    {
                        Console.WriteLine($"Task #{taskNumber} - {j}");
                        //Console.Out.Flush();
                    }
                });

                tasks[i] = task;
            }

            Task.WaitAll(tasks);
            Console.WriteLine("Finish");
            Console.ReadKey();
        }
    }
}
