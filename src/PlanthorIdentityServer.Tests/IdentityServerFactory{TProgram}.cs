using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Time.Testing;

using PlanthorIdentityServer.Data;

namespace PlanthorIdentityServer.Tests;

public class IdentityServerFactory<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
    public FakeTimeProvider FakeTime { get; } = new FakeTimeProvider();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // 1. Swap the DB for In-Memory
            var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
            if (descriptor != null) services.Remove(descriptor);
            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("TestDb"));

            // 2. Inject our FakeTimeProvider
            services.AddSingleton<TimeProvider>(FakeTime);
        });
    }
}