using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ProprietaireConfiguration : IEntityTypeConfiguration<Proprietaire>
    {
        public void Configure(EntityTypeBuilder<Proprietaire> builder)
        {

            builder.ToTable("Proprietaires");

            builder
                .Property(x => x.IsActive)
                .HasDefaultValue(true);
        }
    }
}
