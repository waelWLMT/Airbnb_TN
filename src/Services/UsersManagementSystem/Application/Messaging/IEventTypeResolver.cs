using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Messaging
{
    public interface IEventTypeResolver
    {
        Type Resolve(string eventTypeName);
        bool TryResolve(string eventTypeName, out Type type);
    }
}
