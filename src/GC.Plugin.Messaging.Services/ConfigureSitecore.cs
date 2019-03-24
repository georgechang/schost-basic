using System;
using GC.Plugin.Messaging.Services.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using Sitecore.Framework.Runtime.Configuration;

namespace GC.Plugin.Messaging.Services
{
	public class ConfigureSitecore
	{
		private readonly string _sectionName = "Sitecore:MessageQueueProviders:RabbitMQ";
		private readonly ISitecoreConfiguration _configuration;
		private readonly ILogger<ConfigureSitecore> _logger;
		private readonly RabbitMQConfiguration _rabbitMqConfiguration;

		public ConfigureSitecore(ISitecoreConfiguration configuration, ILogger<ConfigureSitecore> logger)
		{
			_configuration = configuration;
			_logger = logger;

			_rabbitMqConfiguration = new RabbitMQConfiguration();
			_configuration.GetSection(_sectionName).Bind(_rabbitMqConfiguration);
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IModel>(serviceProvider => {
				var connectionFactory = new ConnectionFactory { Uri = new Uri(_rabbitMqConfiguration.ConnectionString) };
				var connection = connectionFactory.CreateConnection();
				return connection.CreateModel();
			});
			
			services.AddHostedService<RabbitMQMessagingService>();
		}
	}
}