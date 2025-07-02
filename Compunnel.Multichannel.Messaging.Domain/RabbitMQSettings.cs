namespace Compunnel.Multichannel.Messaging.Domain
{
    public class RabbitMQSettings
    {

        public string HostName { get; set; } = string.Empty;
        public string QueueName { get; set; } = string.Empty;
        public string QueueRoutingKey { get; set; } = string.Empty;
        public int Port { get; set; } 
    }
}
