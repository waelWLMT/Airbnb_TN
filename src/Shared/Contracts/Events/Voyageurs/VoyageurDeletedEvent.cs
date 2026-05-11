using System;
using System.Collections.Generic;
using System.Text;
using Contracts.Abstractions;
using Contracts.Attributes;

namespace Contracts.Events.Voyageurs
{
    [IntegrationEvent(EventNames.VoyageurDeleted)]
    public class VoyageurDeletedEvent: IIntegrationEvent
    {
        public Guid UserId { get; init; }
    }
}
