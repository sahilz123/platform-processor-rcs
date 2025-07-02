using Compunnel.Multichannel.Messaging.Domain;

namespace Compunnel.Multichannel.Messaging.Application.Interface
{
    public interface IMessageRepository
    {
        Task<string?> UpdateMessageStatus(string message_id, MessageData message);

    }
}
