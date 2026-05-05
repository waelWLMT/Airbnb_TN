using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Messaging;
using Domain.Interfaces;
using Domain.Models;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using static MassTransit.Monitoring.Performance.BuiltInCounters;

namespace Infrastructure.Messaging
{
    public class OutboxWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private readonly TimeSpan _delay = TimeSpan.FromSeconds(60);
        private readonly int _batchSize = 20;
        private readonly int _maxRetryCount = 5;
        public OutboxWorker(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory; 
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using var scope = _serviceScopeFactory.CreateScope();
                
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var messageReadRepo = scope.ServiceProvider.GetRequiredService<IOutboxMessageReadRepository>();
                var writeRepo = scope.ServiceProvider.GetRequiredService<IRepository<OutboxMessage>>();
                var publishEndPoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();


                var eventTypeResolver = scope.ServiceProvider.GetRequiredService<IEventTypeResolver>();
                var messages = await messageReadRepo.GetNotProcessededMessages(_batchSize);
                Console.WriteLine($"OutboxWorker found {messages.Count} messages to process.");

                foreach (var message in messages)
                {
                    try
                    {
                        
                        //var eventObj = JsonSerializer.Deserialize<object>(message.Content);

                        var eventType = eventTypeResolver.Resolve(message.Type);

                        //if(eventType == null)
                        //    throw new Exception("Invalid event type.");

                        var eventObj = JsonSerializer.Deserialize(message.Content, eventType);
                       
                        if (eventObj == null)
                            throw new Exception("Invalid event payload.");

                        await publishEndPoint.Publish(eventObj, stoppingToken);
                        message.ProcessedAt = DateTime.UtcNow;
                        message.LastError = null;
                    }
                    catch (Exception ex)
                    {
                        message.LastError = ex.Message;
                        message.RetryCount += 1;
                    }
                    finally
                    {
                        await writeRepo.UpdateAsync(message);
                        if(message.RetryCount > _maxRetryCount)
                            message.ProcessedAt = DateTime.UtcNow; // Mark as processed to avoid infinite retries
                    }
                }

                await unitOfWork.CommitAsync();
                await Task.Delay(_delay, stoppingToken);

            }
        }

    }
}
