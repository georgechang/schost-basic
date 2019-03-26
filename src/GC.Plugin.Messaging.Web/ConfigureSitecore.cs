using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
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
			services.AddMvc();
		}

		public void Configure(IApplicationBuilder app, ISitecorePluginManager pluginManager, ISitecoreHostingEnvironment hostingEnvironment)
		{
			var plugin = pluginManager.Resolve(this);
			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(Path.Combine(plugin.Path, "Content")),
				RequestPath = $"/{ plugin.PluginName }"
			});
			app.UseMvc((Action<IRouteBuilder>)(routes => routes.MapRoute("default", "{controller=Home}/{action=Index}/{id?}")));
		}
	}
}