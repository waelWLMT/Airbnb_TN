using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Configurations
{
    public class VoayeurConfiguration : IEntityTypeConfiguration<Voyageur>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Voyageur> builder)
        {
            builder.ToTable("Voyageurs");
            builder.Property(x => x.IsActive).HasDefaultValue(true);            
        }
    }
}
