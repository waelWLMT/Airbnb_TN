using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Events.Voyageurs
{
    public record VoyageurActivatedEvent(Guid UserId, bool IsActive);
}
