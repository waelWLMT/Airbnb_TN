using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Interfaces;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
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

            return services;
        }
    }
}
