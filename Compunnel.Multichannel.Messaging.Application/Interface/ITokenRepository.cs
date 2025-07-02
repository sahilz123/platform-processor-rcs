using Compunnel.Multichannel.Messaging.Domain;

namespace Compunnel.Multichannel.Messaging.Application.Interface
{
    public interface ITokenRepository
    {
        Task<TokenData?> GetTokens(string orgId);
    }
}
