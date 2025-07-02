using Compunnel.Multichannel.Messaging.Domain;
using Microsoft.EntityFrameworkCore;

namespace Compunnel.Multichannel.Messaging.Infrastructure.DatContextContext
{
    public class ApplicationContext : DbContext
    {
       

        public ApplicationContext(DbContextOptions _dbContextOptions) : base(_dbContextOptions)
        {
           
        }

        public DbSet<MessageData> MessageDbSet { get; set; }
        public DbSet<TokenData> TokenDbSet { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MessageData>(e => e.ToTable("MessageDetails"));
            modelBuilder.Entity<MessageData>(entity =>
            {
                entity.Property(e => e.MessageId).IsRequired().HasColumnName("message_id");
                entity.Property(e => e.TenantId).IsRequired().HasColumnName("tenant_id");
                entity.Property(e => e.FromNumber).IsRequired().HasColumnName("from_number");
                entity.Property(e => e.ToNumber).IsRequired().HasColumnName("to_number");
                entity.Property(e => e.MessageBody).IsRequired().HasColumnName("message_body");
                entity.Property(e => e.ChannelType).IsRequired().HasColumnName("channel_type");
                entity.Property(e => e.Status).IsRequired().HasColumnName("status");
                entity.Property(e => e.Created).IsRequired().HasColumnName("created_at");
                entity.Property(e => e.OrgId).IsRequired().HasColumnName("orgid");
                entity.Property(e => e.ToCc).HasColumnName("to_cc");
                entity.Property(e => e.RcsMessageId).IsRequired().HasColumnName("event_message_id");
                entity.Property(e => e.ModifiedTime).HasColumnName("modified_time");
            });

            modelBuilder.Entity<TokenData>(e => e.ToTable("OrgConfiguration"));
            modelBuilder.Entity<TokenData>(entity =>
            {                
                entity.Property(e => e.RCS_Token).HasColumnName("rcs_token");
                entity.Property(e => e.OrgId).HasColumnName("org_id");
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
