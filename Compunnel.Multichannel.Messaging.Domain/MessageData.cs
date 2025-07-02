using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compunnel.Multichannel.Messaging.Domain
{
    public class MessageData
    {
        [Key]
        public string MessageId { get; set; } = string.Empty;

        public string TenantId { get; set; } = string.Empty;

        public string FromNumber { get; set; } = string.Empty;

        public string ToNumber { get; set; } = string.Empty;

        public string MessageBody { get; set; } = string.Empty;

        public string ChannelType { get; set; } = string.Empty;

        public DateTime Created { get; set; }

        public string Status { get; set; } = string.Empty;
        public string OrgId { get; set; } = string.Empty;
        public string ToCc { get; set; } = string.Empty;
        public string RcsMessageId { get; set; } = string.Empty;
        public DateTime ModifiedTime { get; set; } = DateTime.Now;
    }

}
