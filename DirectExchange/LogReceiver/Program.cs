using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace LogReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            var severityToListen = args[0];

            var factory = new ConnectionFactory() { HostName = "localhost" };

            using(var connection = factory.CreateConnection())
            {
                using(var channel = connection.CreateModel())
                {
                    
                    channel.ExchangeDeclare("logs", ExchangeType.Direct);

                    var queueName = channel.QueueDeclare().QueueName;

                    channel.QueueBind(queue: queueName, exchange: "logs", routingKey: severityToListen);

                    var consumer = new EventingBasicConsumer(channel);
                    
                    channel.BasicConsume(queue: queueName, true, consumer);


                    consumer.Received += (model, ea) => {

                        var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                        Console.WriteLine(" [x] Received a log message!: {0}", message);
                    };

                    Console.WriteLine(" [x] Waiting for log messages for severity: {0}", severityToListen);
                    Console.ReadLine();
                }

            }


        }
    }
}
