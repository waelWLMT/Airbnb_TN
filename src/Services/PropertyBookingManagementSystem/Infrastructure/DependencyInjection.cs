using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Events.Voyageurs;
using Domain.Interfaces;
using Infrastructure.Messaging.Consumers.Proprietaires;
using Infrastructure.Messaging.Consumers.Voyageurs;

using Infrastructure.Persistence;
using Infrastructure.Repositories;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Inject dbContext
            services.AddDbContext<PropertyBookingDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Inject unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Inject generic repositories
            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));


            

            // Inject specific read repositories
            services.AddScoped<ILogementReadRepository, LogementReadRepository>();
            services.AddScoped<IVoyageurReadRepository, VoyageurReadRepository>();
            services.AddScoped<IProprietaireReadRepository, ProprietaireReadRepository>();


            // add specific wrte repositories
            services.AddScoped<IProprietaireWriteRepository, ProprietaireWriteRepository>();
            services.AddScoped<IVoyageurWriteRepository, VoyageurWriteRepository>();
            services.AddScoped<ILogementWriteRepository, LogementWriteRepository>();

            // Add MassTransit configuration here
            services.AddMassTransit(x =>
            {
                // Register the consumer
                x.AddConsumer<ProprietaireCreatedConsumer>();
                x.AddConsumer<ProprietaireActivatedConsumer>();
                x.AddConsumer<ProprietaireDeletedConsumer>();

                x.AddConsumer<VoyageurCreatedConsumer>();
                x.AddConsumer<VoyageurActivatedConsumer>();
                x.AddConsumer<VoyageurDeletedConsumer>();

                // Configure the bus
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host("localhost", "/", h =>
                    {
                        h.Username("guest");
                        h.Password("guest");
                    });

                    cfg.UseMessageRetry(r =>
                    {
                        r.Interval(3, TimeSpan.FromSeconds(5));
                    });

                    cfg.ConfigureEndpoints(context);

                });
            });

            return services;
        }
   }
}
