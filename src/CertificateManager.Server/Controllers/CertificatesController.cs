using Microsoft.AspNetCore.Mvc;

namespace CertificateManager.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CertificatesController : ControllerBase
{
    private readonly ILogger<CertificatesController> _logger;

    public CertificatesController(ILogger<CertificatesController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { Message = "Certificates API is working!" });
    }

    [HttpGet("test")]
    public IActionResult TestConnection()
    {
        _logger.LogInformation("Test connection endpoint called");
        return Ok(new { 
            Status = "Success", 
            Message = "API connection is working!",
            Timestamp = DateTime.UtcNow,
            Version = typeof(Program).Assembly.GetName().Version?.ToString() ?? "1.0.0"
        });
    }
}
