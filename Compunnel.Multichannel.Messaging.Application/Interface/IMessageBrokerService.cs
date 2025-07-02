namespace Compunnel.Multichannel.Messaging.Application.Interface
{
    public interface IMessageBrokerService
    {
        void PublishExchangeMessage(string exchangeName, string routingKey, string message);

        Task SubscribeMessage(string exchangeQueueName, string routingKey);
    }
}
