using System.Net.Http.Json;
using System.Text.Json;
using CertificateManager.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Http;

namespace CertificateManager.Client.Services;

public class CertificateService : ICertificateService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CertificateService> _logger;
    private readonly AuthenticationStateProvider _authStateProvider;

    public CertificateService(HttpClient httpClient, 
                           ILogger<CertificateService> logger,
                           AuthenticationStateProvider authStateProvider)
    {
        _httpClient = httpClient;
        _logger = logger;
        _authStateProvider = authStateProvider;
    }

    public async Task<string> GetTestMessageAsync()
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "api/certificates/test");
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            var response = await _httpClient.SendAsync(request);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(content))
                {
                    var json = JsonDocument.Parse(content).RootElement;
                    if (json.TryGetProperty("Message", out var messageProp) && 
                        messageProp.ValueKind == JsonValueKind.String)
                    {
                        return messageProp.GetString() ?? "No message content";
                    }
                    // Fallback to raw content if message property not found
                    return content;
                }
                return "Empty response from server";
            }
            return $"Error: {response.StatusCode} - {response.ReasonPhrase}";
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError(httpEx, "HTTP Request error");
            return $"Network error: {httpEx.Message}";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching test message from API");
            return $"Error: {ex.Message}";
        }
    }

    public async Task<List<Certificate>?> GetAllCertificatesAsync()
    {
        _logger.LogInformation("Attempting to retrieve all certificates from API.");
        
        try
        {
            // Create a new request
            var request = new HttpRequestMessage(HttpMethod.Get, "api/certificates");
            
            // Add authorization if user is authenticated
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            if (authState.User.Identity?.IsAuthenticated == true)
            {
                request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            }
            
            // Send the request
            var response = await _httpClient.SendAsync(request);
            
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("Successfully retrieved certificates from API.");
                return JsonSerializer.Deserialize<List<Certificate>>(content, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            else
            {
                _logger.LogError("Failed to retrieve certificates. Status code: {StatusCode}", response.StatusCode);
                return new List<Certificate>(); // Return empty list instead of null
            }
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError(httpEx, "HTTP request error while fetching all certificates.");
            return new List<Certificate>(); // Return empty list instead of null
        }
        catch (JsonException jsonEx)
        {
            _logger.LogError(jsonEx, "JSON deserialization error while fetching all certificates.");
            return new List<Certificate>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while fetching all certificates.");
            return new List<Certificate>();
        }
    }
}
