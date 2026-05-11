using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Contracts.Abstractions;
using Contracts.Attributes;

namespace Contracts.Registry
{
    public static class IntegrationEventRegistry
    {
        public static readonly IReadOnlyDictionary<string, Type> _eventTypes =
        typeof(IIntegrationEvent).Assembly
            .GetTypes()
            .Where(t => typeof(IIntegrationEvent).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract)
            .ToDictionary(
                t => GetEventName(t),
                t => t);

        public static string GetName(Type eventType)
        {
            return GetEventName(eventType);
        }

        public static Type Resolve(string eventName)
        {
            if (!_eventTypes.TryGetValue(eventName, out var type))
            {
                throw new InvalidOperationException(
                    $"Unknown integration event '{eventName}'");
            }

            return type;
        }

        private static string GetEventName(Type type)
        {
            var attribute = type.GetCustomAttribute<IntegrationEventAttribute>();

            if (attribute is null)
            {
                throw new InvalidOperationException(
                    $"Missing IntegrationEventAttribute on '{type.Name}'");
            }

            return attribute.Name;
        }
    }
}
