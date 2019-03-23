namespace GC.Plugin.Messaging.Services.Alerts
{
	public interface IAlertService
	{
		bool IsEnabled { get; set; }
		string Message { get; set; }
	}
}