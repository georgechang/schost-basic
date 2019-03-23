namespace GC.Plugin.Messaging.Services.Alerts
{
	public class AlertService : IAlertService
	{
		public bool IsEnabled { get; set; }
		public string Message { get; set; }
	}
}