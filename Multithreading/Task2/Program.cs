using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Task2
{
    internal class Program
    {
        private static void Main()
        {
            const int length = 10;
            var resultTask = CreateArrayAsync(length)
                .ContinueWith(MultipleWithRandom, TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(Sort, TaskContinuationOptions.OnlyOnRanToCompletion)
                .ContinueWith(ComputeAverage, TaskContinuationOptions.OnlyOnRanToCompletion);

            resultTask.Wait();
            Console.WriteLine("Finish");
            Console.ReadKey();
        }

        private static Task<int[]> CreateArrayAsync(int length)
        {
            var rnd = new Random();
            int[] result = new int[length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = rnd.Next(minValue: 0, maxValue: 10);
            }

            Print(result, "Array: ");
            return Task.FromResult(result);
        }

        private static int[] MultipleWithRandom(Task<int[]> task)
        {
            int rnd = new Random().Next(minValue: 0, maxValue: 10);
            int[] result = task.Result;
            for (int i = 0; i < result.Length; i++)
            {
                result[i] *= rnd;
            }

            Print(result, $"Multiple With Random {rnd}: ");
            return result;
        }

        private static int[] Sort(Task<int[]> task)
        {
            int[] result = task.Result;
            Array.Sort(result);
            Print(result, "Sort: ");
            return result;
        }

        private static int ComputeAverage(Task<int[]> task)
        {
            int average = (int)task.Result.Average();
            Console.WriteLine($"{Environment.NewLine}Average: {average}");
            return average;
        }

        private static void Print(IEnumerable<int> array, string text = null)
        {
            Console.WriteLine();
            if (!string.IsNullOrWhiteSpace(text))
            {
                Console.Write(text);
            }

            foreach (int item in array)
            {
                Console.Write($"{item} ");
            }
        }
    }
}
