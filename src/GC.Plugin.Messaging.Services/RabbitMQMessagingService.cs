using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using GC.Plugin.Messaging.Services.Alerts;
using GC.Plugin.Messaging.Services.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;
using Sitecore.Framework.Runtime.Configuration;

namespace GC.Plugin.Messaging.Services
{
	public class RabbitMQMessagingService : IHostedService, IDisposable
	{
		private readonly string _sectionName = "Sitecore:MessageQueueProviders:RabbitMQ";
		private readonly ISitecoreConfiguration _configuration;
		private readonly ILogger<RabbitMQMessagingService> _logger;
		private readonly IModel _channel;
		private readonly IAlertService _alertService;
		private ISubscription _subscription;
		private Timer _timer;
		private bool _running;

		public RabbitMQMessagingService(ISitecoreConfiguration configuration, ILogger<RabbitMQMessagingService> logger, IModel channel, IAlertService alertService)
		{
			_logger = logger;
			_channel = channel;
			_configuration = configuration;
			_alertService = alertService;
		}

		public Task StartAsync(CancellationToken cancellationToken)
		{
			var rabbitMqConfiguration = new RabbitMQConfiguration();
			_configuration.GetSection(_sectionName).Bind(rabbitMqConfiguration);

			var queueName = _channel.QueueDeclare().QueueName;

            _channel.QueueBind(queue: queueName,
                              exchange: rabbitMqConfiguration.ExchangeName,
                              routingKey: "");

			_subscription = new Subscription(_channel, queueName, false);

			_logger.LogInformation($"Initializing RabbitMQ receiver - Queue: { queueName }");

			_timer = new Timer(GetMessages, null, TimeSpan.Zero, TimeSpan.FromSeconds(rabbitMqConfiguration.Delay));

			return Task.CompletedTask;
		}

		private void GetMessages(object state)
		{
			if (_running)
			{
				return;
			}

			_running = true;

			_logger.LogInformation($"Receiving messages - { DateTime.Now }");

			foreach (BasicDeliverEventArgs args in _subscription)
			{
				string message = Encoding.UTF8.GetString(args.Body);
				_logger.LogInformation($"Message received: { message }");
				var jsonObject = JObject.Parse(message);

				_alertService.IsEnabled = jsonObject.GetValue("enabled").Value<bool>();
				_alertService.Message = jsonObject.GetValue("message").Value<string>();

				_channel.BasicAck(args.DeliveryTag, false);
			}
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			return Task.CompletedTask;
		}

		public void Dispose()
		{
		}
	}
}