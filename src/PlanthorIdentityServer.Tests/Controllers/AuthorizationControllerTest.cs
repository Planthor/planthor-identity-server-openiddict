using System.Net;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using OpenIddict.Abstractions;

namespace PlanthorIdentityServer.Tests.Controllers;

public class AuthorizationControllerTest : IClassFixture<IdentityServerFactory<Program>>
{
    private readonly IdentityServerFactory<Program> _factory;
    private readonly HttpClient _client;

    public AuthorizationControllerTest(IdentityServerFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions { AllowAutoRedirect = false });
    }

    [Fact]
    public async Task Authorize_WhenMaxAgeExceeded_ShouldRedirectToLogin()
    {
        // 1. Arrange: Setup a user session issued "Now"
        var loginTime = DateTimeOffset.UtcNow;
        _factory.FakeTime.SetUtcNow(loginTime);

        // TODO - Trung: Mock a login session (usually done via a test-specific auth handler or seed data)
        // await SeedRequiredData();

        // 2. Act: Advance time by 1 hour
        _factory.FakeTime.Advance(TimeSpan.FromHours(1));

        // Request with max_age=10 (seconds)
        var response = await _client.GetAsync("/connect/authorize?client_id=test-app&max_age=10&response_type=code...");

        // 3. Assert: Since 1 hour > 10 seconds, it should Challenge (302 to Login)
        Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);
        Assert.NotNull(response);
        Assert.NotNull(response.Headers.Location);
        Assert.Contains("/Identity/Account/Login", response.Headers.Location.ToString());
    }

    // private async Task SeedRequiredData()
    // {
    //     using var scope = _factory.Services.CreateScope();
    //     var manager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
    //     // Add your client application setup here

    // }
}