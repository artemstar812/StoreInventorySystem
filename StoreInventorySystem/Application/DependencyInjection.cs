using StoreInventorySystem.Application.Services;

namespace StoreInventorySystem.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<ProductService>();
            
            return services;
        }
    }
}
