using System;
using System.Text;
using System.Threading;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace LogEmitter
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

                    channel.ExchangeDeclare(exchange: "direct_logs", type: ExchangeType.Direct);

                    while(true)
                    {
                        var logItem = new LogRandomizerFactory().GetRandomLogItem();
                        var message = JsonConvert.SerializeObject(logItem);
                        var body = Encoding.UTF8.GetBytes(message);

                        channel.BasicPublish(exchange: "logs", 
                            routingKey: logItem.Severity.ToString(),
                            basicProperties: null,
                            body);

                        
                        Console.WriteLine(" [x] Log pushed to exchange 'logs' on {0} \n {1}", logItem.TimeStamp, message);
                        Thread.Sleep(500);
                    }

                }
            }

        }
    }
}
