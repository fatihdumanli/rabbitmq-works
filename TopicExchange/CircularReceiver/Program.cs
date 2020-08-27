using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace CircularReceiver
{
    class Program
    {
        static void Main(string[] args)
        {

            var location = args[0].ToString().ToLower();
            var department = args[1].ToString().ToLower();
            var routeKey = string.Format("{0}.{1}", location, department);

            var factory = new ConnectionFactory() { HostName = "localhost" };

            using(var connection = factory.CreateConnection())
            {
                using(var channel = connection.CreateModel())
                {
                    
                    channel.ExchangeDeclare("circulars-exchange", type: ExchangeType.Topic);

                    var queueName = channel.QueueDeclare().QueueName;

                    var consumer = new EventingBasicConsumer(channel);

                    channel.QueueBind(queue: queueName, exchange: "circulars-exchange", routeKey);
                    channel.BasicConsume(queue: queueName, autoAck: true, consumer);


                    consumer.Received += (model, ea) => {
                        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                        Console.WriteLine(" [x] Received a message: {0}", message);
                    };

                    Console.WriteLine(" [x] Waiting for messages ({0})...", routeKey);
                    Console.Read();
                }

            }

        }
    }
}
