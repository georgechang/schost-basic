namespace GC.Plugin.Messaging.Services.Processors
{
	public interface IMessageProcessor
	{
		void Process(object message);
	}
}