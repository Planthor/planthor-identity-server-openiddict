using Microsoft.AspNetCore.Mvc.Testing;

namespace PlanthorIdentityServer.Tests.Views.Shared;

public class LoginPartialTests : IClassFixture<IdentityServerFactory<Program>>
{
    private readonly IdentityServerFactory<Program> _factory;
    private readonly HttpClient _client;

    public LoginPartialTests(IdentityServerFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(
            new WebApplicationFactoryClientOptions
            {
                AllowAutoRedirect = false,
                BaseAddress = new Uri("https://localhost:44313/")
            });
    }

    [Fact]
    public async Task AnonymousUser_Sees_LoginAndRegisterLinks()
    {
        // Arrange

        // Act
        var response = await _client.GetAsync("/Identity/Account/Login?ReturnUrl=%2Fconnect%2Fauthorize%3Fclient_id%3Dclient-test%26response_type%3Dcode%26code_challenge%3DhKpKupTM391pE10xfQiorMxXarRKAHRhTfH_xkGf7U4"); // Requesting Home page

        var content = await response.Content.ReadAsStringAsync();
        // Parse HTML using AngleSharp


        // Assert
        response.EnsureSuccessStatusCode();

        // 1. Verify "Login" and "Register" link exists
        Assert.Contains("asp-page=\"/Account/Register\"", content);
        Assert.Contains("asp-page=\"/Account/Login\"", content);
    }
}