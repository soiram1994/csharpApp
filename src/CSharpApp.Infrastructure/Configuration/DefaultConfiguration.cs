using Microsoft.Extensions.Configuration;

namespace CSharpApp.Infrastructure.Configuration;

public static class DefaultConfiguration
{
    public static IServiceCollection AddDefaultConfiguration(this IServiceCollection services, IConfiguration config)
    {
        services.AddHttpClient<ITodoService, TodoService>(cfg => { cfg.BaseAddress = new Uri(config["BaseUrl"]!); });

        return services;
    }
}