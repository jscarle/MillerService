using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MillerDemo.Application.Persistence;
using MillerDemo.Infrastructure.Persistence;
using MillerDemo.Infrastructure.Services;

namespace MillerDemo.Infrastructure;

/// <summary>
/// Contains extensions methods for configuring infrastructure services.
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    /// Adds infrastructure services to the service collection.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The service collection.</returns>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddSingleton<IDnsService, FakeDnsService>();
        services.AddHostedService<JobService>();
        services.AddPersistence();

        return services;
    }

    private static void AddPersistence(this IServiceCollection services)
    {
        services.AddDbContext();
        services.AddScoped<IApplicationDbContext, SqlServerDbContext>();
    }

    private static void AddDbContext(this IServiceCollection services)
    {
        services.AddDbContextPool<SqlServerDbContext>((provider, options) =>
        {
            var configuration = provider.GetRequiredService<IConfiguration>();
            var dbServer = configuration.GetValue<string>("DB_SERVER") ?? throw new InvalidOperationException("Could not get value for DB_SERVER.");
            var dbName = configuration.GetValue<string>("DB_NAME") ?? throw new InvalidOperationException("Could not get value for DB_NAME.");
            var dbUsername = configuration.GetValue<string>("DB_USERNAME") ?? throw new InvalidOperationException("Could not get value for DB_USERNAME.");
            var dbPassword = configuration.GetValue<string>("DB_PASSWORD") ?? throw new InvalidOperationException("Could not get value for DB_PASSWORD.");
            var connectionString = BuildConnectionString(dbServer, dbName, dbUsername, dbPassword);
            options.UseSqlServer(connectionString)
                .ConfigureWarnings(w => w.Ignore(SqlServerEventId.DecimalTypeKeyWarning));
        });
    }

    private static string BuildConnectionString(string dbServer, string dbName, string dbUsername, string dbPassword)
    {
        return new SqlConnectionStringBuilder
        {
            DataSource = dbServer,
            InitialCatalog = dbName,
            UserID = dbUsername,
            Password = dbPassword,
            Authentication = SqlAuthenticationMethod.SqlPassword,
            TrustServerCertificate = true
        }.ToString();
    }
}