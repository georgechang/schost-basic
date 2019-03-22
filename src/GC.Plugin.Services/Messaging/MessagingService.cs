using System;
using System.Text;
using System.Threading;
using GC.Plugin.Services.Alerts;
using GC.Plugin.Services.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Sitecore.Framework.Runtime.Configuration;

namespace GC.Plugin.Services.Messaging
{
	public class MessagingService : IMessagingService
	{
		private readonly string _sectionName = "Sitecore:MessageQueueProviders:RabbitMQ";
		private readonly ISitecoreConfiguration _configuration;
		private readonly ILogger<MessagingService> _logger;
		private readonly IAlertService _alertService;
		private RabbitMQConfiguration rabbitMqConfiguration;

		public MessagingService(ISitecoreConfiguration configuration, ILogger<MessagingService> logger, IAlertService alertService)
		{
			_configuration = configuration;
			_logger = logger;
			_alertService = alertService;
			rabbitMqConfiguration = new RabbitMQConfiguration();
			_configuration.GetSection(_sectionName).Bind(rabbitMqConfiguration);
		}

		public void Execute()
		{
			_logger.LogInformation($"Starting message service for RabbitMQ...");
			var factory = new ConnectionFactory { Uri = new Uri(rabbitMqConfiguration.ConnectionString) };

			using (var connection = factory.CreateConnection())
			{
				using (var channel = connection.CreateModel())
				{
                    channel.BasicQos(
                        prefetchSize: 0,
                        prefetchCount: 10,
                        global: false
                    );

					var consumer = new EventingBasicConsumer(channel);

					consumer.Received += (model, ea) =>
					{
						var message = Encoding.UTF8.GetString(ea.Body);
						_logger.LogInformation($"Message received: { message }");
						var jsonObject = JObject.Parse(message);

						_alertService.IsEnabled = jsonObject.GetValue("enabled").Value<bool>();
						_alertService.Message = jsonObject.GetValue("message").Value<string>();

                        Thread.Sleep(rabbitMqConfiguration.Delay);
						channel.BasicAck(ea.DeliveryTag, false);
					};

					channel.BasicConsume(
						queue: rabbitMqConfiguration.QueueName,
						autoAck: false,
						consumer: consumer
					);
				}
			}
		}
	}
}