using System;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;

namespace Sender
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter a message.");
                return 1;
            }

            var connectionFactory = new ConnectionFactory { Uri = new Uri("amqp://guest:guest@localhost:5672") };
            
            using (var connection = connectionFactory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var jsonObject = new JObject(
                        new JProperty("enabled", 1),
                        new JProperty("message", args[0])
                    );
                    var body = Encoding.UTF8.GetBytes(jsonObject.ToString(Formatting.None));

                    channel.BasicPublish(
                        exchange: "sugcon-exchange",
                        routingKey: "",
                        basicProperties: null,
                        body: body
                    );

                    return 0;
                }
            }
        }
    }
}
