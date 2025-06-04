using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Net.Http;
using Microsoft.Extensions.Logging; // Added for logging

namespace CertificateManager.Client;

public class PersistentAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<PersistentAuthenticationStateProvider> _logger;
    private static readonly AuthenticationState _unauthenticatedState = new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));

    public PersistentAuthenticationStateProvider(IHttpClientFactory httpClientFactory, ILogger<PersistentAuthenticationStateProvider> logger)
    {
        _httpClient = httpClientFactory.CreateClient("ServerAPI"); // Use the named HttpClient
        _logger = logger;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        try
        {
            // The /manage/info endpoint is part of ASP.NET Core Identity Endpoints
            // It returns user claims if the authentication cookie is valid.
            var user = await _httpClient.GetFromJsonAsync<UserInfo?>("manage/info");

            if (user?.Email != null) // Check for a key piece of user info
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email), // Typically email is used as Name
                    new Claim(ClaimTypes.Email, user.Email)
                    // Add other claims from UserInfo as needed
                };

                foreach (var kvp in user.Claims ?? new Dictionary<string, string>())
                {
                    claims.Add(new Claim(kvp.Key, kvp.Value));
                }

                var identity = new ClaimsIdentity(claims, "IdentityEndpoints");
                return new AuthenticationState(new ClaimsPrincipal(identity));
            }
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.Unauthorized)
        {
            _logger.LogInformation("User is not authenticated (manage/info returned Unauthorized).");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching user info from /manage/info for AuthenticationState.");
        }

        return _unauthenticatedState;
    }

    // Call this method after a successful login/logout to update the auth state
    public void NotifyAuthenticationStateChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}

// DTO for the /manage/info endpoint response
public class UserInfo
{
    public string? Email { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public Dictionary<string, string>? Claims { get; set; }
}


// Simple DelegatingHandler to ensure credentials (cookies) are included if needed.
public class CookieHandler : DelegatingHandler
{
    public CookieHandler()
    {
        // The HttpClientFactory will set the InnerHandler. 
        // Manually setting it here can cause the 'InnerHandler must be null' error.
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // The SetBrowserRequestCredentials call is in CertificateService.cs, which is good.
        // This handler can remain simple for now.
        return await base.SendAsync(request, cancellationToken);
    }
}
