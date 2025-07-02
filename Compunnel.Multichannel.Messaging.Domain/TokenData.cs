using System.ComponentModel.DataAnnotations;

namespace Compunnel.Multichannel.Messaging.Domain
{
    public class TokenData
    {
        public string RCS_Token { get; set; } = string.Empty;
        [Key]
        public string OrgId { get; set; } = string.Empty;
    }
}
