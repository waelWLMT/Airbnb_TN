using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Abstractions;
using Contracts.Attributes;

namespace Contracts.Events.Proprietaires
{


    [IntegrationEvent(EventNames.ProprietaireCreated)]
    public class ProprietaireCreatedEvent : IIntegrationEvent
    {
        public Guid UserId { get; init; }
    }
}