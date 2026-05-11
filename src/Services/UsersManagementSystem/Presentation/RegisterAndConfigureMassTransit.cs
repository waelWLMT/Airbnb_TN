using Domain.Interfaces;
using MassTransit;

namespace Presentation
{
    public static class RegisterAndConfigureMassTransit
    {
        public static void AddMassTransitConfigurationExtension(this IServiceCollection services, IConfiguration configuration)
        {
            // Register Messaging services
            services.AddMassTransit(x =>
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMQ:Host"], "/", h =>
                    {
                        h.Username(configuration["RabbitMQ:Username"]);
                        h.Password(configuration["RabbitMQ:Password"]);
                    });

                    cfg.ConfigureSend(sendCfg =>
                    {
                        sendCfg.UseExecute(sendContext =>
                        {
                            var correlationContext = context.GetRequiredService<ICorrelationContext>();

                            sendContext.CorrelationId = correlationContext.CorrelationId;
                        });
                    });

                    cfg.ConfigurePublish(publishCfg =>
                    {
                        publishCfg.UseExecute(publishContext =>
                        {
                            var correlationContext = context.GetRequiredService<ICorrelationContext>();
                            publishContext.CorrelationId = correlationContext.CorrelationId;
                        });
                    });


                    cfg.ConfigureEndpoints(context);
                });
            });


        }
    }
}