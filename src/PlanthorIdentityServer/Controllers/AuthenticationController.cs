using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;

using OpenIddict.Abstractions;
using OpenIddict.Client.AspNetCore;

using static OpenIddict.Abstractions.OpenIddictConstants;

namespace PlanthorIdentityServer.Controllers;

[Route("/")]
public class AuthenticationController : Controller
{
    [HttpGet("callback/login/{provider}")]
    [HttpPost("callback/login/{provider}")]
    public async Task<ActionResult> LogInCallback()
    {
        var result = await HttpContext.AuthenticateAsync(OpenIddictClientAspNetCoreDefaults.AuthenticationScheme);

        if (!result.Succeeded
            && result.Principal?.Identity != null
            && !result.Principal.Identity.IsAuthenticated)
        {
            throw new InvalidOperationException("The external authorization data cannot be used for authentication.");
        }

        // Build an identity based on the external claims and that will be used to create the authentication cookie.
        var identity = new ClaimsIdentity(
            authenticationType: "ExternalLogin",
            nameType: ClaimTypes.Name,
            roleType: ClaimTypes.Role);

        // By default, OpenIddict will automatically try to map the email/name and name identifier claims from
        // their standard OpenID Connect or provider-specific equivalent, if available. If needed, additional
        // claims can be resolved from the external identity and copied to the final authentication cookie.
        identity.SetClaim(ClaimTypes.Email, result.Principal?.GetClaim(ClaimTypes.Email))
                .SetClaim(ClaimTypes.Name, result.Principal?.GetClaim(ClaimTypes.Name))
                .SetClaim(ClaimTypes.NameIdentifier, result.Principal?.GetClaim(ClaimTypes.NameIdentifier));

        // Preserve the registration details to be able to resolve them later.
        identity.SetClaim(Claims.Private.RegistrationId, result.Principal?.GetClaim(Claims.Private.RegistrationId))
                .SetClaim(Claims.Private.ProviderName, result.Principal?.GetClaim(Claims.Private.ProviderName));

        if (result.Properties != null)
        {
            // Build the authentication properties based on the properties that were added when the challenge was triggered.
            var properties = new AuthenticationProperties(result.Properties.Items)
            {
                RedirectUri = result.Properties.RedirectUri ?? "/",
                IssuedUtc = null,
                ExpiresUtc = null,
                IsPersistent = false
            };

            // If needed, the tokens returned by the authorization server can be stored in the authentication cookie.
            // To make cookies less heavy, tokens that are not used are filtered out before creating the cookie.
            properties.StoreTokens(result.Properties.GetTokens().Where(token => token.Name is
                // Preserve the access and refresh tokens returned in the token response, if available.
                OpenIddictClientAspNetCoreConstants.Tokens.BackchannelAccessToken or
                OpenIddictClientAspNetCoreConstants.Tokens.RefreshToken));

            // Ask the default sign-in handler to return a new cookie and redirect the
            // user agent to the return URL stored in the authentication properties.
            //
            // For scenarios where the default sign-in handler configured in the ASP.NET Core
            // authentication options shouldn't be used, a specific scheme can be specified here.
            return SignIn(new ClaimsPrincipal(identity), properties);
        }
        throw new InvalidOperationException("Failed to get information from external login provider");
    }
}