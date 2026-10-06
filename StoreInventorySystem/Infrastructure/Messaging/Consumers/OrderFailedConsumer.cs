using Confluent.Kafka;
using Shared.Contracts.Events;
using StoreInventorySystem.Application.Services;
using System.Text.Json;

namespace InventorySystem.Infrastructure.Messaging.Consumers
{
    public class OrderFailedConsumer : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IConsumer<string, string> _consumer;
                
        public OrderFailedConsumer(IServiceScopeFactory scopeFactory, IConsumer<string, string> consumer)
        {
            _scopeFactory = scopeFactory;
            _consumer = consumer;

            _consumer.Subscribe("order-failed");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while(!stoppingToken.IsCancellationRequested)
            {
                var result = _consumer.Consume(stoppingToken);

                var @event = JsonSerializer.Deserialize<OrderFailedEvent>(result.Message.Value);

                if (@event == null)
                    continue;

                using var scope = _scopeFactory.CreateScope();

                var productService = scope.ServiceProvider.GetRequiredService<ProductService>();

                await productService.AddProductAmount(@event.ProductId, @event.Quantity);
            }
        }

        public override void Dispose()
        {
            _consumer.Close();
            _consumer.Dispose();
            base.Dispose();
        }
    }
}
