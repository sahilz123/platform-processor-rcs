using Compunnel.Multichannel.Messaging.Application.Interface;
using Compunnel.Multichannel.Messaging.Domain;
using Compunnel.Multichannel.Messaging.Infrastructure.DatContextContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Compunnel.Multichannel.Messaging.Infrastructure.Data
{
    public class TokenRepository: ITokenRepository
    {
        protected readonly DbContextOptions<ApplicationContext> _dbOrgContextOptions;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<TokenRepository> _logger;

        public TokenRepository(IServiceScopeFactory serviceScopeFactory, IConfiguration configuration, ILogger<TokenRepository> logger)
        {
            _scopeFactory = serviceScopeFactory;
            _dbOrgContextOptions = _scopeFactory.CreateScope().ServiceProvider.GetRequiredService<DbContextOptions<ApplicationContext>>();
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<TokenData?> GetTokens(string orgId)
        {
            try
            {
                string Id = _configuration["DefaultOrgId"]!;
                using (var db = new ApplicationContext(_dbOrgContextOptions))
                {
                    return await db.TokenDbSet
                                    .Where(token => token.OrgId == orgId)
                                    .FirstOrDefaultAsync()
                                    ??
                               await db.TokenDbSet
                                    .Where(t => t.OrgId == Id)
                                    .FirstOrDefaultAsync();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{DateTime}--------------GET TOKENS ERROR :::: {Exception} --------------------------- ", DateTime.Now.ToString(), ex.ToString());
                throw;
            }
        }

    }
}
