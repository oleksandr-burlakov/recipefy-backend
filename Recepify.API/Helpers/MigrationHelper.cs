using Microsoft.EntityFrameworkCore;
using Recepify.DLL;

namespace Recepify.API.Helpers;

public static class MigrationHelper
{
    public static void ApplyMigrations(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<RecepifyContext>();
        dbContext.Database.EnsureCreated();
        dbContext.Database.Migrate();
    }
}