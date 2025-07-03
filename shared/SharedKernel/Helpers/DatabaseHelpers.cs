
using Microsoft.EntityFrameworkCore;

namespace SharedKernel.Helpers;

public static class DatabaseHelpers
{
    public static async Task EnsureDatabaseCreatedAsync<TDbContext>(TDbContext context)
        where TDbContext : DbContext
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        try
        {
            if (await context.Database.EnsureCreatedAsync())
            {
                // Database was created. You might want to seed data here.
                Console.WriteLine("Database created.");
            }
            else
            {
                // Database already exists.
                Console.WriteLine("Database already exists.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while creating or checking the database: {ex.Message}");
            //Handle Exception appropriately.
            throw; //rethrow to allow calling code to handle the exception, or handle here.
        }
    }

    //Synchronous version if needed, but async is typically preferred.
    public static void EnsureDatabaseCreated<TDbContext>(TDbContext context)
        where TDbContext : DbContext
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        try
        {
            if (context.Database.EnsureCreated())
            {
                Console.WriteLine("Database created.");
            }
            else
            {
                Console.WriteLine("Database already exists.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred while creating or checking the database: {ex.Message}");
            throw;
        }
    }

    public static async Task MigrateDatabaseAsync<TDbContext>(TDbContext context) where TDbContext : DbContext
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        try
        {
            var pending = await context.Database.GetPendingMigrationsAsync();
            if (pending.Any())
            {
                await context.Database.MigrateAsync();
            }            
            //await context.Database.MigrateAsync();
            Console.WriteLine("Database migration complete.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            Console.WriteLine($"An error occurred during database migration: {ex.Message}");
            throw;
        }
    }

    public static void MigrateDatabase<TDbContext>(TDbContext context) where TDbContext : DbContext
    {
        if (context == null)
        {
            throw new ArgumentNullException(nameof(context));
        }

        try
        {
            context.Database.Migrate();
            Console.WriteLine("Database migration complete.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during database migration: {ex.Message}");
            throw;
        }
    }
}