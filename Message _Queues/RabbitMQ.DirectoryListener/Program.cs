using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.DirectoryListener
{
    internal class Program
    {
        private const string HostName = "localhost";
        private const string Exchange = "Message_Queues";
        private const string FileSendRoutingKey = "FileSendRoutingKey";
        private const string StatusSendRoutingKey = "StatusSendRoutingKey";
        private const string StatusChangeRoutingKey = "StatusChangeRoutingKey";
        private const string DirectoryToWatch = @".";
        private const string WatcherFilter = "*.pdf";
        private const int Interval = 5000;

        private static int _chunkSize = 4096;
        private static Status _status = Status.Waiting;
        private static Guid _guid = Guid.NewGuid();

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
                        var getStatusAndChangeChunkSizeQueueName = channel.QueueDeclare().QueueName;
                        channel.QueueBind(getStatusAndChangeChunkSizeQueueName, Exchange, StatusChangeRoutingKey);

                        var consumer = new EventingBasicConsumer(channel);
                        consumer.Received += (sender, args) =>
                        {
                            int chunkSize = Convert.ToInt32(args.BasicProperties.Headers["chunkSize"]);
                            _chunkSize = chunkSize;
                            SendStatus(channel);
                            Console.WriteLine($"New chunkSize = {_chunkSize}");
                        };
                        channel.BasicConsume(getStatusAndChangeChunkSizeQueueName, autoAck: true, consumer);

                        var onChanged = new FileSystemEventHandler((sender, e) =>
                        {
                            if (!Pdfs.Any(s => s.Equals(e.FullPath, StringComparison.OrdinalIgnoreCase)))
                            {
                                Pdfs.Add(e.FullPath);
                                SendFile(channel, e.FullPath);
                            }
                        });

                        var sendStatusAction = new ElapsedEventHandler((sender, args) => { SendStatus(channel); });
                        var timer = new Timer(Interval) { AutoReset = true };
                        timer.Elapsed += sendStatusAction;
                        timer.Start();

                        watcher.Changed += onChanged;
                        watcher.EnableRaisingEvents = true;

                        Console.WriteLine("Press key to exit");
                        Console.ReadKey();
                        watcher.Changed -= onChanged;
                        timer.Elapsed -= sendStatusAction;
                        timer.Dispose();
                    }
                }
            }
        }

        private static void SendFile(IModel model, string filePath)
        {
            _status = Status.Sending;
            using (var fe = new FileIterator(filePath, _chunkSize))
            {
                foreach (Chunk chunk in fe)
                {
                    var basicProperties = model.CreateBasicProperties();
                    basicProperties.Persistent = true;
                    basicProperties.Headers = new Dictionary<string, object>
                    {
                        { "file", chunk.FileName },
                        { "size", chunk.Size },
                        { "offset", chunk.Offset },
                        { "totalSize", chunk.TotalSize }
                    };

                    model.BasicPublish(Exchange, FileSendRoutingKey, basicProperties, chunk.Data);
                }
            }

            _status = Status.Waiting;
        }

        private static void SendStatus(IModel model)
        {
            var basicProperties = model.CreateBasicProperties();
            basicProperties.Persistent = true;
            basicProperties.Headers = new Dictionary<string, object>
            {
                { "status", _status.ToString() },
                { "guid", _guid.ToString() },
                { "chunkSize", _chunkSize }
            };

            model.BasicPublish(Exchange, StatusSendRoutingKey, basicProperties, new byte[0]);
        }
    }
}
