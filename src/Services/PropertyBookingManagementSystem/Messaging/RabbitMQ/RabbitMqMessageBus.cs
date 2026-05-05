using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Messaging.Abstractions;

namespace Messaging.RabbitMQ
{
    public class RabbitMqMessageBus : IMessageBus
    {
        public Task PublishAsync<T>(T message)
        {
            var json = JsonSerializer.Serialize(message);

            Console.WriteLine($"[RabbitMQ MOCK] Publishing: {typeof(T).Name}");
            Console.WriteLine(json);

            return Task.CompletedTask;
        }
    }
}
