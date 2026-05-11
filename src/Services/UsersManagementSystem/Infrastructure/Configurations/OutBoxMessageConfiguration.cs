using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configurations
{
    public class OutBoxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.Property(x => x.EventName)
                   .IsRequired();

            builder.Property(x => x.Content)
                .IsRequired();

            builder.HasIndex(x => x.CorrelationId);

            builder.HasIndex(x=> new { x.Status, x.LockedUntil });
        }
    }
}
