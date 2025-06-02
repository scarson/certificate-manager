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
            var response = await _httpClient.GetFromJsonAsync<JsonElement>("api/certificates/test");
            return response.GetProperty("Message").GetString() ?? "No message received";
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching test message from API");
            return $"Error: {ex.Message}";
        }
    }
}
