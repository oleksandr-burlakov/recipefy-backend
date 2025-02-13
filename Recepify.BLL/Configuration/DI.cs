using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recepify.DLL.Configuration;

namespace Recepify.BLL.Configuration;

public static class DI
{
    public static void AddBusinessLayerDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDataLayerDependencies(configuration);
    }
}