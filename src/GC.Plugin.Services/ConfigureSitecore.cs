using System;
using System.IO;
using GC.Plugin.Services.Alerts;
using GC.Plugin.Services.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Sitecore.Framework.Runtime.Configuration;
using Sitecore.Framework.Runtime.Hosting;
using Sitecore.Framework.Runtime.Plugins;

namespace GC.Plugin.Services
{
	public class ConfigureSitecore
	{
		private readonly ISitecoreConfiguration _configuration;
		private readonly ILogger<ConfigureSitecore> _logger;

		public ConfigureSitecore(ISitecoreConfiguration configuration, ILogger<ConfigureSitecore> logger)
		{
			_configuration = configuration;
			_logger = logger;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<IAlertService, AlertService>();
			services.AddSingleton<IMessagingService, MessagingService>();
		}

		public void Configure(IMessagingService messagingService)
		{
			messagingService.Execute();
		}
	}
}