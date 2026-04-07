using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;
using Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence
{
    public class PropertyBookingDbContext : DbContext
    {
        public DbSet<Logement> Logements { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Locataire> Locataires { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<JourReservation> JourReservations { get; set; }

        public PropertyBookingDbContext(DbContextOptions<PropertyBookingDbContext> options) : base(options)
        { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new LogementConfiguration());
        }



    }
}
