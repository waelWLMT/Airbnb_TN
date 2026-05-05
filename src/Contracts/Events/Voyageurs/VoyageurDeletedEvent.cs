using System;
using System.Collections.Generic;
using System.Text;

namespace Contracts.Events.Voyageurs
{
    public record VoyageurDeletedEvent(Guid UserId);
}
