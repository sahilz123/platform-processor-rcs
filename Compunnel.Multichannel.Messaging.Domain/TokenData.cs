using System.ComponentModel.DataAnnotations;

namespace Compunnel.Multichannel.Messaging.Domain
{
    public class TokenData
    {
        public string Twilio_Auth { get; set; } = string.Empty;
        public string Twilio_Sid { get; set; } = string.Empty;
        public string Twilio_Vid { get; set; } = string.Empty;
        [Key]
        public string OrgId { get; set; } = string.Empty;
    }
}
