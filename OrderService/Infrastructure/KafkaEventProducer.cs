using Confluent.Kafka;
using OrderService.Application.Interfaces;
using System.Text.Json;
using Shared.Contracts.Events;
using Shared.Contracts.Events.Interfaces;

namespace OrderService.Infrastructure
{
    public class KafkaEventProducer : IEventProducer
    {
        public readonly IProducer<string, string> _producer;

        public KafkaEventProducer(IProducer<string, string> producer)
        {
            _producer = producer;
        }

        public async Task PublishAsync<T>(T @event) where T : IIntegrationEvent
        {
            var message = JsonSerializer.Serialize(@event);

            await _producer.ProduceAsync(@event.Topic, new Message<string, string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = message
            });
        }
    }
}
