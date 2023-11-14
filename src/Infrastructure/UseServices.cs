using MillerDemo.Application.Persistence;

namespace MillerDemo.Infrastructure;

/// <summary>
/// Contains extensions methods for using infrastructure services.
/// </summary>
public static class UseServices
{
    /// <summary>
    /// Migrates the database.
    /// </summary>
    /// <param name="app">The web application.</param>
    /// <returns>An awaitable task.</returns>
    public static async Task MigrateDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<IApplicationDbContext>();
        await dbContext.MigrateAsync();
    }
}