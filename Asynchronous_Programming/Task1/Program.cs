using System;
using System.Threading;
using System.Threading.Tasks;

namespace Task1
{
    internal class Program
    {
        private const string ExitChar = "q";

        private static void Main()
        {
            Console.WriteLine("Type 'q' to exit.");
            Console.Write("Enter number: ");
            string input = Console.ReadLine();

            CancellationTokenSource cts = null;
            while (!input.StartsWith(ExitChar, StringComparison.OrdinalIgnoreCase))
            {
                if (long.TryParse(input, out long number))
                {
                    cts?.Cancel();
                    cts?.Dispose();
                    cts = new CancellationTokenSource();
                    StartCalculationAsync(cts.Token, number);
                }

                Console.Write("Enter number: ");
                input = Console.ReadLine();
            }

            cts?.Cancel();
            cts?.Dispose();
            Console.WriteLine("Finish");
            Console.ReadKey();
        }

        private static async Task StartCalculationAsync(CancellationToken token, long number)
        {
            var result = await Task.Run(() =>
            {
                long sum = 0;
                for (int i = 0; i <= number; i++)
                {
                    token.ThrowIfCancellationRequested();
                    Thread.Sleep(50);
                    sum += i;
                }
                return sum;
            }, token);

            Console.WriteLine($"{Environment.NewLine}User number: {number}, sum: {result}");
        }
    }
}
