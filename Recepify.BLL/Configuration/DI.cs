using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Recepify.BLL.Services;
using Recepify.DLL.Configuration;

namespace Recepify.BLL.Configuration;

public static class DI
{
    public static void AddBusinessLayerDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        services.AddDataLayerDependencies(configuration);

        services.AddScoped<ProductService>();
        services.AddScoped<ProductCategoryService>();
        services.AddScoped<ReceiptService>();
    }
}
