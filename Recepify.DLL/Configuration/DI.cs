using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Recepify.DLL.Entities;

namespace Recepify.DLL.Configuration;

public static class DI
{
    public static void AddDataLayerDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<RecepifyContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("Default"));
        });
        services.AddIdentityCore<User>(options =>
            {
                options.User.RequireUniqueEmail = false;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
            })
            .AddRoles<Role>()
            .AddSignInManager()
            .AddEntityFrameworkStores<RecepifyContext>();
    }
}