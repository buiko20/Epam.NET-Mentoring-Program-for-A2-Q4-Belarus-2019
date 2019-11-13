using System;
using System.IO;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMQ.Server
{
    internal class Program
    {
        private const string HostName = "localhost";
        private const string Exchange = "Message_Queues";
        private const string RoutingKey = "RabbitMQ";
        private const string Storage = ".";

        private static void Main()
        {
            var factory = new ConnectionFactory { HostName = HostName };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(Exchange, ExchangeType.Direct);
                    var queue = channel.QueueDeclare();
                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                    channel.QueueBind(queue.QueueName, Exchange, RoutingKey);

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += ConsumerOnReceived;
                    channel.BasicConsume(queue.QueueName, autoAck: false, consumer);

                    Console.WriteLine("Waiting for messages.");
                    Console.ReadKey();
                    consumer.Received -= ConsumerOnReceived;
                }
            }
        }

        private static void ConsumerOnReceived(object sender, BasicDeliverEventArgs e)
        {
            var consumer = (EventingBasicConsumer)sender;
            var channel = consumer.Model;

            string fileName = Encoding.UTF8.GetString((byte[])e.BasicProperties.Headers["file"]);
            int size = Convert.ToInt32(e.BasicProperties.Headers["size"]);
            long offset = Convert.ToInt64(e.BasicProperties.Headers["offset"]);
            long totalSize = Convert.ToInt64(e.BasicProperties.Headers["totalSize"]);
            Console.WriteLine($"{fileName} size={size} offset={offset} totalSize={totalSize}");

            string path = Path.Combine(Storage, fileName);
            using (var fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                fs.SetLength(totalSize);
                fs.Position = offset;
                fs.Write(e.Body, 0, size);
            }

            channel.BasicAck(e.DeliveryTag, multiple: false);
        }
    }
}
