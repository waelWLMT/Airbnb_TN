using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
namespace Messaging
{
    public static class DependencyInjection
    {
        public static void RegisterMessaging(this IServiceCollection services, IConfigurationSection configration)
        {
            services.AddMassTransit(x=>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configration["Host"], "/", h =>
                    {
                        h.Username(configration["Username"]);
                        h.Password(configration["Password"]);
                    });
                });
            });           
        }
    }
}
