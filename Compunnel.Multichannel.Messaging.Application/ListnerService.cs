using Compunnel.Multichannel.Messaging.Application.Interface;
using Compunnel.Multichannel.Messaging.Domain;
using Microsoft.Extensions.Options;

namespace Compunnel.Multichannel.Messaging.Application
{
    public class ListnerService : IListnerService
    {
        readonly IMessageBrokerService _messageBrokerService;

        readonly string _rcsQueue;
        readonly string _routingKey;

        public ListnerService(IMessageBrokerService messageBrokerService, IOptions<RabbitMQSettings> options)
        {
            _messageBrokerService = messageBrokerService;
            _rcsQueue = options.Value.QueueName;
            _routingKey = options.Value.QueueRoutingKey;
        }

        public async Task ListenRCS()
        {
            await _messageBrokerService.SubscribeMessage(_rcsQueue, _routingKey);
        }

    }
}
