using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Contracts.Events;
using Contracts.Events.Proprietaires;
using Contracts.Events.Voyageurs;
using Domain.Enums;
using Domain.Models;

namespace Application.Services
{
    public static class OutBoxMessageBuilder
    {
        #region Voyageur messages
        public static OutboxMessage BuildVoyageurCreatedMessage(User user, Guid correlationId)
        {
            var voyageurCreatedEvent =  new VoyageurCreatedEvent { UserId = user.Id };

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                CorrelationId = correlationId,
                EventName =  EventNames.VoyageurCreated,
                Content = JsonSerializer.Serialize(voyageurCreatedEvent),
                CreatedAt = DateTime.UtcNow,
                Status = OutboxStatus.Pending
            };

            return outboxMessage;
        }
        public static OutboxMessage BuildVoyageurActivatedMessage(User user, Guid correlationId)
        {
            var voyageurActivatedEvent = new VoyageurActivatedEvent { UserId = user.Id, IsActivated = user.IsActive};

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                CorrelationId = correlationId,
                EventName = EventNames.VoyageurActivated,
                Content = JsonSerializer.Serialize(voyageurActivatedEvent),
                CreatedAt = DateTime.UtcNow,
                Status = OutboxStatus.Pending
            };

            return outboxMessage;

        }
        public static OutboxMessage BuildVoyageurDeletedMessage(User user, Guid correlationId)
        {
            var voyageurDeletedEvent = new VoyageurDeletedEvent{ UserId = user.Id };
            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                CorrelationId = correlationId,
                EventName = EventNames.VoyageurDeleted,
                Content = JsonSerializer.Serialize(voyageurDeletedEvent),
                CreatedAt = DateTime.UtcNow,
                Status = OutboxStatus.Pending
            };
            return outboxMessage;

        }

        #endregion

        #region Proprietaire messages
        public static OutboxMessage BuildProprietaireCreatedMessage(User user, Guid correlationId)
        {

            var proprietaireCreatedEvent = new ProprietaireCreatedEvent { UserId = user.Id };

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                CorrelationId = correlationId,
                EventName = EventNames.ProprietaireCreated,
                Content = JsonSerializer.Serialize(proprietaireCreatedEvent),
                CreatedAt = DateTime.UtcNow,
                Status = OutboxStatus.Pending
            };
            return outboxMessage;
        }
        public static OutboxMessage BuildProprietaireActivatedMessage(User user, Guid correlationId)
        {
            var proprietaireActivatedEvent = new ProprietaireActivatedEvent { UserId = user.Id, IsActivated = user.IsActive };

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                CorrelationId = correlationId,
                EventName = EventNames.ProprietaireActivated,
                Content = JsonSerializer.Serialize(proprietaireActivatedEvent),
                CreatedAt = DateTime.UtcNow,
                Status = OutboxStatus.Pending
            };

            return outboxMessage;

        }
        public static OutboxMessage BuildProprietaireDeletedMessage(User user, Guid correlationId)
        {
            var proprietaireDeletedEvent = new ProprietaireDeletedEvent { UserId = user.Id };

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                CorrelationId = correlationId,
                EventName = EventNames.ProprietaireDeleted,
                Content = JsonSerializer.Serialize(proprietaireDeletedEvent),
                CreatedAt = DateTime.UtcNow,
                Status = OutboxStatus.Pending
            };

            return outboxMessage;

        }

        #endregion
    }
}
