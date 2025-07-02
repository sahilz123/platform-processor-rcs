using Compunnel.Multichannel.Messaging.Domain;

namespace Compunnel.Multichannel.Messaging.Application.Interface
{
    public interface IMessageProcessor
    {
        Task ProcessRCS(MessageData messageData);

    }
}
