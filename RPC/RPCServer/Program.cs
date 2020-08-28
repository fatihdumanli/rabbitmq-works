using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RPCServer
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
                    channel.QueueDeclare(queue: "rpc_queue", durable: false, exclusive: false, autoDelete: false,
                        arguments: null);

                    channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

                    var consumer = new EventingBasicConsumer(channel);
                    channel.BasicConsume(queue: "rpc_queue", autoAck: false, consumer: consumer);

                    Console.WriteLine(" [x] Awaiting RPC requests.");


                    consumer.Received += (model, ea) => {
                        string response = null;

                        var body = ea.Body.ToArray();
                        var props = ea.BasicProperties;
                        var replyProps = channel.CreateBasicProperties();

                        replyProps.CorrelationId = props.CorrelationId;

                        try
                        {
                            var message = Encoding.UTF8.GetString(body);
                            int n = int.Parse(message);
                            Console.WriteLine(" [.] fib({0})", message);
                            response = fib(n).ToString();
                        } 
                        
                        catch(Exception e)
                        {
                            Console.WriteLine(" [.] {0}", e.Message);
                            response = string.Empty;
                        }

                        finally
                        {
                            var responseBytes = Encoding.UTF8.GetBytes(response);
                            channel.BasicPublish(exchange: "", routingKey: props.ReplyTo,
                                basicProperties: replyProps, body: responseBytes);

                            Console.WriteLine(" [x] Publishing message {0}" + 
                                "with routingKey: {1}, basicProperties: {2}", response, props.ReplyTo, replyProps);

                            channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                        }

                    };


                    Console.WriteLine(" Press [enter] to exit. ");
                    Console.ReadLine();
                }


            }
            
        }

        private static int fib(int n)
        {
            if (n == 0 || n == 1)
            {
                return n;
            }

            return fib(n - 1) + fib(n - 2);
        }
    }
}
