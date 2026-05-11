using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Contracts.Exceptions;
using Contracts.Registry;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Models;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using static MassTransit.Monitoring.Performance.BuiltInCounters;

namespace Infrastructure.Messaging
{
    public class OutboxWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<OutboxWorker> _logger;
        private readonly TimeSpan _delay = TimeSpan.FromMinutes(4);
        private readonly int _batchSize = 20;
        private readonly int _maxRetryCount = 5;
        public OutboxWorker(IServiceScopeFactory serviceScopeFactory, ILogger<OutboxWorker> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }
        private async Task LockMessagesAsync(List<OutboxMessage> messages, IUnitOfWork unitOfWork)
        {
            foreach (var message in messages)
            {
                message.LockedUntil = DateTime.UtcNow.AddMinutes(7);
                message.Status = OutboxStatus.Processing;
            }

            await unitOfWork.CommitAsync();
        }
        private void HandleRetryMessage(OutboxMessage message, Exception ex)
        {
            message.RetryCount++;

            if (message.RetryCount >= _maxRetryCount)
                HandleDeadLetterMessage(message, ex);
            else
            {
                message.LastError = ex.Message;
                message.Status = OutboxStatus.Retrying;
            }
        }
        private void HandlePublishedMessage(OutboxMessage message)
        {
            message.ProcessedAt = DateTime.UtcNow;
            message.LastError = null;
            message.Status = OutboxStatus.Published;
            message.LockedUntil = null; // Clear processing time after successful publish
        }
        private string GetExceptionMessageError(Exception ex)
        {
            if (ex is UnknownIntegrationEventException)
                return "Unknown Integration Event Exception: " + ex.Message;

            else if (ex is JsonException)
                return "JSON deserialization Exception: " + ex.Message;

            else if (ex is InvalidIntegrationEventPayloadException)
                return "Invalid payload for event: " + ex.Message;
            else
                return "Error while processing outbox message: " + ex.Message;

        }
        private void HandleDeadLetterMessage(OutboxMessage message, Exception ex)
        {
            message.LastError = GetExceptionMessageError(ex);
            message.Status = OutboxStatus.DeadLetter; // Mark as dead letter since this is likely a configuration issue that won't resolve with retries
            message.ProcessedAt = DateTime.UtcNow; // Mark as processed to avoid infinite retries
            message.LockedUntil = null; // Clear processing time to allow retrying in the next cycle if needed
        }
        private async Task ProcessMessagesAsync(List<OutboxMessage> messages, IPublishEndpoint publishEndPoint, CancellationToken stoppingToken)
        {
            foreach (var message in messages)
            {
                try
                {
                    var eventType = IntegrationEventRegistry.Resolve(message.EventName);

                    var eventObj = JsonSerializer.Deserialize(message.Content, eventType);

                    if (eventObj is null)
                        throw new InvalidIntegrationEventPayloadException(message.EventName);

                    _logger.LogInformation("Publishing integration event {EventName} with message id {MessageId}", message.EventName, message.Id);

                    await publishEndPoint.Publish(eventObj, stoppingToken);

                    HandlePublishedMessage(message);

                    _logger.LogInformation("Successfully published event {EventName} with message id {MessageId}", message.EventName, message.Id);


                }
                catch (InvalidIntegrationEventPayloadException ex)
                {
                    HandleDeadLetterMessage(message, ex);
                    _logger.LogError(
                        ex,
                        "Invalid payload for event {EventName} in outbox message {MessageId}",
                        message.EventName,
                        message.Id);
                }
                catch (UnknownIntegrationEventException ex)
                {
                    HandleDeadLetterMessage(message, ex);
                    _logger.LogError(
                        ex,
                        "Unknown event type {EventName} for outbox message {MessageId}",
                        message.EventName,
                        message.Id);
                }
                catch (JsonException jsonEx)
                {
                    HandleDeadLetterMessage(message, jsonEx);
                    _logger.LogError(
                        jsonEx,
                        "JSON deserialization error while processing outbox message {MessageId} for event {EventName}",
                        message.Id,
                        message.EventName);
                }
                catch (Exception ex)
                {
                    HandleRetryMessage(message, ex);
                    _logger.LogError(
                        ex,
                        "Error while processing outbox message {MessageId} for event {EventName}",
                        message.Id,
                        message.EventName
                        );
                }
            }

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                 using var scope = _serviceScopeFactory.CreateScope();

                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var messageReadRepo = scope.ServiceProvider.GetRequiredService<IOutboxMessageReadRepository>();
                var publishEndPoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
                var messages = await messageReadRepo.GetPendingMessagesAsync(_batchSize);

                _logger.LogInformation("OutboxWorker found {MessageCount} messages to process", messages.Count);

                if (messages.Any())
                {
                    await LockMessagesAsync(messages, unitOfWork);
                    await ProcessMessagesAsync(messages, publishEndPoint, stoppingToken);
                    await unitOfWork.CommitAsync();
                }
                   
                await Task.Delay(_delay, stoppingToken);
            }
        }

    }
}
