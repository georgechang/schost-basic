using System;
using System.IO;
using GC.Plugin.Messaging.Web.TagHelpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Sitecore.Framework.Runtime.Configuration;
using Sitecore.Framework.Runtime.Hosting;
using Sitecore.Framework.Runtime.Plugins;

namespace GC.Plugin.Messaging.Web
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
			services.AddSingleton<ITagHelperComponent, AlertTagHelper>();
		}

		public void Configure(IApplicationBuilder app, ISitecorePluginManager pluginManager, ISitecoreHostingEnvironment hostingEnvironment)
		{
			var plugin = pluginManager.Resolve(this);
			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(Path.Combine(Directory.GetCurrentDirectory(), plugin.Path), "content")),
				RequestPath = $"/{ plugin.PluginName }"
			});
		}
	}
}