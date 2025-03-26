using Microsoft.EntityFrameworkCore;
using Recepify.DLL;

namespace Recepify.API.Helpers;

public static class MigrationHelper
{
    public static void ApplyMigrations(IApplicationBuilder app)
    {
        try
        {
            using var scope = app.ApplicationServices.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<RecepifyContext>();
            dbContext.Database.Migrate();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
    }
}