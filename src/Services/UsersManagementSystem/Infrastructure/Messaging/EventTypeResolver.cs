using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Messaging;
using Contracts.Events.Proprietaires;
using Contracts.Events.Voyageurs;

namespace Infrastructure.Messaging
{
    public class EventTypeResolver : IEventTypeResolver
    {
        private readonly Dictionary<string, Type> _eventTypes;

        public EventTypeResolver()
        {
            _eventTypes = new Dictionary<string, Type>
            {
                { nameof(ProprietaireActivatedEvent), typeof(ProprietaireActivatedEvent) },
                { nameof(ProprietaireCreatedEvent), typeof(ProprietaireCreatedEvent) },
                { nameof(ProprietaireDeletedEvent), typeof(ProprietaireDeletedEvent) },

                { nameof(VoyageurActivatedEvent), typeof(VoyageurActivatedEvent) },
                { nameof(VoyageurCreatedEvent), typeof(VoyageurCreatedEvent) },
                { nameof(VoyageurDeletedEvent), typeof(VoyageurDeletedEvent) }
            };
        }

        public Type Resolve(string eventTypeName)
        {
            if (!_eventTypes.TryGetValue(eventTypeName, out var type))
                throw new ArgumentException($"Event type: {eventTypeName} introuvable");

            return type;
        }

        public bool TryResolve(string eventTypeName, out Type type)
        {
            return _eventTypes.TryGetValue(eventTypeName, out type);
        }

    }
}
