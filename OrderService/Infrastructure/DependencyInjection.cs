using Confluent.Kafka;
using OrderService.Application.Interfaces;
using OrderService.Infrastructure.Repositories;

namespace OrderService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IOrderRepository, OrderRepository>();

            services.AddSingleton<IProducer<string, string>>(_ =>
            {
                var config = new ProducerConfig
                {
                    BootstrapServers = "localhost:9092"
                };

                return new ProducerBuilder<string, string>(config).Build();
            });

            services.AddScoped<IEventProducer, KafkaEventProducer>();

            return services;
        }
    }
}
