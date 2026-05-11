using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Abstractions;
using Contracts.Attributes;

namespace Contracts.Events.Voyageurs
{
    [IntegrationEvent(EventNames.VoyageurCreated)]
    public class VoyageurCreatedEvent : IIntegrationEvent
    {
        public Guid UserId { get; init; }
    }
}
