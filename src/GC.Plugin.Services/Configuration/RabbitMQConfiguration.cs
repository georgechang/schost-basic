namespace GC.Plugin.Services.Configuration
{
	public class RabbitMQConfiguration
	{
		public string ConnectionString { get; set; }

		public string ExchangeName { get; set; }

		public string QueueName { get; set; }

		public int Delay { get; set; }
	}
}