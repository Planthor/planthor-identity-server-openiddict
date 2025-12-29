using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;

using PlanthorIdentityServer.Data;

using Testcontainers.PostgreSql;

namespace PlanthorIdentityServer.Tests;

public class IdentityServerFactory<TProgram> : WebApplicationFactory<TProgram>, IAsyncLifetime where TProgram : class
{
    public FakeTimeProvider FakeTime { get; } = new FakeTimeProvider();

    // Define the PostgreSQL container
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder()
        .WithImage("postgres:18")
        .WithDatabase("PLANTHOR_IDENTITY")
        .WithUsername("planthor-admin")
        .WithPassword("Planthor@123")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureAppConfiguration((context, config) =>
        {
            // Inject the dynamic connection string from the container
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:DefaultConnection"] = _dbContainer.GetConnectionString()
            });
        });

        builder.ConfigureServices(services =>
        {
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            db.Database.EnsureCreated();

            // Inject our FakeTimeProvider
            services.AddSingleton<TimeProvider>(FakeTime);
        });
    }

    /// <inheritdoc />
    public async Task InitializeAsync() => await _dbContainer.StartAsync();

    /// <inheritdoc />
    async Task IAsyncLifetime.DisposeAsync()
    {
        await _dbContainer.StopAsync();
    }
}