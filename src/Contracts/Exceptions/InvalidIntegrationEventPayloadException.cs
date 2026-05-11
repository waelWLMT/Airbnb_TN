using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Exceptions
{
    public sealed class InvalidIntegrationEventPayloadException : Exception
    {
        public InvalidIntegrationEventPayloadException(string eventName)
        : base($"Invalid payload for integration event: {eventName}")
        {
        }

        public InvalidIntegrationEventPayloadException(
            string eventName,
            Exception innerException)
            : base(
                $"Invalid payload for integration event: {eventName}",
                innerException)
        {
        }
    }
}
