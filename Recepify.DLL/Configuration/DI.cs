using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Recepify.DLL.Configuration;

public static class DI
{
    public static void AddDataLayerDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RecepifyContext>(opt =>
        {
            opt.UseNpgsql(configuration.GetConnectionString("Default"));
        });
    }
}