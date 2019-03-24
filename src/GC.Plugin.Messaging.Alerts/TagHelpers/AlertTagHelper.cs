using System;
using System.Threading.Tasks;
using GC.Plugin.Messaging.Alerts.Services;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Sitecore.Framework.Runtime.FileProvider;
using Sitecore.Framework.Runtime.Hosting;
using Sitecore.Framework.Runtime.Plugins;

namespace GC.Plugin.Messaging.Alerts.TagHelpers
{
	public class AlertTagHelper : TagHelperComponent
	{
		private readonly ISitecorePluginManager _pluginManager;
		private readonly IAlertService _alertService;

		public AlertTagHelper(ISitecorePluginManager pluginManager, IAlertService alertService)
		{
			_pluginManager = pluginManager;
			_alertService = alertService;
		}

		public override Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
		{
			if (_alertService.IsEnabled)
			{
				var plugin = _pluginManager.Resolve(this);
				if (string.Equals(context.TagName, "head", StringComparison.OrdinalIgnoreCase))
				{
					string styles = $"<link rel=\"stylesheet\" href=\"/{ plugin.PluginName }/css/alert.css\">";
					output.PostContent.AppendHtmlLine(styles);
				}

				if (string.Equals(context.TagName, "body", StringComparison.OrdinalIgnoreCase))
				{
					string script = $"<script src=\"/{ plugin.PluginName }/js/alert.js\" data-message=\"{ _alertService.Message }\"></script>";
					output.PostContent.AppendHtmlLine(script);
				}
			}
			return Task.CompletedTask;
		}
	}
	
}