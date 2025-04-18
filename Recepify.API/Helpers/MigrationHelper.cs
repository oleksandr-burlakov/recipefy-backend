using Microsoft.EntityFrameworkCore;
using Recepify.DLL;
using Recepify.DLL.Helpers;

namespace Recepify.API.Helpers;

public static class MigrationHelper
{
    public static async Task ApplyMigrationsAsync(IApplicationBuilder app)
    {
        try
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<RecepifyContext>();
            await DatabaseHelpers.EnsureDatabaseCreatedAsync(dbContext);
            await DatabaseHelpers.MigrateDatabaseAsync(dbContext);
            //dbContext.Database.Migrate();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}