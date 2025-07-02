using Compunnel.Multichannel.Messaging.Application.Interface;
using Compunnel.Multichannel.Messaging.Domain;
using System.Threading.Channels;

namespace Compunnel.Multichannel.Messaging.BackgroudJob.Host_RCS
{
    public class RcsWorker : BackgroundService
    {
        private readonly ILogger<RcsWorker> _logger;
        readonly IMessageProcessor _messageProcessor;
        private readonly Channel<MessageData> _channelSubscribedExchangeMessage;

        public RcsWorker(ILogger<RcsWorker> logger, IListnerService listnerService,
            Channel<MessageData> channelSubscribedExchangeMessage, IMessageProcessor messageProcessor)
        {
            _logger = logger;
            listnerService.ListenRCS();
            _messageProcessor = messageProcessor;
            _channelSubscribedExchangeMessage = channelSubscribedExchangeMessage;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("-----------------------------Worker ** RCS ** running at: {Time} : WAITING FOR MSG ------------------------------------------", DateTimeOffset.Now);

                if (await _channelSubscribedExchangeMessage.Reader.WaitToReadAsync(stoppingToken))
                {
                    while (_channelSubscribedExchangeMessage.Reader.TryRead(out MessageData? messageData))
                    {
                        await _messageProcessor.ProcessRCS(messageData);
                        _logger.LogCritical("Processed The MESSAGE : for Subscriber --- {TenantId} and MessageId --- {MessageId}"
                                                + "Sender :: {FromNumber} and ToNumber :: {ToNumber} "
                                                + "Content =>> {Body}", messageData.TenantId, messageData.MessageId, messageData.FromNumber, messageData.ToNumber, messageData.MessageBody);

                    }
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
