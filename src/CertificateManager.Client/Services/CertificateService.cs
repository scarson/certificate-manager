using System.Net.Http.Json;
using System.Text.Json;

namespace CertificateManager.Client.Services;

public class CertificateService : ICertificateService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<CertificateService> _logger;

    public CertificateService(HttpClient httpClient, ILogger<CertificateService> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<string> GetTestMessageAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("api/certificates/test");
            
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
}
