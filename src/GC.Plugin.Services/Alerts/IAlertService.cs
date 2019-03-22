namespace GC.Plugin.Services.Alerts
{
	public interface IAlertService
	{
		bool IsEnabled { get; set; }
		string Message { get; set; }
	}
}