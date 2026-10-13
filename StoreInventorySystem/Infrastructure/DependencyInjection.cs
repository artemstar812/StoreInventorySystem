using Confluent.Kafka;
using InventorySystem.Application.Interfaces;
using InventorySystem.Infrastructure.Messaging.Consumers;
using InventorySystem.Infrastructure.Repositories;
using StoreInventorySystem.Application.Interfaces;
using StoreInventorySystem.Infrastructure.Caching;
using StoreInventorySystem.Infrastructure.Repositories;

namespace StoreInventorySystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProcessedMessagesRepository, ProcessedMessageRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<ICacheService, RedisCacheService>();

            services.AddSingleton<IConsumer<string, string>>(_ =>
            {
                var config = new ConsumerConfig
                {
                    BootstrapServers = "localhost:9094",
                    GroupId = "inventory-service",
                    AutoOffsetReset = AutoOffsetReset.Earliest
                };

                return new ConsumerBuilder<string, string>(config).Build();
            });

            services.AddHostedService<OrderFailedConsumer>();

            return services;
        }
    }
}
