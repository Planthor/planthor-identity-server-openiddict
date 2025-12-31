namespace PlanthorIdentityServer.Options;

/// <summary>
/// Represents the configuration settings required for Facebook authentication.
/// </summary>
public class FacebookOptions
{
    /// <summary>
    /// The configuration section name in appsettings.json or environment variables.
    /// </summary>
    public const string SectionName = "Authentication:Facebook";

    /// <summary>
    /// Gets the Client ID (App ID) assigned by Facebook for the application.
    /// </summary>
    public string ClientId { get; init; } = string.Empty;

    /// <summary>
    /// Gets the Client Secret (App Secret) assigned by Facebook for the application.
    /// </summary>
    public string ClientSecret { get; init; } = string.Empty;
}