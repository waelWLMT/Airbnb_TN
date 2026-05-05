using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.ComplexeType;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class LogementConfiguration : IEntityTypeConfiguration<Logement>
    {
        public void Configure(EntityTypeBuilder<Logement> builder)
        {
            builder
                .ComplexProperty(x=> x.Adresse);
        }
    }
}
