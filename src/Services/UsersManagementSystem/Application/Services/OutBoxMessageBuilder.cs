using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Contracts.Events.Proprietaires;
using Contracts.Events.Voyageurs;
using Domain.Enums;
using Domain.Models;

namespace Application.Services
{
    public static class OutBoxMessageBuilder
    {
        #region Voyageur messages
        public static OutboxMessage BuildVoyageurCreatedMessage(User user)
        {
            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = nameof(VoyageurCreatedEvent),
                Content = JsonSerializer.Serialize(new VoyageurCreatedEvent(user.Id)),
                CreatedAt = DateTime.UtcNow,
            };

            return outboxMessage;
        }

        public static OutboxMessage BuildVoyageurActivatedMessage(User user)
        {
            var voyageurActivatedEvent = new VoyageurActivatedEvent(user.Id, user.IsActive);

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = nameof(VoyageurActivatedEvent),
                Content = JsonSerializer.Serialize(voyageurActivatedEvent),
                CreatedAt = DateTime.UtcNow
            };

            return outboxMessage;

        }

        public static OutboxMessage BuildVoyageurDeletedMessage(User user)
        {
            var voyageurDeletedEvent = new VoyageurDeletedEvent(user.Id);
            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = nameof(VoyageurDeletedEvent),
                Content = JsonSerializer.Serialize(voyageurDeletedEvent),
                CreatedAt = DateTime.UtcNow
            };
            return outboxMessage;

        }

        #endregion

        #region Proprietaire messages
        public static OutboxMessage BuildProprietaireCreatedMessage(User user)
        {
            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = nameof(ProprietaireCreatedEvent),
                Content = JsonSerializer.Serialize(new ProprietaireCreatedEvent(user.Id)),
                CreatedAt = DateTime.UtcNow,
            };
            return outboxMessage;
        }

        public static OutboxMessage BuildProprietaireActivatedMessage(User user)
        {
            var proprietaireActivatedEvent = new ProprietaireActivatedEvent(user.Id, user.IsActive);

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = nameof(ProprietaireActivatedEvent),
                Content = JsonSerializer.Serialize(proprietaireActivatedEvent),
                CreatedAt = DateTime.UtcNow
            };

            return outboxMessage;

        }

        public static OutboxMessage BuildProprietaireDeletedMessage(User user)
        {
            var proprietaireDeletedEvent = new ProprietaireDeletedEvent(user.Id);

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                Type = nameof(ProprietaireDeletedEvent),
                Content = JsonSerializer.Serialize(proprietaireDeletedEvent),
                CreatedAt = DateTime.UtcNow
            };

            return outboxMessage;

        }


        #endregion
    }
}
