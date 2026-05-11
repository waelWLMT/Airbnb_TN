using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Exceptions
{
    public sealed class UnknownIntegrationEventException : Exception
    {
        public UnknownIntegrationEventException(string eventName) : base(eventName) { } 
        public UnknownIntegrationEventException(string eventName, Exception innerException) : base(eventName, innerException) { }
       
    }
}
