using Compunnel.Multichannel.Messaging.Application.Interface;
using Compunnel.Multichannel.Messaging.Domain;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace Compunnel.Multichannel.Messaging.Infrastructure.MessageBroker
{
    public class RabbitMQMessageBrokerService : IMessageBrokerService
    {
        private readonly IConnection _connection;
        private IModel? _channel;
        private EventingBasicConsumer? _consumer;
        private readonly CancellationToken _cancellationToken;

        private readonly Channel<MessageData> _channelSubscribed_RCS;
        private readonly ILogger<RabbitMQMessageBrokerService> _logger;

        public RabbitMQMessageBrokerService(Channel<MessageData> channelSubscribed_RCS, IOptions<RabbitMQSettings> options, ILogger<RabbitMQMessageBrokerService> logger)
        {
            _logger= logger;
            _cancellationToken = new CancellationToken();
            _channelSubscribed_RCS = channelSubscribed_RCS;

            var _connectionFactory = new ConnectionFactory
            {
                HostName = options.Value.HostName,
                Port = options.Value.Port
            };
            _connection = _connectionFactory.CreateConnection();
        }
        public void PublishExchangeMessage(string exchangeName, string routingKey, string message)
        {
            using (_channel = _connection.CreateModel())
            {
                var body = Encoding.UTF8.GetBytes(message);

                _channel.BasicPublish(exchange: exchangeName,
                                     routingKey: routingKey,
                                     basicProperties: null,
                                     body: body);
            }
        }

        public Task SubscribeMessage(string exchangeQueueName, string routingKey)
        {

            _channel = _connection.CreateModel();

            var _result = new Task(
                    () =>
                    {

                        _consumer = new EventingBasicConsumer(_channel);
                        _channel.BasicConsume(
                                               queue: exchangeQueueName,
                                               autoAck: false,
                                               consumer: _consumer);

                        RunConsumeLoop();
                    },
                    _cancellationToken,
                    TaskCreationOptions.LongRunning);

            if(_result.IsCompleted)
            {
                return _result;
            }
            _result.Start();

            return Task.CompletedTask;

        }

        private void RunConsumeLoop()
        {
            _consumer!.Received += (model, basicDeliverEventArgs) =>
            {
                try
                {
                    var body = basicDeliverEventArgs.Body.ToArray();
                    var response = Encoding.UTF8.GetString(body);

                    var messageData = JsonSerializer.Deserialize<MessageData>(response);
                    
                    _channelSubscribed_RCS.Writer.WriteAsync(messageData!);

                    _channel!.BasicAck(basicDeliverEventArgs.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _channel!.BasicNack(basicDeliverEventArgs.DeliveryTag, false, false);
                    _logger.LogError(ex, "{DateTime}--------------RABBITMQ EVENT LISTNER ERROR :::: {Exception} ---------------------------", DateTime.Now.ToString(), ex.ToString());
                }
            };

            while (!_cancellationToken.IsCancellationRequested)
            {
                Thread.Sleep(100);
            }
        }
    }
}
