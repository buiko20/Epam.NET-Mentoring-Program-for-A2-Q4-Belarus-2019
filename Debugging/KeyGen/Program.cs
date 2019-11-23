using System;
using System.Linq;
using System.Net.NetworkInformation;

namespace KeyGen
{
    internal class Program
    {
        private static void Main()
        {
            var firstNetworkInterface = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault();
            if (firstNetworkInterface == null)
            {
                Console.WriteLine("Could not find the network interfaces");
                return;
            }
            byte[] addressBytes = firstNetworkInterface.GetPhysicalAddress().GetAddressBytes();

            byte[] bytes = BitConverter.GetBytes(DateTime.Now.Date.ToBinary());
            int[] keyArray = addressBytes
                .Select((b, i) => SomeKeySalt(b, i, bytes))
                .Select(b => b * 10)
                .ToArray();
            string key = keyArray
                .Aggregate(string.Empty, (current, i) => current + (i + "-"))
                .TrimEnd('-');
            Console.WriteLine(key);

            int[] input = key.Split('-').Select(int.Parse).ToArray();
            bool isCorrectKey = keyArray
                .Select((i, i1) => Transform(i, i1, input))
                .All(i => i == 0);
            Console.WriteLine(isCorrectKey);

            Console.ReadKey();
            Console.WriteLine("Finish");
        }

        private static int SomeKeySalt(int a, int b, byte[] bytes)
        {
            return a ^ bytes[b];
        }

        private static int Transform(int a, int b, int[] ints)
        {
            return a - ints[b];
        }
    }
}
