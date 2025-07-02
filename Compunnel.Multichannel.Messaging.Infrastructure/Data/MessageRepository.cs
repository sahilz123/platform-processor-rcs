using Compunnel.Multichannel.Messaging.Infrastructure.DatContextContext;
using Compunnel.Multichannel.Messaging.Application.Interface;
using Compunnel.Multichannel.Messaging.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Compunnel.Multichannel.Messaging.Infrastructure.Data
{
    public class MessageRepository : IMessageRepository
    {
        protected readonly DbContextOptions<ApplicationContext> _dbContextOptions;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<MessageRepository> _logger;

        public MessageRepository(IServiceScopeFactory scopeFactory, ILogger<MessageRepository> logger)
        {
            _scopeFactory = scopeFactory;
            _dbContextOptions = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<DbContextOptions<ApplicationContext>>();
            _logger = logger;
        }

        public async Task<string?> UpdateMessageStatus(string message_id, MessageData message)
        {
            try
            {
                using (var db = new ApplicationContext(_dbContextOptions))
                {
                    var t = await db.MessageDbSet.FindAsync(message_id);
                    t!.Status = message.Status;
                    t.RcsMessageId = message.RcsMessageId;
                    await db.SaveChangesAsync();
                    return message_id;
                }
            }
            catch(Exception ex) 
            {
                _logger.LogError(ex, "{DateTime}--------------UPDATE MESSAGE STATUS ERROR :::: {Exception} --------------------------- ", DateTime.Now.ToString(), ex.ToString());
                return ex.Message.ToString();
            }
        }
       
    }
}
