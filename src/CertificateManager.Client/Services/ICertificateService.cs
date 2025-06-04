using System.Net.Http.Json;
using System.Text.Json;
using CertificateManager.Shared.Models; // Added for shared Certificate model

namespace CertificateManager.Client.Services;

public interface ICertificateService
{
    Task<string> GetTestMessageAsync();
    Task<List<Certificate>?> GetAllCertificatesAsync(); // Method to get all certificates
}
