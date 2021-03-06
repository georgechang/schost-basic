using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Sitecore.Framework.Runtime.Configuration;
using Sitecore.Framework.Runtime.Hosting;

namespace ScHost
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

		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();
			app.UseCors((Action<CorsPolicyBuilder>)(policy =>
		   {
			   policy.AllowAnyOrigin();
			   policy.AllowAnyHeader();
			   policy.AllowAnyMethod();
		   }));
		}
	}
}