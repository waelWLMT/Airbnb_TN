using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Messaging.Events
{
    public record UserCreatedEvent(Guid UserId, int idRole);
}
