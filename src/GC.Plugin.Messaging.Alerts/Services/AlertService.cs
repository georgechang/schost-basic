namespace GC.Plugin.Messaging.Alerts.Services
{
	public class AlertService : IAlertService
	{
		public bool IsEnabled { get; set; }
		public string Message { get; set; }
	}
}