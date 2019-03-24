using GC.Plugin.Messaging.Alerts.MessageTypes;
using GC.Plugin.Messaging.Alerts.Services;
using GC.Plugin.Messaging.Services.Processors;

namespace GC.Plugin.Messaging.Alerts
{
	public class AlertMessageProcessor : IMessageProcessor
	{
		private readonly IAlertService _alertService;

		public AlertMessageProcessor(IAlertService alertService)
		{
			_alertService = alertService;
		}

		public void Process(object message)
		{
			var alert = message as AlertMessage;

			_alertService.IsEnabled = alert.Enabled;
			_alertService.Message = alert.Message;
		}
	}
}