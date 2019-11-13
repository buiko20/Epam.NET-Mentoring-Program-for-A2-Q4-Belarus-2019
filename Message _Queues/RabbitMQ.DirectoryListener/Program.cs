using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RabbitMQ.Client;
using Shared;

namespace RabbitMQ.DirectoryListener
{
    internal class Program
    {
        private const string HostName = "localhost";
        private const string Exchange = "Message_Queues";
        private const string RoutingKey = "RabbitMQ";
        private const string DirectoryToWatch = @".";
        private const string WatcherFilter = "*.pdf";

        private static readonly object SyncRoot = new object();
        private static readonly IList<string> Pdfs = new List<string>();

        private static void Main()
        {
            using (var watcher = new FileSystemWatcher(DirectoryToWatch, WatcherFilter))
            {
                watcher.NotifyFilter = NotifyFilters.LastWrite;

                var factory = new ConnectionFactory { HostName = HostName };
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        channel.ExchangeDeclare(Exchange, ExchangeType.Direct);

                        var onChanged = new FileSystemEventHandler((sender, e) =>
                        {
                            lock (SyncRoot)
                            {
                                if (!Pdfs.Any(s => s.Equals(e.FullPath, StringComparison.OrdinalIgnoreCase)))
                                {
                                    Pdfs.Add(e.FullPath);
                                    SendFile(channel, e.FullPath);
                                }
                            }
                        });

                        watcher.Changed += onChanged;
                        watcher.EnableRaisingEvents = true;

                        Console.WriteLine("Press key to exit");
                        Console.ReadKey();
                        watcher.Changed -= onChanged;
                    }
                }
            }
        }

        private static void SendFile(IModel model, string filePath)
        {
            var fe = new FileEnumerator(filePath);
            foreach (Chunk chunk in fe)
            {
                var basicProperties = model.CreateBasicProperties();
                basicProperties.Persistent = true;
                basicProperties.Headers = new Dictionary<string, object>
                {
                    { "file", chunk.FileName },
                    { "size", chunk.Size },
                    { "offset", chunk.Offset },
                    { "totalSize", chunk.TotalSize },
                };

                model.BasicPublish(Exchange, RoutingKey, basicProperties, chunk.Data);
            }
        }
    }
}
