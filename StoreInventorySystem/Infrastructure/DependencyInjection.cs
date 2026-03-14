using StoreInventorySystem.Application.Interfaces;
using StoreInventorySystem.Infrastructure.Repositories;

namespace StoreInventorySystem.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<IProductRepository, ProductRepository>();

            return services;
        }
    }
}
