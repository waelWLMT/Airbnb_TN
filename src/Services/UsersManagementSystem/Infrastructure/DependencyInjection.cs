using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Messaging;
using Domain.Interfaces;
using Infrastructure.Messaging;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static void RegisterInfrastructure(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<UsersDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            services.AddScoped(typeof(IReadRepository<>), typeof(ReadRepository<>));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            services.AddScoped<IOutboxMessageRepository, OutboxMessageRepository>();
            services.AddScoped<IOutboxMessageReadRepository, OutBoxMessageReadRepository>();
            services.AddScoped<IUserReadRepository, UserReadRepository>();
            services.AddScoped<IUserWriteRepository, UserWriteRepository>();
            services.AddScoped<IRoleReadRepository, RoleReadRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();



            // add eventType resolver
            services.AddSingleton<IEventTypeResolver, EventTypeResolver>();

            // worker for outbox pattern
            services.AddHostedService<OutboxWorker>();



        }
    }
}
