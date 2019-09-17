using System;
using System.Collections.Generic;
using System.Threading;

namespace Task6
{
    internal class Program
    {
        private const int Count = 10;
        private static readonly List<int> SharedCollection = new List<int>(Count);
        private static readonly AutoResetEvent ItemAddedEvent = new AutoResetEvent(false),
            ItemsPrintedEvent = new AutoResetEvent(true);

        private static void Main()
        {
            var addItemsThread = new Thread(AddElements);
            addItemsThread.Start();
            var printItemsThread = new Thread(PrintItems);
            printItemsThread.Start();


            addItemsThread.Join();
            printItemsThread.Join();
            ItemAddedEvent.Dispose();
            ItemsPrintedEvent.Dispose();
            Console.WriteLine("Finish");
            Console.ReadKey();
        }

        private static void AddElements()
        {
            var rnd = new Random();
            for (int i = 0; i < Count; i++)
            {
                ItemsPrintedEvent.WaitOne();
                int newItem = rnd.Next(minValue: 0, maxValue: Count);
                Console.WriteLine($"Thread id: {Thread.CurrentThread.ManagedThreadId}. Add item {newItem}");
                SharedCollection.Add(newItem);
                ItemAddedEvent.Set();
            }
        }

        private static void PrintItems()
        {
            for (int i = 0; i < Count; i++)
            {
                ItemAddedEvent.WaitOne();
                Console.Write($"Thread id: {Thread.CurrentThread.ManagedThreadId}. Print items: ");
                foreach (int item in SharedCollection)
                {
                    Console.Write($"{item} ");
                }
                Console.WriteLine();
                ItemsPrintedEvent.Set();
            }
        }
    }
}
