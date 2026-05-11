using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Abstractions;
using Contracts.Attributes;

namespace Contracts.Events.Voyageurs
{
    [IntegrationEvent(EventNames.VoyageurActivated)]
    public class VoyageurActivatedEvent : IIntegrationEvent
    {
        public Guid UserId { get; init; }
        public bool IsActivated { get; init; }
    }
}
