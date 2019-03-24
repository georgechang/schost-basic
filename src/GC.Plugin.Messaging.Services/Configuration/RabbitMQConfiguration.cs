using System;

namespace GC.Plugin.Messaging.Services.Configuration
{
	public class RabbitMQConfiguration
	{
		public string ConnectionString { get; set; }

		public string ExchangeName { get; set; }

		public int Delay { get; set; }

		public Type MessageType { get; set;}
	}
}