using System.Net.Http.Json;
using System.Text.Json;

namespace CertificateManager.Client.Services;

public interface ICertificateService
{
    Task<string> GetTestMessageAsync();
}
