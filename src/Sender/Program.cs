using System;
using System.Text;
using Microsoft.Extensions.CommandLineUtils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;

namespace Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            var app = new CommandLineApplication();
            app.Name = "Sender";
            app.Description = "Sends an alert message to RabbitMQ to be processed by Sitecore Host applications";

            app.HelpOption("-?|-h|--help");

            var enabledOption = app.Option("--enabled", "Enables the alert", CommandOptionType.NoValue);
            var disabledOption = app.Option("--disabled", "Disables the alert", CommandOptionType.NoValue);
            var messageOption = app.Option("-m|--message", "Specifies the alert message", CommandOptionType.SingleValue);

            app.OnExecute(() =>
            {
                var enabled = -1;
                var message = "";

                if (enabledOption.HasValue())
                {
                    enabled = 1;
                }
                if (disabledOption.HasValue())
                {
                    enabled = 0;
                }
                if (messageOption.HasValue())
                {
                    message = messageOption.Value();
                }
                else
                {
                    app.ShowHint();
                    return 0;
                }

                if (enabled == -1)
                {
                    app.ShowHint();
                    return 0;
                }

                var jsonObject = new JObject(
                    new JProperty("enabled", enabled),
                    new JProperty("message", message)
                );

                Console.WriteLine("Sending the following message:");
                Console.WriteLine(jsonObject.ToString());

                var connectionFactory = new ConnectionFactory { Uri = new Uri("amqp://guest:guest@localhost:5672") };
            
                using (var connection = connectionFactory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {
                        var body = Encoding.UTF8.GetBytes(jsonObject.ToString(Formatting.None));

                        channel.BasicPublish(
                            exchange: "sugcon-exchange",
                            routingKey: "",
                            basicProperties: null,
                            body: body
                        );

                        Console.WriteLine("Message sent.");
                    }
                }

                return 0;
            });

            app.Execute(args);
        }
    }
}
