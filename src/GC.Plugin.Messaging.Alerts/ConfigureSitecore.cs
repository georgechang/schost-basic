using System;
using System.IO;
using GC.Plugin.Messaging.Alerts.Services;
using GC.Plugin.Messaging.Alerts.TagHelpers;
using GC.Plugin.Messaging.Services.Processors;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Sitecore.Framework.Runtime.Configuration;
using Sitecore.Framework.Runtime.Hosting;
using Sitecore.Framework.Runtime.Plugins;

namespace GC.Plugin.Messaging.Alerts
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
			services.AddSingleton<ITagHelperComponent, AlertTagHelper>();
			services.AddTransient<IMessageProcessor, AlertMessageProcessor>();
		}

		public void Configure(IApplicationBuilder app, ISitecorePluginManager pluginManager, ISitecoreHostingEnvironment hostingEnvironment)
		{
			var plugin = pluginManager.Resolve(this);
			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), plugin.Path), "Content")),
				RequestPath = $"/{ plugin.PluginName }"
			});
		}
	}
}