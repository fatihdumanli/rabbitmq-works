using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;

namespace CircularEmitter
{
    class Program
    {
        static void Main(string[] args)
        {   
            
            var factory = new ConnectionFactory() { HostName = "localhost" };

            using(var connection = factory.CreateConnection())
            {
                using(var channel = connection.CreateModel())
                {

                    channel.ExchangeDeclare(exchange: "circulars-exchange", type: ExchangeType.Topic);
                   
                    while(true)
                    {

                        var circular = (Circular) new CircularRandomizer().CreateRandomizeInstance();
                        var message = circular.Description;
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "circulars-exchange", 
                            routingKey: circular.GetRouteKey(),
                            null, body); 

                        Console.WriteLine(" [x] Message delivered to exchange: {0}", message);                       

                        Thread.Sleep(500);
                    }

                }
            }

        }
    }
}
