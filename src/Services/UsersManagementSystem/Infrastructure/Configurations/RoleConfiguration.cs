using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Configurations
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role
                {
                    Id = 1,
                    Code = "Admin",
                    Libelle = "Administrateur"
                },
                new Role
                {
                    Id = 2,
                    Code = "Voyageur",
                    Libelle = "Voyageur"
                },
                new Role
                {
                    Id = 3,
                    Code = "Proprietaire",
                    Libelle = "Proprietaire"
                }
            );
        }
    }
}
