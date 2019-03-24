namespace GC.Plugin.Messaging.Alerts.Services
{
	public interface IAlertService
	{
		bool IsEnabled { get; set; }
		string Message { get; set; }
	}	
}