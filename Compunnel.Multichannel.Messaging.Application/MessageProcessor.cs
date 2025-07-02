using Compunnel.Multichannel.Messaging.Application.Interface;
using Compunnel.Multichannel.Messaging.Domain;
using Microsoft.Extensions.Logging;

namespace Compunnel.Multichannel.Messaging.Application
{
    public class MessageProcessor : IMessageProcessor
    {
        //readonly ITwilioService _twilioService;
        readonly IMessageRepository _messageRepository;
        readonly ITokenRepository _tokenRepository;
        readonly ILogger<MessageProcessor> _logger;

        public MessageProcessor( IMessageRepository messageRepository, ITokenRepository tokenRepository, ILogger<MessageProcessor> logger)
        {
            //_twilioService = twilioService;
            _messageRepository = messageRepository;
            _tokenRepository = tokenRepository;
            _logger = logger;
        }

        public async Task ProcessRCS(MessageData messageData)
        {
            try
            {
                var token = await _tokenRepository.GetTokens(messageData.OrgId);

               

                //var result = await InfobipService.SendRCS(messageData, newSetting);
                //if (result.RcsSent)
                //{
                //    messageData.Status = "Dispatched";
                //    messageData.RcsMessageId = result.RcsMessageId!;
                //}
                //else
                //{
                //    messageData.Status = "Failed";
                //}
                await _messageRepository.UpdateMessageStatus(messageData.MessageId, messageData);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{DateTime}--------------PROCESS RCS ERROR :::: {Exception} --------------------------- ", DateTime.Now.ToString(), ex.ToString());
                Console.WriteLine(ex.ToString());
            }
        }
    }
}