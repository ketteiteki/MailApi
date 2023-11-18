using IdentityModel.Client;
using Mail.Domain.Constants;
using Mail.Domain.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Mail.WebApi.DependencyInjection;

public static class AuthenticationServices
{
    public static IServiceCollection AddAuthenticationServices(
        this IServiceCollection serviceCollection,
        IConfiguration configuration)
    {
        var keyCloakConfiguration = configuration.GetSection(AppSettingsConstants.KeyCloak).Get<KeyCloakConfiguration>();
        
        serviceCollection
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.Cookie.Name = "keycloak.cookie";
                options.Cookie.MaxAge = TimeSpan.FromDays(2);
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.SlidingExpiration = true;

                options.Events = new CookieAuthenticationEvents
                {
                    OnValidatePrincipal = ValidatePrincipal
                };
            })
            .AddOpenIdConnect(options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.Authority = keyCloakConfiguration.ServerRealm;
                options.ClientId = keyCloakConfiguration.ClientId;
                options.ClientSecret = keyCloakConfiguration.ClientSecret;
                options.MetadataAddress = keyCloakConfiguration.Metadata;
                options.RequireHttpsMetadata = false;
                options.GetClaimsFromUserInfoEndpoint = true;
                options.Scope.Add(OpenIdConnectScope.OpenIdProfile);
                options.SaveTokens = true;
                options.ResponseType = OpenIdConnectResponseType.Code;
                options.NonceCookie.SameSite = SameSiteMode.Unspecified;
                options.CorrelationCookie.SameSite = SameSiteMode.Unspecified;
            });

        return serviceCollection;
    }

    private static async Task ValidatePrincipal(CookieValidatePrincipalContext context)
    {
        var configuration = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
        var httpContext = context.HttpContext.RequestServices.GetRequiredService<HttpClient>();
        
        var keyCloakConfiguration = configuration.GetSection(AppSettingsConstants.KeyCloak).Get<KeyCloakConfiguration>();
        var hasAuthorizeAttribute = context.HttpContext.GetEndpoint()?.Metadata.GetMetadata<AuthorizeAttribute>() != null;
        
        if (!hasAuthorizeAttribute)
        {
            return;
        }

        var getExpiresAtResult = context.Properties.Items.TryGetValue(KeyCloakClaimConstants.ExpiresAt, out var expireAt);
        
        if (!getExpiresAtResult || expireAt == null)
        {
            context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            return;
        }

        var expireAtDate = DateTime.Parse(expireAt).ToUniversalTime();

        if (expireAtDate < DateTime.UtcNow)
        {
            if (!context.Properties.Items.TryGetValue(KeyCloakClaimConstants.RefreshToken, out var token) || token == null)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            } 
            
            var tokenResponse = await RequestRefreshTokenAsync(token);

            if (tokenResponse.AccessToken == null || tokenResponse.RefreshToken == null || tokenResponse.IdentityToken == null)
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return;
            }
            
            context.Properties.UpdateTokenValue(KeyCloakClaimConstants.AccessToken, tokenResponse.AccessToken);
            context.Properties.UpdateTokenValue(KeyCloakClaimConstants.RefreshToken, tokenResponse.AccessToken);
            context.Properties.UpdateTokenValue(KeyCloakClaimConstants.IdToken, tokenResponse.IdentityToken);

            context.ShouldRenew = true;
        }
        
        async Task<TokenResponse> RequestRefreshTokenAsync(string refreshToken)
        {
            var refreshTokenRequest = new RefreshTokenRequest
            {
                Address = keyCloakConfiguration.TokenUrl,
                ClientId = keyCloakConfiguration.ClientId,
                ClientSecret = keyCloakConfiguration.ClientSecret,
                GrantType = OpenIdConnectGrantTypes.RefreshToken,
                Scope = OpenIdConnectScope.OpenIdProfile,
                RefreshToken = refreshToken
            };

            return await httpContext.RequestRefreshTokenAsync(refreshTokenRequest);
        }
    }
}