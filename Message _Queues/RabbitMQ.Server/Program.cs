using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Server
{
    internal class Program
    {
        private const string HostName = "localhost";
        private const string Exchange = "Message_Queues";
        private const string FileSendRoutingKey = "FileSendRoutingKey";
        private const string StatusSendRoutingKey = "StatusSendRoutingKey";
        private const string StatusChangeRoutingKey = "StatusChangeRoutingKey";
        private const string WatcherFilter = "settings.json";
        private const string Storage = ".";

        private static void Main()
        {
            var factory = new ConnectionFactory { HostName = HostName };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(Exchange, ExchangeType.Direct);
                    var sendFileQueueName = channel.QueueDeclare().QueueName;
                    var getStatusQueueName = channel.QueueDeclare().QueueName;
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                    channel.QueueBind(sendFileQueueName, Exchange, FileSendRoutingKey);
                    channel.QueueBind(getStatusQueueName, Exchange, StatusSendRoutingKey);

                    var fileConsumer = new EventingBasicConsumer(channel);
                    fileConsumer.Received += OnFileReceived;
                    channel.BasicConsume(sendFileQueueName, autoAck: false, fileConsumer);

                    var statusConsumer = new EventingBasicConsumer(channel);
                    statusConsumer.Received += OnStatusReceived;
                    channel.BasicConsume(getStatusQueueName, autoAck: false, statusConsumer);

                    Console.WriteLine("Waiting for messages.");
                    using (var watcher = new FileSystemWatcher(Storage, WatcherFilter))
                    {
                        watcher.NotifyFilter = NotifyFilters.LastWrite;
                        watcher.Changed += (sender, args) => { ChangeChunkSizeAndGetStatus(channel); };
                        watcher.EnableRaisingEvents = true;

                        Console.ReadKey();
                    }

                    fileConsumer.Received -= OnFileReceived;
                    statusConsumer.Received -= OnStatusReceived;
                }
            }
        }

        private static void OnFileReceived(object sender, BasicDeliverEventArgs e)
        {
            var consumer = (EventingBasicConsumer)sender;
            var channel = consumer.Model;

            string fileName = Encoding.UTF8.GetString((byte[])e.BasicProperties.Headers["file"]);
            int size = Convert.ToInt32(e.BasicProperties.Headers["size"]);
            long offset = Convert.ToInt64(e.BasicProperties.Headers["offset"]);
            long totalSize = Convert.ToInt64(e.BasicProperties.Headers["totalSize"]);

            string path = Path.Combine(Storage, fileName);
            using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                fs.SetLength(totalSize);
                fs.Position = offset;
                fs.Write(e.Body, 0, size);
            }

            channel.BasicAck(e.DeliveryTag, multiple: false);
        }

        private static void OnStatusReceived(object sender, BasicDeliverEventArgs e)
        {
            var consumer = (EventingBasicConsumer)sender;
            var channel = consumer.Model;

            string status = Encoding.UTF8.GetString((byte[])e.BasicProperties.Headers["status"]);
            string guid = Encoding.UTF8.GetString((byte[])e.BasicProperties.Headers["guid"]);
            int chunkSize = Convert.ToInt32(e.BasicProperties.Headers["chunkSize"]);
            Console.WriteLine($"guid={guid} status={status} chunkSize={chunkSize}");

            channel.BasicAck(e.DeliveryTag, multiple: false);
        }

        private static void ChangeChunkSizeAndGetStatus(IModel model)
        {
            var basicProperties = model.CreateBasicProperties();
            basicProperties.Persistent = true;
            int size = JsonConvert.DeserializeObject<dynamic>(File.ReadAllText(WatcherFilter)).chunkSize;
            basicProperties.Headers = new Dictionary<string, object>
            {
                { "chunkSize", size }
            };

            model.BasicPublish(Exchange, StatusChangeRoutingKey, basicProperties, new byte[0]);
        }
    }
}
