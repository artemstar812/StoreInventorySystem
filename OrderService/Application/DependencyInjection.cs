using Confluent.Kafka;
using OrderService.Application.Interfaces;

namespace OrderService.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, Services.OrderService>();

            return services;
        }
    }
}
