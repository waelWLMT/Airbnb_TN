using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Contracts.Abstractions;
using Contracts.Attributes;

namespace Contracts.Events.Proprietaires
{
    [IntegrationEvent(EventNames.ProprietaireDeleted)]
    public class ProprietaireDeletedEvent : IIntegrationEvent
    {
        public Guid UserId { get; init; }
    }
}
